
# Getting Fluent Migrator CLI to Run Migrations
dotnet tool install -g FluentMigrator.DotNet.Cli --version 3.2.1

#Getting libman CLI for js package management
dotnet tool install -g Microsoft.Web.LibraryManager.Cli 

#Ignore changes made to the appsettings.Development.json file
git update-index --assume-unchanged Web/appsettings.Development.json

#Moves to web project

cd Web/
#Restore js dependencies
libman restore