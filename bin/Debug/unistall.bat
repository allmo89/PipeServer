@ECHO OFF
REM The Following directory is for .NET 4.0
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%

echo Uninstalling PipeServer......
echo ---------------------------
echo Stoping Service
net stop PipeService
InstallUtil /u PipeServer.exe
echo ---------------------------
echo Done.