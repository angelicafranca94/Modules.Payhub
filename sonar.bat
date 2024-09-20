call set PARENT_DIR=%CD%
set PARENT_DIR=%PARENT_DIR:\= %
set LAST_WORD=
for %%i in (%PARENT_DIR%) do set LAST_WORD=%%i
echo dotnet:%LAST_WORD%

dotnet sonarscanner begin /k:"dotnet:%LAST_WORD%" /d:sonar.host.url="https://sonar.fiap.com.br"  /d:sonar.login=%SONAR_TOKEN%
dotnet build
dotnet sonarscanner end /d:sonar.login=%SONAR_TOKEN%