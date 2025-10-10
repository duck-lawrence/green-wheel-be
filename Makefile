# Load environment variables from .env (nếu có)
ifneq (,$(wildcard .env))
	include .env
	export $(shell sed 's/=.*//' .env)
endif

MSSQL_CONTAINER=greenwheel-mssql

reset-db:
	@echo "Dropping and reinitializing database '$(MSSQL_DB)'..."
	docker exec -i $(MSSQL_CONTAINER) bash -c "/opt/mssql-tools/bin/sqlcmd -S localhost -U $(MSSQL_USER) -P '$(MSSQL_PASSWORD)' -Q \"DROP DATABASE IF EXISTS [$(MSSQL_DB)];\""
	docker restart $(MSSQL_CONTAINER)
	@echo "Database $(MSSQL_DB) has been dropped and will be reinitialized."

# Rebuild all containers with clean volumes
rebuild:
	@echo "Rebuilding all containers and cleaning volumes..."
	docker compose down -v
	docker compose up -d --build
	@echo "All containers rebuilt."
