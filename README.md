# Course Registration Assessment

A production-style .NET 8 Web API assessment project for course registration using Clean Architecture, SQL Server, JWT Authentication, FluentValidation, structured logging, transaction management, concurrency protection, and audit logging.

## Business Requirements
- A learner can register for a course.
- A learner cannot register twice for the same course.
- Course capacity cannot be exceeded.
- Registration must be tenant-aware.
- Invalid requests must return meaningful responses.
- API must support concurrent requests safely.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- Serilog
- xUnit
- Clean Architecture

## Project Structure

```text
CourseRegistrationAssessment
│
├── CourseRegistration.Api
├── CourseRegistration.Application
├── CourseRegistration.Domain
├── CourseRegistration.Infrastructure
└── CourseRegistration.Tests

