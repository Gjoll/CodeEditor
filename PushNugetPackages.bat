rem nuget sources add -Name "CodeEditor" -ConfigFile nuget.config -Source "C:\Development\Tools\CodeEditor\Packages-username "KurtA@Gjoll.com" -password "Z"

call :push Packages\CodeEditor.1.0.0.snupkg
exit

:push
	dir %1
	nuget.exe push %1 -Timeout 6000 -ApiKey %GitApiToken% -Source https://api.nuget.org/v3/index.json
	rem ExternalProjects\nuget.exe push %1 %GitApiToken% -Source https://www.myget.org/F/applicadia/api/v2/package
	IF %errorlevel% NEQ 0 GOTO :error
	del CompiledNugetPackages\*.nupkg
	del CompiledNugetPackages\*.snupkg
	exit
	
:error
	echo "*"
	echo "* nuget push failed "
	echo "*"
	pause
	exit /b
	
	
	
	
