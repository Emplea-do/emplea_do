# Start setting .net enviroment
ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.2-alpine3.8

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.401

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='89ba545c35154d7b5d40480148aff245d624ce287be4c52711ee987167feb9688b8beac92e607885b9e66a8228981df05b41e9846b6810449e8c05e170389619' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \ 
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

#Finished setting .net enviroment


# Copy current directory contents to /app inside container
WORKDIR "/app"
COPY . .

RUN dotnet build Migrations \
    && dotnet tool install -g FluentMigrator.DotNet.Cli \
    && export PATH="$PATH:/root/.dotnet/tools" \
    && cd Migrations/Scripts && ./up.sh

# Install FluentMigrator tool
#RUN dotnet tool install -g FluentMigrator.DotNet.Cli
#RUN cd Migrations && ls
# Set dotnet tools on env path and build and run Migrations
#RUN  export PATH="$PATH:/root/.dotnet/tools" \
#&& cd Migrations && ls \
#&& cd Scripts && ./up.sh

#Install and restore packages
RUN cd Web && dotnet add package Microsoft.AspNetCore.HttpsPolicy \
   && dotnet add package Microsoft.AspNetCore.Session \
   && dotnet restore

CMD ["dotnet", "run", "--project", "Web"]

