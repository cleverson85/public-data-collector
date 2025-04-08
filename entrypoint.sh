#!/bin/bash
set -e

echo "⏳ Waiting SQL Server..."
until dotnet ef database update -s PublicDataCollector.WebApi.csproj; do
  >&2 echo "❌ Data Base unavailable. Trying again in 5 seconds..."
  sleep 5
done

echo "✅ Data Base available, migrations applied with success!"
exec dotnet PublicDataCollector.WebApi.dll
