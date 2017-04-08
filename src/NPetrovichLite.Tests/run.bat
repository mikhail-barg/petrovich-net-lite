@echo off
@echo "runing nunit"
..\..\..\packages\NUnit.ConsoleRunner.3.6.0\tools\nunit3-console.exe NPetrovichLite.Tests.dll
..\..\..\packages\ReportUnit.1.5.0-beta1\tools\ReportUnit.exe TestResult.xml
start TestResult.html
pause