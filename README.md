# Restaurant Reservation System

A full-stack restaurant reservation application built with **C# and ASP.NET Core Web API**.

The project was created as a practical learning project focused on backend development, business logic, database access, testing, and REST API design. It is being developed incrementally from a simple CRUD application into a complete reservation workflow.

## Features

- Create, read, update and delete restaurant reservations
- Manage restaurant tables
- Search for available tables based on:
  - reservation date and time
  - number of guests
  - table capacity
- Detect overlapping reservations
- Validate restaurant opening hours
- Validate reservation data using DTOs
- Find the nearest available reservation time within the next **7 days**
- Suggest another reservation time when the requested time is unavailable
- Create reservations through a simple web interface
- Swagger/OpenAPI documentation
- Unit and integration tests

## Reservation Flow

The application supports the following reservation process:

1. The user selects a date, time and number of guests.
2. The frontend sends a request to the REST API.
3. The backend searches for tables with sufficient capacity.
4. Existing reservations are checked for time conflicts.
5. If a table is available, the user can select it and create a reservation.
6. If no table is available, the backend searches for the nearest available reservation time within the next 7 days.
7. The suggested reservation can then be selected and created.

## Technologies

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- LINQ
- Dependency Injection
- DTOs
- REST API
- Swagger / OpenAPI

### Testing

- xUnit
- Unit tests
- Integration tests
- SQLite in-memory database for integration testing

### Frontend

- HTML
- CSS
- JavaScript
- Fetch API

The frontend is currently intentionally simple and is used mainly to demonstrate the complete communication flow between the user interface and the REST API.

## Architecture

The application is divided into several layers:

```text
Frontend
   |
   | HTTP / JSON
   v
Controllers
   |
   v
Services
   |
   v
Entity Framework Core
   |
   v
SQLite
```

Main project directories:

```text
Controllers/          API endpoints
Services/             business logic
Dtos/                 request and response DTOs
Models/               domain models
Data/                 Entity Framework DbContext
Migrations/           EF Core database migrations
Results/              service result types
RestaurantApi.Tests/  unit and integration tests
wwwroot/              frontend HTML, CSS and JavaScript
```

## Testing

The reservation availability logic is covered by unit tests.

The reservation search flow is also tested using integration tests with an SQLite in-memory database.

Example scenarios include:

- returning the requested time when it is available
- handling multiple conflicting reservations
- handling reservations supplied in a different order
- searching for availability on the next day
- skipping closed days
- returning no result when no reservation is available within 7 days

Run all tests with:

```bash
dotnet test
```

## Running the Project

### Requirements

- .NET 10 SDK

Clone the repository:

```bash
git clone https://github.com/Willoath/RestaurantFromScratch.git
cd RestaurantFromScratch
```

Restore dependencies:

```bash
dotnet restore
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

The application URL will be displayed in the console.

The frontend is available at the application root:

```text
http://localhost:<port>/
```

Swagger is available in Development mode at:

```text
http://localhost:<port>/swagger
```

## Current Development Status

The project is actively developed.

Current priorities include:

- improving frontend validation and API error messages
- refactoring database operations to async/await
- improving the frontend UI
- adding additional automated tests
- adding API rate limiting
- improving production-ready configuration and database setup

## What I Learned

While developing this project I worked with:

- REST API design
- ASP.NET Core controllers and Dependency Injection
- Entity Framework Core and database migrations
- relational data and foreign keys
- DTO-based API contracts
- business rule validation
- date and time conflict detection
- LINQ queries
- algorithm design for reservation availability
- unit and integration testing
- communication between JavaScript and an ASP.NET Core API using Fetch API

## Author

**Mateusz Strzałkowski**

Computer Science student at Lublin University of Technology, developing toward backend development with C# and .NET.
