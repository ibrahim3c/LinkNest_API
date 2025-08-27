# LinkNest

LinkNest is a modern, modular social networking backend built with **.NET 8** and **C# 12**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles. It implements the **CQRS pattern** with **MediatR**, and provides authentication, authorization, and event-driven extensibility.

---

## ✨ Features

- **User Profiles** – Manage personal data (name, email, date of birth, city)
- **Posts** – Users can create posts with text and images
- **Comments** – Add comments to posts with domain events
- **Likes** – Like/unlike posts and track engagement
- **Follows** – Follow/unfollow users, with events raised on follow actions
- **Authentication & Authorization**
  - **JWT Authentication** with refresh tokens
  - **Permission-based Authorization** for fine-grained access control
- **Notifications** – Email notifications powered by **SendGrid**
- **Logging** – Centralized, structured logging using **Serilog**
- **Domain Events** – Decoupled side-effects for business logic
- **Unit of Work & Repositories** – Clean abstraction for persistence
- **Extensible Architecture** – Add new modules or integrate external systems

---

## 🛠️ Technologies

- **.NET 8 / C# 12**
- **PostgreSQL** – primary database (via EF Core)
- **Entity Framework Core** – ORM with Unit of Work & Repositories
- **MediatR** – CQRS (Command & Query handling)
- **Serilog** – structured logging
- **SendGrid** – email notifications
- **JWT + Refresh Tokens** – authentication
- **Permission-Based Authorization** – granular security
- **Domain-Driven Design (DDD)** + **Clean Architecture**
- **xUnit / Moq / FluentAssertions** – unit testing

---

## 🏗️ Project Structure
LinkNest
├── LinkNest.Api # ASP.NET Core API (controllers, middleware, auth, logging, etc.)
├── LinkNest.Domain # Core domain models, value objects, domain events
├── LinkNest.Application # Application layer (CQRS commands, queries, handlers, services)
├── LinkNest.Infrastructure # EF Core persistence, PostgreSQL, SendGrid, Serilog
└── LinkNest.Application.UnitTests # Unit tests for application logic


---

## 📚 Key Domain Concepts  

- **UserProfile** – Represents a user’s profile with identity and personal details.  
- **Post** – Represents user-generated posts with content & images.  
- **PostComment** – Represents a comment, linked to a post and user.  
- **Interaction** – Represents an interaction (like,love,..) on a post.  
- **Follow** – Represents following relationships; emits domain events.  
- **Domain Events** – Signal important business actions (e.g., profile updated, follow created).  

---

## 🚀 Getting Started  

### Prerequisites  

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- [PostgreSQL](https://www.postgresql.org/download/)  
- (Optional) [SendGrid Account](https://sendgrid.com/) for email notifications  

### Clone the Repository  

```bash
git clone https://github.com/your-username/LinkNest.git
cd LinkNest
