dotnet restore ../Migrations.csproj
dotnet build ../Migrations.csproj
dotnet fm migrate -p sqlite -c "Data Source=../../mydb.db" -a "../bin/Debug/netcoreapp2.2/Migrations.dll"