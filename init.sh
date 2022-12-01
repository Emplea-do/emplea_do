printGreenWithBorder () {
    green=$(tput setaf 10)
    noColor='\033[0m'
    printf "${green}-%.0s" {1..80}; printf "\n"
    printf "${1}\n"
    printf "${green}-%.0s" {1..80}; printf "\n${noColor}"
}

# Clean up Migrations project
sh Migrations/Scripts/cleanup.sh

printGreenWithBorder "Instaling Fluent Migrator CLI to Run Migrations"
dotnet tool install -g FluentMigrator.DotNet.Cli --version 3.2.7
printf "\n"

printGreenWithBorder "Installing Libman CLI for js package management"
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
printf "\n"

# Ignore changes made to the appsettings.Development.json file
git update-index --assume-unchanged Web/appsettings.Development.json

# Moves to the Web project
cd Web/

printGreenWithBorder "Restoring js dependencies"
libman restore

# Create SQL Server instance
cd ../
sh Migrations/Scripts/dockerize-db.sh

printGreenWithBorder "Restoring, building and running Migrations"
cd Migrations/
dotnet restore Migrations.csproj
dotnet build Migrations.csproj
dotnet fm migrate -p sqlserver -c "Data Source=localhost,1439;Initial Catalog=EmpleadoDB;User id=sa;Password=MyPass@word;" -a "bin/Debug/netcoreapp3.1/Migrations.dll"

cd ../