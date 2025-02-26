# 2025.TaskManager
# 2025.sql
PassWord PostgresSQL:12345
Scaffold-DbContext "Server=localhost;Port=5432;Database=identity;User Id=postgres;Password=12345" -Provider "Npgsql.EntityFrameworkCore.PostgreSQL" -OutputDir "Core\Entities" -ContextDir ".\Core" -Context "IdentityContext" -Force
