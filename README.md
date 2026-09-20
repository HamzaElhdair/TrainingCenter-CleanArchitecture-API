# Training Center Management System API

> Enterprise-grade backend built with .NET Core, implementing Modern Clean Architecture (API, Core, Infrastructure) and **Database First** approach.

---

## 🏗️ Architecture Overview

The project is structured following **Clean Architecture** principles to ensure high maintainability, separation of concerns, and testability:

1. **`TrainingCenter API` (Presentation Layer)**
   - Handles HTTP requests via ASP.NET Core Web Controllers.
   - Contains JWT Authentication, Custom Authorization Handlers (`SameUserOrAdmin`), and Middlewares.
   - Configured with interactive **Swagger UI** supporting Bearer Token authentication.

2. **`TrainingCenter Core` (Domain & Application Layer)**
   - **Entities:** Core domain models (`Student`, `Course`, `Instructor`, `User`, etc.).
   - **DTOs:** Request and response models organized by domain.
   - **Interfaces:** Abstractions for both Repositories and Services.
   - **Services:** Core business logic implementations.

3. **`TrainingCenter Infrastructure` (Data Access Layer)**
   - **Data:** Entity Framework Core `AppDbContext` and Fluent API Configurations.
   - **Repositories:** Concrete data access implementations communicating with the database.

---

## 🗄️ Database Approach: Database First

This project adopts the **Database First** approach:
- The database schema is designed and maintained directly in the SQL Server database.
- Entity Framework Core was used to scaffold/reverse-engineer the existing database models into the infrastructure layer, ensuring tight alignment with the pre-existing database schema.

---

## 🚀 Key Features

- **Clean Architecture & SOLID Principles** implementation.
- **Secure JWT Authentication & Authorization** (Role & Policy-based).
- **Repository Pattern** combined with Service Layer architecture.
- **Centralized Error Handling** and structured request validation.
- **API Documentation** via Swagger/OpenAPI.

---

## 🛠️ Tech Stack

- **Framework:** .NET Core / ASP.NET Core Web API
- **ORM:** Entity Framework Core (Database First)
- **Database:** Microsoft SQL Server
- **Security:** JWT (JSON Web Tokens), BCrypt / ASP.NET Identity password hashing patterns.

---

## 📂 Project Structure

```text
TrainingCenterAPI/
│
├── TrainingCenter API/           # Presentation Layer (Controllers, Auth, Program.cs)
├── TrainingCenter Core/          # Domain & Application (Entities, DTOs, Services, Interfaces)
├── TrainingCenter Infrastructure/# Data Access (DbContext, Configurations, Repositories)
│
├── .gitignore
├── .gitattributes
└── README.md
