# Database

The main database used in the project is [SQL Server](https://www.microsoft.com/en-us/sql-server/).

## Local Setup

For major productivity, the following instructions regarding the database are using Docker.

### Docker run example

```bash
docker run \
    -e "ACCEPT_EULA=Y" \
    -e "SA_PASSWORD=MyPass@word" \
    -e "MSSQL_PID=Developer" \
    -p 1439:1433 \
    -v "empleado-db-volume:/var/opt/mssql" \
    --name=empleado-db \
    -d
    mcr.microsoft.com/mssql/server:latest
```

| Parameter                             | Description                                                                                                                                                                                                                    |
| ------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| -e "ACCEPT_EULA=Y"                    | Set the ACCEPT_EULA variable to any value to confirm your acceptance of the End-User Licensing Agreement. Required setting for the SQL Server image.                                                                           |
| -e "MSSQL_PID=Developer"              | Set the Product ID (PID) or Edition that the container will run with.                                                                                                                                                          |
| -e "SA_PASSWORD=MyPass@word"          | Specify your own strong password that is at least 8 characters and meets the SQL Server password requirements. Required setting for the SQL Server image.                                                                      |
| -p 1439:1433                          | Map a TCP port on the host environment (first value) with a TCP port in the container (second value). In this example, SQL Server is listening on TCP 1433 in the container and this is exposed to the port 1439, on the host. |
| -v empleado-db-volume:/var/opt/mssql  | It creates and mounts a volume with the data directory of the database. This will persist the data even if the container is removed.                                                                                           |
| --name empleado-db                    | Specify a custom name for the container rather than a randomly generated one. If you run more than one container, you cannot reuse this same name.                                                                             |
| -d (--detach)                         | Run the container in the background.                                                                                                                                                                                           |
| mcr.microsoft.com/mssql/server:latest | SQL Server latest image.                                                                                                                                                                                                       |

## Common Errors

### Problem

```
Error A connection was successfully established with the server, but then an error occurred during the pre-login handshake.
```

### Solution

The problem occurs due to the database was not ready when the action was performed. If you wait a couple of seconds and try again it should work as expected.

## References

- [Dockerhub - Microsoft SQL Server](https://hub.docker.com/_/microsoft-mssql-server)

- [Quickstart: Run SQL Server container images with Docker](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash)

- [Configure SQL Server container images on Docker](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-configure-docker?view=sql-server-ver15)
