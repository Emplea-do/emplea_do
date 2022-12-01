printGreenWithBorder () {
    green=$(tput setaf 10)
    NC='\033[0m' # No Color
    printf "${green}-%.0s" {1..80}; printf "\n"
    printf "${1}\n"
    printf "${green}-%.0s" {1..80}; printf "\n${NC}"
}

printGreenWithBorder "Stoping and removing database container"
docker ps -q --filter "name=empleado-db" | grep -q . && docker stop empleado-db && docker rm empleado-db || echo Not Found
printf "\n"

printGreenWithBorder "Cleaning Migrations project"
dotnet clean Migrations/Migrations.csproj

printf "\n\n"