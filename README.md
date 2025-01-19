<<<<<<< HEAD
# Product-Catalog
=======
# Product Catalog Web Application

This is a **Product Catalog Web Application** built using **ASP.NET Core MVC** and **Entity Framework Core**. The application allows users to manage a catalog of products, with features like adding, editing, and deleting products. It also includes role-based access control, where only **Admin** users can manage products.

---

## Features

### Product Management
- **Add Products**: Admins can add new products with details like name, description, price, start date, duration, and an image (JPG, JPEG, or PNG, max 1MB).
- **Edit Products**: Admins can update existing product details.
- **Delete Products**: Admins can remove products from the catalog.
- **View Products**: All users can view products, but only admins can see all products regardless of their active status.

### Product Display
- **Active Products**: The application displays products that are currently active (based on their start date and duration).
- **Filter by Category**: Users can filter products by category.

### User Management
- **Role-Based Access**: Only users with the **Admin** role can add, edit, or delete products.
- **Authentication**: Users can register and log in to access the application.

### Logging
- **Error Logging**: Errors are logged for debugging and monitoring.
- **Product Update Logging**: All product updates are logged, including the timestamp and the user who made the change.

### Unit Testing
- The solution includes a **unit test project** using **xUnit** to ensure the application's reliability.

---

## Technologies Used

- **Backend**: ASP.NET Core MVC
- **Database**: MSSQL with Entity Framework Core (EF Core) as the ORM
- **Authentication**: ASP.NET Core Identity for user management and role-based access
- **Frontend**: Razor Views with Bootstrap for responsive design
- **Logging**: Built-in logging with ILogger
- **Testing**: xUnit for unit testing

---

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or any code editor with .NET support)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)

##Project Structure
**Controllers**: Contains the MVC controllers for handling HTTP requests.

**Models**: Contains the entity models (e.g., Product, Category).

**Views**: Contains the Razor views for the user interface.

**Services**: Contains business logic and service classes.

**Repositories**: Contains data access logic using EF Core.

**Migrations**: Contains database migrations for EF Core.

**Tests**: The solution includes a unit test project using xUnit. To run the tests:
>>>>>>> master
