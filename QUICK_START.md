# Quick Start Guide - Task Manager Project

This guide provides a quick overview to get your Task Manager project up and running.

## 1. Prerequisites

Make sure you have these installed:
- **.NET 8 SDK**
- **Node.js (LTS)**
- **Angular CLI** (`npm install -g @angular/cli`)

## 2. Backend Setup (ASP.NET Core API)

1.  **Navigate**: `cd TaskManagerAPI`
2.  **Restore Packages**: `dotnet restore`
3.  **Apply Migrations**: `dotnet ef database update`
    *If `dotnet ef` is not found, install it: `dotnet tool install --global dotnet-ef`*
4.  **Run API**: `dotnet run`
    *API will run on `https://localhost:7000` (check console for exact URL).* 
    *Access Swagger UI at `https://localhost:7000/swagger`.*

## 3. Frontend Setup (Angular)

1.  **Navigate**: `cd TaskManagerFrontend`
2.  **Install Dependencies**: `npm install`
3.  **Run App**: `ng serve`
    *App will run on `http://localhost:4200` (opens automatically).* 

## 4. Initial Login

-   **Admin Username**: `admin`
-   **Admin Password**: `admin123`

## 5. What You'll See

-   **Login/Register Page**: Start here.
-   **Dashboard**: Overview of tasks and users (for Admin).
-   **Tasks**: List, create, edit, delete tasks.
-   **Users**: Manage users (Admin only).
-   **Profile**: Update your email and password.

## Need Help?

Check the complete **README.md** for:
- Detailed setup instructions
- API documentation
- Comprehensive troubleshooting guide
- Project structure overview


