@ECHO OFF

@REM Clean up Migrations project
cd Migrations\Scripts\
call cleanup.bat

@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mInstalling Fluent Migrator CLI to Run Migrations[0m
@ECHO [92m--------------------------------------------------------------------------------[0m

dotnet tool install -g FluentMigrator.DotNet.Cli --version 3.2.7

@ECHO.

@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mInstalling Libman CLI for js package management[0m
@ECHO [92m--------------------------------------------------------------------------------[0m

dotnet tool install -g Microsoft.Web.LibraryManager.Cli

@ECHO.

@REM Moves the project's base path
CD ..\..\

@REM Ignore changes made to the appsettings.Development.json file
git update-index --assume-unchanged Web/appsettings.Development.json

@REM Moves to the Web project
cd Web/

@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mRestoring js dependencies[0m
@ECHO [92m--------------------------------------------------------------------------------[0m
libman restore

@REM @ECHO.

CD ../Migrations/Scripts/
call dockerize-db.bat

@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mRestoring, building and running Migrations[0m
@ECHO [92m--------------------------------------------------------------------------------[0m

@REM Moves to the Migrations project's folder
CD ..

dotnet restore Migrations.csproj
dotnet build Migrations.csproj
dotnet fm migrate -p sqlserver -c "Data Source=localhost,1439;Initial Catalog=EmpleadoDB;User id=sa;Password=MyPass@word;" -a "bin/Debug/netcoreapp3.1/Migrations.dll"

@ECHO.

@REM Moves the project's base path
CD ..

PAUSE