# Permigo - Leave Management System

A comprehensive case study project for a **Leave Management System**, built with **.NET**, **Docker**, and a **flexible database architecture**. This project demonstrates various software development practices, from fundamental approaches to advanced design patterns.

---

## 🔑 Key Features

- **Leave Request Management**  
  Users can create leave requests with specific start/end dates and types.

- **Approval Workflow**  
  Managers can approve or reject pending leave requests.

- **Request Status Tracking**  
  Users can track the status of their requests (Pending, Approved, Rejected).

- **Employee Management**  
  Admins and Managers can add new employees to the system.

- **Dynamic Leave Types**  
  Admins can create and manage different types of leave with unique properties.

- **Leave Entitlement**  
  Assign specific leave types to employees.

- **Advanced Validation**  
  Prevents conflicting leave requests.  
  The validation for past-dated requests can be dynamically enabled or disabled based on the leave type's configuration.

- **Email Notifications**  
  A framework for sending email notifications for events like new user creation is included.  
  _Note: SMTP configuration is required._

---

## 🧱 Architectural Overview

- **Architecture:** Layered Architecture for separation of concerns and maintainability  
- **Technology Stack:**  
  - ASP.NET Core MVC  
  - Entity Framework Core  
  - Docker  

- **Database Flexibility:**  
  - Default: PostgreSQL  
  - Optional: Microsoft SQL Server  
  - Switching requires minimal config change in `appsettings.json` and `DI`.

- **Design Patterns:**  
  - Result Pattern is used for robust and standardized handling of method outcomes, validations, and errors.

---

## 🚀 Running the Project

### Option 1: Running with Docker (Recommended)

1. Make sure **Docker Desktop** is running.
2. Open the solution in **Visual Studio**.
3. Set **docker-compose** as the startup project.
4. Press `F5` or click **Run**.
5. Visit the application via Docker URL (e.g., `http://localhost:8080`).

### Option 2: Running Locally (Without Docker)

1. Open the solution in **Visual Studio**.
2. Update the database connection string in `appsettings.Development.json`  
   (e.g., `Host=localhost` if using local PostgreSQL).
3. Set **Permigo.MVC** as the startup project.
4. Press `F5` or click **Run**.

> 💡 On first run, the database will be created and seeded automatically using EF Core migrations.

---

## 🧪 How to Test

### Login Credentials

- You can log in with either **username** or **email**.
- **Default Password** for all users: `P@ssword123`

### Test Accounts

- `superadmin`
- `manager1`, `manager2`
- `emp1`, `emp2`, `emp3`, `emp4`

---

## 👥 User Roles & Permissions

| Role        | Capabilities |
|-------------|--------------|
| **Super Admin** | Full system access. Can create Managers and Employees. |
| **Manager**     | Can create Employees. Can approve/reject all leave requests. |
| **Employee**    | Can create and view their own leave requests only. |

> ⚠️ There is **no hierarchical relationship** between managers and employees (no direct reporting line).

---

## 🗄️ Database

- Built with **Entity Framework Core Code-First**.
- Migrations applied automatically on startup.
- Includes full DB script: `permigo_full.sql` for manual setup or reference.

---

## 🧠 A Note on Design Choices

This project is designed as an **educational showcase** of architectural and implementation versatility.

- Different parts of the codebase intentionally use different approaches—from simple methods to advanced patterns (e.g., Result Pattern).
- Principles like **KISS** (Keep It Simple, Stupid) and **YAGNI** (You Ain’t Gonna Need It) were **intentionally set aside** in some areas for demonstration purposes.

---

Happy coding! ✨
