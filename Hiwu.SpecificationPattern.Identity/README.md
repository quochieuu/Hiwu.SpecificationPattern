# EF Core Identity Migration Guideline

## Step 1: Create a Migration

Use the following commands to create a migration for Identity in EF Core:

### Using Package Manager Console

```bash
Add-Migration <MigrationName> -Context ApplicationDbContext
```

### Using .NET CLI
```bash
dotnet ef migrations add <MigrationName> -c ApplicationDbContext
```

### Notes:
ApplicationDbContext is the name of your DbContext. Make sure to use the correct name if you have customized it.
Replace <MigrationName> with a descriptive name that reflects the changes in the migration.

## Step 2: Apply the Migration

Once the migration is created, apply it to the database:

### Using Package Manager Console

```bash
Update-Database -Context ApplicationDbContext
```

### Using .NET CLI
```bash
dotnet ef database update -c ApplicationDbContext
```

## Step 3: Verify the Results
After running the Update-Database command, check your database to ensure that the Identity tables (AspNetUsers, AspNetRoles, etc.) have been successfully created.