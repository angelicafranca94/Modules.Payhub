using Api.Pix.Domain.Interfaces;
using Api.Pix.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

namespace Api.Pix.Infrastructure.Resiliences;

public class ResiliencePolicy : IResiliencePolicy
{
    private readonly ILogger<ResiliencePolicy> _logger;
    private readonly ResilienceSettings _resilience;

    public ResiliencePolicy(ILogger<ResiliencePolicy> logger, IOptions<ResilienceSettings> resilience)
    {
        _logger = logger;
        _resilience = resilience.Value;
    }

    private AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
         .HandleTransientHttpError()
         .Or<TimeoutRejectedException>()
          .Or<TaskCanceledException>()
             .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(_resilience.RetryTime),
                 onRetry: (exception, retryCount, context) =>
                {
                    _logger.LogWarning("Error: {Exception}... Retry Count: {RetryCount}", exception.Exception.Message, retryCount);
                });
    }

    private AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
         .HandleTransientHttpError()
         .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(_resilience.CircuitBreakTime),
                onBreak: (exception, timespan) =>
                {
                    _logger.LogError("Circuit breaker open for {Timespan}", timespan);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker reset.");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit half open.");
                });
    }

    public async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> action)
    {
        var retryPolicy = GetRetryPolicy();
        var circuitBreakerPolicy = GetCircuitBreakerPolicy();

        return await retryPolicy.WrapAsync(circuitBreakerPolicy).ExecuteAsync(action);
    }
}