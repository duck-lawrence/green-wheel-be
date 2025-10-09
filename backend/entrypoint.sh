#!/bin/bash
/opt/mssql/bin/sqlservr &

# wait for SQL Server to start
sleep 25

# Check if database already exists
DB_EXISTS=$(/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -h -1 -Q "IF DB_ID('green_wheel_db') IS NOT NULL PRINT 1 ELSE PRINT 0")

if [ "$DB_EXISTS" -eq 0 ]; then
  echo "Database not found. Running initialization scripts..."
  for f in /init-db/*.sql
  do
    echo "Executing $f"
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -i "$f"
  done
else
  echo "Database already exists. Skipping initialization."
fi

wait
