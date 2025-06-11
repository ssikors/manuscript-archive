# Manuscript Archive

Manuscript Archive is a project meant for sharing and filtering through historical documents and their sources. Here's the setup guide for setting up the ManuscriptApi:

### Prerequisites
- .NET 8 SDK
- SQL Server

### Configuration
**1. Set up the database**
- Use the scripts in the MigrationScripts folder to manually set up the database schema
- Get the connection string to later include it in appsettings.json

**2. Set up appsettings.json**
- By default the `DefaultConnection` connection string and `JwtSettings` are to be declared in the appsettings.json file *in the ManuscriptApi.Presentation folder*.
- The json file should include:
  ```
  "JwtSettings": {
    "Token": "YOUR_SUPER_SECURE_KEY_WITH_SUFFICIENT_LENGTH_FOR_SHA512",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-db-connection-string"
  },
  ```
- The minimum secret key (token) length is 64 characters.

**3. Run the application**
- The app should now be ready to start