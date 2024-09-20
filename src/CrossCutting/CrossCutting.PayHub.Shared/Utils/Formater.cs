namespace CrossCutting.PayHub.Shared.Utils
{
    public static class Formater
    {
        public static string FormatDate(DateTime date)
        {
            // 31/12/2023 --> "2023-12-31"
            return date.ToString("yyyy-MM-dd");
        }

        public static string FormatValue(decimal value)
        {
            // 1234 --> "1234.00"
            // 1234.567 --> "1234.56"
            return value.ToString("0.00").Replace(",", ".");
        }

        public static string FormatPercentual(decimal percentual)
        {
            // 20 --> "0000020.00000"
            // 25.76 --> "0000025.76000"
            return percentual.ToString("0000000.00000").Replace(",", ".");
        }

        public static string FormatValueItauPayload(decimal value)
        {
            return value.ToString("000000000000000.00").Replace(",", "").Replace(".", "");
        }

        public static string FormatPercentualItauPayload(decimal value)
        {
            return value.ToString("0000000.00000").Replace(",", "").Replace(".", "");
        }

        public static string FormatOurNumber(int value)
        {
            return value.ToString("00000000");
        }

        public static string FormatDebtAmout(decimal value)
        {
            return Math.Round(value, 2).ToString();
        }
    }
}
