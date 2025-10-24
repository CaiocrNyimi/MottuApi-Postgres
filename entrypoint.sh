#!/bin/bash
set -e

echo "🔧 Aplicando migrations..."
cd /src/MottuApi
dotnet ef database update

echo "🚀 Iniciando aplicação..."
cd /app
exec dotnet MottuApi.dll