# Project Name - REST API in .NET

## Overview
This project is a fully functional REST API built using .NET, following best practices for scalability, maintainability, and security. It includes authentication, authorization, CRUD operations, and advanced API features like pagination, filtering, and caching.

## Features
- RESTful API design principles
- Authentication & Authorization (JWT-based)
- CRUD operations with validation
- Pagination, filtering, and sorting
- API versioning & documentation (Swagger)
- Caching & performance optimization
- Role-based access control

## Tech Stack
- **Backend:** .NET 7/8, ASP.NET Core
- **Database:** SQL Server / PostgreSQL / MongoDB
- **Authentication:** JWT
- **API Documentation:** Swagger
- **Caching:** Redis (optional)

## Getting Started

### Prerequisites
- .NET SDK (7 or 8)
- A database (SQL Server, PostgreSQL, or SQLite for local development)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/mzamann1/WatchThisShit.git
   cd WatchThisShit
   ```
2. Install dependencies:
   ```bash
   dotnet restore
   ```
3. Configure database connection in `appsettings.json`:
   ```json
   "Database": {
      "ConnectionString": "Your_Database_Connection_String"
   }
   ```
4. Apply migrations:
   ```bash
   dotnet ef database update
   ```
5. Run the API:
   ```bash
   dotnet run
   ```

## API Endpoints

| Method | Endpoint                  | Description               | Auth Required |
|--------|---------------------------|---------------------------|---------------|
| GET    | `/api/movies`             | Get all items             | No            |
| GET    | `/api/movies/{idOrSlug}`  | Get item by ID            | No            |
| POST   | `/api/movies`             | Create a new item         | Yes           |
| PUT    | `/api/movies/{id}`        | Update an item            | Yes           |
| DELETE | `/api/movies/{id}`        | Delete an item            | Yes           |

## Authentication
This API uses JWT-based authentication. To access protected endpoints:
1. Register or log in to obtain a token.
2. Include the token in the `Authorization` header:
   ```http
   Authorization: Bearer YOUR_ACCESS_TOKEN
   ```

## Documentation
Swagger UI is available at:
```
http://localhost:port/swagger
```

## Contributing
Feel free to fork this repository, create a new branch, and submit a pull request!

