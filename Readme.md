# Requirements
- [.NET SDK](https://www.microsoft.com/net/learn/get-started/)
- Bash terminal using Ubuntu for Windows or another distro available in the Microsoft Store (Windows Devs Only)

## Ubuntu command line configuration (Windows only)
After you setup your Ubuntu command line on Windows (user, password, etc.), you need to setup the `dotnet` enviroment. On the command line, type the following commands:

``` bash
~$ wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb
~$ sudo dpkg -i packages-microsoft-prod.deb
```
# Getting the .NET SDK

## Windows
Download the .Net SDK [in this link](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.302-windows-x64-installer) or paste this in your  browser https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.302-windows-x64-installer

## Mac
Download the .Net SDK [in this link](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.302-macos-x64-installer) or paste this in your  browser https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.302-macos-x64-installer 

## Linux
Run the following commands to get the .NET SDK on the linux command line:
``` bash
~$ sudo apt-get install apt-transport-https
~$ sudo apt-get update
~$ sudo apt-get install dotnet-sdk-2.1
```

# Run migrations for the first time

1. Run the `init.sh` script in the root path of the project. This will run the command lines tools necessary to run migrations (Mac users may need to restart the terminal Window to detect the new changes)

2. Open the solution and compile the `Migrations` project

3. On the terminal/command line, go to "Migrations/Scripts/" folder and run the `up.sh` script. This should have two effects:
    - Create mydb.db database file (which is a SQLite database) in the root folder
    - All Migrations should apply into the database file

# Run the app for the first time

1. Make a copy of the `appsettings.json.template` file located inside the Web directory

2. Rename it `appsettings.json`

3. Fill each corresponding key value for every specific service (you can leave the values empty, but the file must be present since it is required to get the project running, without the values filled, things like authentication wont work tho.)

4. Open the solution

5. Install this nuget packages
    - Microsoft.AspNetCore.HttpsPolicy
    - Microsoft.AspNetCore.Session

6. Restore the rest of nuget package

7. Run


# Running the project using Docker

On project root run this command: `docker build -t emplea_do_net_core/dev . && docker run -it --publish 5001:5000 emplea_do_net_core/dev` and the app will be running at `localhost:5001`.

### Enabling authentication for Docker container

1. Make a copy of the `appsettings.json.template` file located inside the Web directory

2. Rename it `appsettings.json`

3. Fill each corresponding key value for every specific service
