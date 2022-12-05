@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mStoping and removing database container[0m
@ECHO [92m--------------------------------------------------------------------------------[0m

docker ps -q --filter "name=empleado-db" | findstr . && docker stop empleado-db && docker rm empleado-db || echo Not Found
@ECHO.

@ECHO [92m--------------------------------------------------------------------------------[0m
@ECHO [92mCleaning Migrations project[0m
@ECHO [92m--------------------------------------------------------------------------------[0m
dotnet clean ..\Migrations.csproj

@ECHO.