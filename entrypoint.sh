#!/bin/sh
set -e

echo "[entrypoint] Starting LavenderFlow API..."
exec dotnet LavenderFlow-API.dll