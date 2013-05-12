"%~dp0tools\nant\bin\NAnt.exe" -logfile:build.log %*
@SET exitCode=%errorlevel%
@ping -n 2 127.0.0.1 >nul
@EXIT /B %exitCode%