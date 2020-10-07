
::# Getting Fluent Migrator CLI to Run Migrations
dotnet tool install -g FluentMigrator.DotNet.Cli --version 3.2.1

::# Update Fluent Migrator CLI
dotnet tool upgrade -g FluentMigrator.DotNet.Cli 

::#Getting libman CLI for js package management
dotnet tool install -g Microsoft.Web.LibraryManager.Cli 

::#Ignore changes made to the appsettings.Development.json file
git update-index --assume-unchanged Web/appsettings.Development.json

::#Moves to web project
cd Web/
::#Restore js dependencies
libman restore

cd ../Migrations
dotnet restore Migrations.csproj
dotnet build Migrations.csproj
dotnet fm migrate -p sqlite -c "Data Source=../mydb.db" -a "bin/Debug/netcoreapp2.2/Migrations.dll"

cd ../