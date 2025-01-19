# Product Catalog Web Application

This is a **Product Catalog Web Application** built using **ASP.NET Core MVC** and **Entity Framework Core**. 
The application allows users to manage a catalog of products, with features like adding, editing,
and deleting products. It also includes role-based access control,
where only **Admin** users can manage products and only Admin can see proucts regardless of time,

There a global list of products, and whenever the application loads, it will 
return the products that are supposed to show up at the current time. Products 
have the following properties: a name, a creation date, created by userId, a start 
date, a duration, and a price.

Each product should have an image.
Product image should be (JPG, JPEG or PNG), and cannot be more than 1MB size.


Managing products (Add/Edit/Delete) should be allowed only for “Admin” users, 
so you should use Identity to manage the user privileges.

---

## 1. Architecture
 The application follows the clean Architecture with the following layers:

-** Presentation Layer**:
Contains the MVC controllers and Razor views.
Handles user interactions and displays data.

-**Business Logic Layer**:
Contains service classes that implement the application's business logic.
Handles product management, user authentication, and logging.

-**Data Access Layer**:
Contains repository classes that interact with the database using EF Core.
Manages CRUD operations for products and categories.

-**Database Layer:
MSSQL database with tables for products, categories, users, and logs.


## 2. Features

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

## 3. Technologies Used

- **Backend**: ASP.NET Core MVC
- **Database**: MSSQL with Entity Framework Core (EF Core) as the ORM
- **Authentication**: ASP.NET Core Identity for user management and role-based access
- **Frontend**: Razor Views with Bootstrap for responsive design
- **Logging**: Built-in logging with ILogger
- **Testing**: xUnit for unit testing

---

## Usage
-**Admin Features**
Add Product: Navigate to the "Create Product" page and fill in the product details.

Edit Product: Click the "Edit" button on a product to update its details.

Delete Product: Click the "Delete" button on a product to remove it from the catalog.

View All Products: Admins can view all products, regardless of their active status.

-**User Features**
View Active Products: Users can view products that are currently active.

Filter by Category: Use the category dropdown to filter products.

## Logging and Error Handling
Error Logging: Errors are logged using the built-in ILogger interface. Logs can be viewed in the console or configured to write to a file or external service.
Product Update Logging: All product updates are logged in the database, including the timestamp and the user who made the change.

## 4.Unit Testing

### 5. Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or any code editor with .NET support)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)



