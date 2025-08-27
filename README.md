# Aion – Distributed Documentation & Analysis Tool

**Aion** is a distributed system designed to support **documentation, time tracking, and analysis** in software projects.  
It was created as a **learning and demonstration project** to explore and apply modern **software architecture principles** in practice.

---

## 🚀 Architecture Overview

This project intentionally applies established software engineering principles:

- **Clean Architecture** (Hexagonal, Dependency Rule)
- **Interface-based Programming**
- **Domain-Driven Design (DDD-ready)**
- **CQRS** (Command Query Responsibility Segregation)
- **Event Sourcing**
- **Event-driven architecture** (Publish/Subscribe)
- **gRPC & Protobuf** for efficient inter-service communication

The goal is not primarily a finished product, but rather a realistic case study of these concepts in action.

---

## 📦 Features

### Avalonia Client (Desktop)
- ⏱️ **Time Tracking** – capture and export work sessions
- 📝 **Documentation** – link notes and documentation to tasks
- 📊 **Analysis** – overview and analysis of activities

### Blazor Admin Web (Web Interface)
- 📅 **Sprint & Metadata Management** – manage sprints, project data, and metadata
- 🔎 **Observability & Monitoring** – system state and distributed event insights

### Cross-Cutting
- 🌐 **Global Tracing** across all distributed services
- 🧩 Modular design for extensibility

---

## ⚠️ Project Status

- **Prototype** – some features are experimental or incomplete
- Focus on **architecture and learning value**, not production readiness
- Security features are not fully implemented

---

## ▶️ How to Run

1. Clone this repository
2. Start the database in Docker (`docker-compose up`)
3. The web interface can be reached via http://localhost:5000
4. PGAdming can be reached via http://localhost:5050 (Password: aionpass)
5. The Avalonia Client must be compiled and started via the .exe file

⚠️ Note: Some parts are still unstable or incomplete.

---

## 🗄️ EF Core Migrations

To update or add database migrations:

1. Change the `appsettings.json` connection string to `localhost`
2. Start the database in Docker
3. Run:
   ```bash
   dotnet ef migrations add InitialCreate --project Core.Persistence
