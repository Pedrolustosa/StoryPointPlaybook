# StoryPointPlaybook

[![Version](https://img.shields.io/badge/version-1.0.0-blue.svg?style=for-the-badge)]()
[![NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)]()
[![License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=for-the-badge)](LICENSE.txt)
[![Status](https://img.shields.io/badge/status-active-success.svg?style=for-the-badge)]()
[![SignalR](https://img.shields.io/badge/SignalR-Real--time-orange?style=for-the-badge)]()
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Pedro%20Lustosa-0077B5?style=for-the-badge&logo=linkedin)](https://www.linkedin.com/in/pedrolustosadev/)


## Overview
StoryPointPlaybook is a modern, real-time Planning Poker application designed for agile teams to efficiently estimate story points. It facilitates remote estimation sessions with features like real-time voting, chat, and automatic vote reveal. Teams can collaborate in real-time regardless of their physical location, making it ideal for distributed agile teams.

## Architecture
The project follows Clean Architecture principles with distinct layers:
- **API Layer**: REST endpoints and SignalR hubs for real-time communication
- **Application Layer**: Business logic and CQRS implementation using MediatR
- **Domain Layer**: Entities and business rules
- **Infrastructure Layer**: Data persistence and external services integration
- **IoC Layer**: Dependency injection configuration

## Features
- **Room Management**: Create and manage Planning Poker rooms
- **Story Management**: Add, edit, and organize user stories
- **Real-Time Voting**: Synchronous voting with multiple scales (Fibonacci, T-shirt sizes, Powers of Two, Custom)
- **Live Chat**: Real-time communication between team members during sessions
- **Auto-Reveal**: Optional automatic reveal of votes after everyone has voted
- **Session Tracking**: Keep track of voting sessions and results
- **Rate Limiting**: Protection against excessive API usage
- **Connected User Tracking**: Monitor who is online and participating
- **Time-boxed Voting**: Optional time limits for voting sessions

## Technologies
- **.NET 8.0**
- **Entity Framework Core**
- **SQL Server**
- **MediatR (CQRS pattern)** for command and query segregation
- **SignalR** for real-time communications
- **FluentValidation** for input validation
- **Serilog** for structured logging with console and file sinks
- **Health checks** for system status monitoring
- **JWT Authentication** for securing endpoints
- **Swagger/OpenAPI** for API documentation
- **Docker** support for containerization

## Setup and Installation

### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Node.js and npm (for frontend, if applicable)

### Backend Setup
1. Clone this repository
2. Update the connection string in `appsettings.json` in the API project
3. Run database migrations:
```
dotnet ef database update --project StoryPointPlaybook.Infrastructure --startup-project StoryPointPlaybook.API
```
4. Run the API:
```
dotnet run --project StoryPointPlaybook.API
```

### Docker Setup
The application can be containerized using Docker:
```
docker build -t storypointplaybook -f StoryPointPlaybook.API/Dockerfile .
docker run -p 8080:80 storypointplaybook
```

## Usage
1. Create a new Planning Poker room
2. Add user stories to be estimated
3. Invite team members to join the room using the generated room code
4. Start a voting session on a specific story
5. Reveal votes after all members have voted
6. Discuss and repeat if necessary

## API Endpoints
The API exposes several endpoints for interacting with the system:
- `/api/rooms`: Room creation, joining, and management
- `/api/stories`: Story creation, retrieval, and estimation workflows
- `/api/votes`: Individual and group voting operations
- `/api/chat`: Chat message operations for team communication
- `/health`: Basic health check endpoint for monitoring

All API endpoints are documented using Swagger/OpenAPI available at `/swagger` when running in development mode.

Real-time features are available through SignalR hubs:
- `/gamehub`: For game-related real-time updates (voting, revealing results)
- `/chathub`: For real-time chat functionality

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request.

## License
This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.