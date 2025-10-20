#!/bin/bash
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
echo "Waiting for SQL Server to start..."
for i in {1..30}; do
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" &>/dev/null && break
  sleep 2
done

# Check if the database exists
DB_EXISTS=$(/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -h -1 -Q "IF DB_ID('${MSSQL_DB:-green_wheel_db}') IS NOT NULL PRINT 1 ELSE PRINT 0")

if [ "$DB_EXISTS" -eq 0 ]; then
  echo "Database not found. Running initialization scripts..."
  for f in /init-db/*.sql; do
    echo "Executing $f"
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -i "$f"
  done
else
  echo "Database already exists. Skipping initialization."
fi

wait
