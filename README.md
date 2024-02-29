# FileSystemAPI

##  API service for  browser-based file system

- Create, rename and delete folders
- Create, rename and delete files
- Download selected file
- Search for files across all files or within specific folder (including subfolders)
- List directory (get list of all items in specific fodler)

## Instructions

### 1. Requirements

- Visual Studio (.NET 8.0 SDK and Runtime needed)
    - Used version: Microsoft Visual Studio Community 2022 (64-bit) - Version 17.9.2
- Microsoft SQL Server (With Windows login enabled and necessary permissions)
    - Used version: Microsoft SQL Server 2022 SQLEXPRESS 16.0.1110.1

### 2. Setup startup project 

In Visual studio set startup project: `FileSystemAPI.Api`

### 3. Initialize database

Create and initialize database on local SQLEXPRESS database server.  
(If database on remote server is used, connection string in `appsettings.Development.json` file need to be changed accordingly.)

Database can be created on different ways:  
1. **EF core Migration (recommended):**  
In Visual Studio Package Manager Console run following command:  
```
dotnet ef database update --project src\FileSystemApi.Persistence --startup-project src\FileSystemApi.Api
```
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(To run previous command, `ef tools` needs to be installed, if not already: `dotnet tool install --global dotnet-ef`)

2. **Restore database from backup:**  
Download backup (.bak) file from my Google drive using followin link:  
https://drive.google.com/file/d/1IpMaVtcTRrWoWZTydq_kPbg0sO_mOrI8/view?usp=sharing  
Restore database (using SQL Server Management Studio)

3. **Run SQL script:**  
Run SQL script for creating database. Script can be found in this repository at following location:  
`\scripts\db_create.sql`


### 4. Customize configuration (optional)
Customize configuration entries if needed (`appsettings.Development.json` file):  
- `ConnectionStrings:DefaultConnection`: DB connection string
- `MaxUploadSizeInBytes`: Max uploaded file size in bytes
- `StoragePath`: **The directory where the uploaded files will be written (current user must have write permission to that folder)**
- `Serilog:WriteTo:Args:path`: **The directory where the log files will be written (if logs are not needed, whole section can be removed)**

### 5. Run application and explore endpoints
Start application in Visual Studio and in browser navigate to following link to explore and test available API endpoints:
https://localhost:7261/swagger/index.html


