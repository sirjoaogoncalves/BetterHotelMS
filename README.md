# BetterHotel Management Studio

## Overview

This project is a Hotel Management System designed to facilitate the management of various aspects of a hotel, including employees, clients, rooms, and stay records. It provides a web-based interface for administrators and employees to perform tasks such as adding new employees, updating client information, managing room details, and handling stay records.

## Features

### User Authentication

- Users are required to log in with their credentials to access the system.
- Different levels of access are provided based on user roles (administrator or employee).

### Employee Management

- **FuncionarioController**: Allows administrators to manage employees, including creating, updating, and deleting employee records.
- Employees can view their own details but have limited access to other employee records.

### Client Management

- **ClienteController**: Provides CRUD operations for managing client information.
- Both administrators and employees have access to client management functionalities.

### Room Management

- **QuartoController**: Enables administrators to manage room details such as room type and cost per night.
- CRUD operations are available for room management.

### Stay Record Management

- **RegistoController**: Handles stay records, allowing users to create, update, and delete records of guest stays.
- Provides functionalities to view stay details and manage stay records.

### Data Filtering

- **FiltroController**: Offers various filtering options for stay records, including filtering by date, client name, employee name, and room type.
- Calculation of total earnings based on filtered records is also available.

### Export to PDF and Excel

- Users can export records of clients, reservations, and filtered stay records to PDF and Excel formats.
- This feature enhances data accessibility and enables users to generate reports for analysis or sharing.

## Class Structure

- **LoginController**: Manages user authentication, providing login, logout, and session management functionalities.
- **ClienteController**: Handles client-related operations.
- **FuncionarioController**: Manages employee-related operations.
- **QuartoController**: Deals with room-related operations.
- **RegistoController**: Manages stay record operations.
- **FiltroController**: Provides data filtering functionalities.

## Database Context

- **DbGestaoHotelJoaoContext**: Represents the database context for the application, including entity configurations and database interactions.

## Dependencies

- **Entity Framework Core**: Used for database management and interaction.
- **ASP.NET Core MVC**: Framework for building web applications.
- **C#**: Primary programming language used in development.

## Setup Instructions

1. Clone the repository to your local machine.
2. Ensure you have the necessary dependencies installed, including .NET Core SDK and Entity Framework Core.
3. Set up the database connection string in the `appsettings.json` file.
4. Run the database migrations to create the database schema.
5. Build and run the application.


## License

This project is licensed under the MIT License. 



