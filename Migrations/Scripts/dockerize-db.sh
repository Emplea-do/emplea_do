printGreenWithBorder () {
    green=$(tput setaf 10)
    NC='\033[0m' # No Color
    printf "${green}-%.0s" {1..80}; printf "\n"
    printf "${1}\n"
    printf "${green}-%.0s" {1..80}; printf "\n${NC}"
}

printGreenWithBorder "Creating a new container and volume for SQL Server"

docker run \
    -e "ACCEPT_EULA=Y" \
    -e "SA_PASSWORD=MyPass@word" \
    -e "MSSQL_PID=Developer" \
    -p 1439:1433 \
    -v "empleado-db-volume:/var/opt/mssql" \
    --name=empleado-db \
    -d \
    mcr.microsoft.com/mssql/server:latest

printf "\n"

countdownInSeconds(){
    seconds=${1} # seconds to wait
    rewrite="\e[25D\e[1A\e[K"
    while [ $seconds -gt 0 ]; do 
        seconds=$((seconds-1))
        sleep 1
        echo -e "${rewrite}$seconds seconds to go..."
    done
    echo -e "${rewrite}Done wating...\n"
}

printGreenWithBorder "Wating for container to be ready"
printf "\n"

countdownInSeconds 60

printGreenWithBorder "Creating database if not exits"

sqlcmd -S localhost,1439 -U SA -P "MyPass@word" -Q "IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'EmpleadoDB') CREATE DATABASE EmpleadoDB"

printf "Done.\n\n"