@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mCreating a new container and volume for SQL Server[0m
@ECHO [92m--------------------------------------------------------------------------------[0m

docker run ^
    -e "ACCEPT_EULA=Y" ^
    -e "SA_PASSWORD=MyPass@word" ^
    -e "MSSQL_PID=Developer" ^
    -p 1439:1433 ^
    -v "empleado-db-volume:/var/opt/mssql" ^
    --name=empleado-db ^
    -d ^
    mcr.microsoft.com/mssql/server:latest

@ECHO.

@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mWating for container to be ready[0m
@ECHO [92m--------------------------------------------------------------------------------[0m

@ECHO off&SetLocal EnableExtensions EnableDelayedExpansion

@REM
SET /A countdownInSeconds=60

@REM Prepare carriage return
FOR /F %%a IN ('COPY /Z "%~dpf0" NUL') DO set "carret=%%a"

SET /p =%countdownInSeconds% seconds to go...!carret!  <NUL
FOR /L %%S IN (%countdownInSeconds%,-1,1) DO (
    SET /p =%%S seconds to go...!carret!  <NUL
    TIMEOUT /T 1 >NUL
)

@ECHO Done waiting...    

@ECHO.

@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mCreating database if not exits[0m
@ECHO [92m--------------------------------------------------------------------------------[0m

sqlcmd -S localhost,1439 -U SA -P "MyPass@word" -Q "IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'EmpleadoDB') CREATE DATABASE EmpleadoDB"

@ECHO Done.
@ECHO.
