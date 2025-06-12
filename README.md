# Task Manager Project

This is a Task Manager application built with ASP.NET Core Web API for the backend and Angular for the frontend. It allows administrators to manage users and tasks, and regular users to view and update their assigned tasks.

## Technologies Used

**Backend:**

- ASP.NET Core 8.0 Web API (C#)
- Entity Framework Core (for database interaction)
- SQLite (as the database)
- JWT Authentication (for secure API access)
- BCrypt.Net-Next (for password hashing)

**Frontend:**

- Angular 17+
- Angular Material (for UI components)
- Bootstrap 5 (for responsive layout)

## Features

### Admin Features

- **User Management**: Create, view, update, and delete user accounts.
- **Task Management**: Create, view, update, and delete tasks.
- **Task Assignment**: Assign tasks to specific users.
- **Dashboard**: Overview of all tasks and users.

### User Features

- **Authentication**: Register and Login with JWT-based authentication.
- **Profile Management**: View and update personal profile (email, password).
- **Dashboard**: Overview of assigned tasks.
- **Task Progress**: View and update the status of assigned tasks.

## Getting Started

Follow these steps to set up and run the project on your local machine.

### Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js (LTS version)](https://nodejs.org/en/download/)
- [Angular CLI](https://angular.io/cli) (install globally: `npm install -g @angular/cli`)

### Backend Setup (ASP.NET Core API)

1.  **Navigate to the Backend Directory**:

    ```bash
    cd TaskManagerAPI
    ```

2.  **Restore NuGet Packages**:

    ```bash
    dotnet restore
    ```

3.  **Apply Database Migrations**:
    This project uses Entity Framework Core with SQLite. You need to apply migrations to create the database and seed the initial admin user.

    ```bash
    dotnet ef database update
    ```

    _If you encounter an error about `dotnet ef` not being found, install the EF Core tools globally:_

    ```bash
    dotnet tool install --global dotnet-ef
    ```

    _Then try `dotnet ef database update` again._

4.  **Run the Backend API**:
    ```bash
    dotnet run
    ```
    The API will typically run on `https://localhost:7000` (or a similar port). You will see the URL in the console output.

### Frontend Setup (Angular)

1.  **Navigate to the Frontend Directory** (in a new terminal window/tab):

    ```bash
    cd TaskManagerFrontend
    ```

2.  **Install Node.js Dependencies**:

    ```bash
    npm install
    ```

3.  **Configure API URL**:
    Open `src/environments/environment.ts` and `src/environments/environment.prod.ts`.
    Ensure `apiUrl` matches your backend API URL (e.g., `https://localhost:7000/api`).

4.  **Run the Angular Application**:
    ```bash
    ng serve
    ```
    The Angular app will typically run on `http://localhost:4200`. It will automatically open in your browser.

## Initial Credentials

An initial admin user is seeded in the database:

- **Username**: `admin`
- **Password**: `admin123`

You can log in with these credentials or register a new user.

## API Endpoints (Swagger UI)

Once the backend is running, you can access the Swagger UI for API documentation and testing:

- Navigate to `https://localhost:7000/swagger` (replace 7000 with your actual port).

## Project Structure

```
TaskManager/
├── TaskManagerAPI/             # ASP.NET Core Backend
│   ├── Controllers/
│   ├── Data/                   # DbContext, Migrations
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Models/                 # Entity Models (User, TaskItem)
│   ├── Services/               # JWT Service
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs              # Entry point and configuration
│   └── TaskManagerAPI.csproj
│
└── TaskManagerFrontend/        # Angular Frontend
    ├── src/
    │   ├── app/
    │   │   ├── components/     # Shared components (if any)
    │   │   ├── guards/         # AuthGuard, AdminGuard
    │   │   ├── models/         # TypeScript interfaces for data
    │   │   ├── pages/          # Individual page components (login, dashboard, tasks, users, profile)
    |   |   |   |---share/
                         Navbar
                         Footer
    │   │   │   ├── dashboard/
    │   │   │   ├── login/
    │   │   │   ├── profile/
    │   │   │   ├── register/
    │   │   │   ├── task-form/
    │   │   │   ├── task-list/
    │   │   │   ├── user-form/
    │   │   │   └── user-list/
    │   │   ├── services/       # Auth, Task, User services, AuthInterceptor
    │   │   ├── app-routing.module.ts
    │   │   └── app.module.ts
    │   ├── environments/       # Environment configurations
    │   ├── favicon.ico
    │   ├── index.html
    │   ├── main.ts
    │   └── styles.css
    ├── angular.json
    ├── package.json
    ├── tsconfig.json
    └── ... (other Angular config files)
```
