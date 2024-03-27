#!/bin/bash

if [ -z "$1" ]
    then
        echo "specify the name of the migration."
        exit 1
fi

dotnet ef migrations add $1 --project src/Infrastructure/Infrastructure.csproj -s src/Api/Api.csproj -o Persistence/Migrations