@ECHO OFF
REM The Following directory is for .NET 4.0
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%

echo Installing PipeServer......
echo ---------------------------
InstallUtil /i PipeServer.exe
echo ---------------------------
echo Starting Service......
net start PipeService
echo Done.