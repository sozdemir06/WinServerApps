# WinApps - Modular Monolith .NET Application

A modern, modular monolith application built with .NET 9, following Clean Architecture principles and Domain-Driven Design (DDD) patterns.

## 🏗️ Architecture

This project follows a **Modular Monolith** architecture pattern, where each module is self-contained with its own domain logic, data access, and business rules while sharing a common infrastructure.

### Project Structure

```
WinApps/
├── Bootstrapper/
│   └── Api/                    # Main API project
├── Modules/                    # Business modules
│   ├── Accounting/            # Accounting module
│   ├── Catalog/              # Catalog module
│   ├── Customers/            # Customer management
│   ├── Notifications/        # Notification services
│   ├── Orders/              # Order management
│   ├── Shipping/            # Shipping services
│   └── Users/               # User management
└── Shared/                   # Shared libraries
    ├── Shared/              # Common utilities
    └── Shared.Messages/     # Integration events
```

## 🚀 Features

- **Modular Architecture**: Each module is independent with its own domain
- **CQRS Pattern**: Command Query Responsibility Segregation
- **Event-Driven Architecture**: Using MassTransit and RabbitMQ
- **Outbox Pattern**: Reliable message processing
- **Multi-tenancy**: Support for multiple tenants
- **Caching**: Redis-based caching with cache invalidation
- **Localization**: Multi-language support
- **Validation**: FluentValidation integration
- **Logging**: Structured logging with Serilog

## 🛠️ Technology Stack

- **.NET 9**
- **ASP.NET Core**
- **Entity Framework Core**
- **PostgreSQL**
- **Redis**
- **RabbitMQ**
- **MassTransit**
- **FastEndpoints**
- **MediatR**
- **FluentValidation**
- **Serilog**

## 📋 Prerequisites

- .NET 9 SDK
- Docker Desktop
- PostgreSQL
- Redis
- RabbitMQ

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd WinApps
```

### 2. Setup Docker Services

```bash
docker-compose up -d
```

This will start:
- PostgreSQL database
- Redis cache
- RabbitMQ message broker

### 3. Run the Application

```bash
cd Bootstrapper/Api
dotnet run
```

The API will be available at `https://localhost:7001`

## 🏗️ Module Structure

Each module follows a consistent structure:

```
ModuleName/
├── Data/                      # Data access layer
│   ├── Configurations/       # Entity configurations
│   ├── Migrations/          # EF Core migrations
│   ├── Processors/          # Background services
│   └── Seed/               # Initial data
├── EntityName/              # Domain entities
│   ├── Constants/          # Module constants
│   ├── DomainEvents/       # Domain events
│   ├── DomainExtensions/   # Entity extensions
│   ├── Dtos/              # Data transfer objects
│   ├── Exceptions/         # Domain exceptions
│   ├── Features/           # CQRS commands/queries
│   ├── Models/             # Domain models
│   └── QueryParams/        # Query parameters
└── ModuleName.cs           # Module registration
```

## 🔧 Configuration

### Environment Variables

Create `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=WinApps;Username=postgres;Password=password"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

## 📚 API Documentation

The API uses FastEndpoints with automatic OpenAPI documentation. Visit `/swagger` for interactive API documentation.

## 🧪 Testing

```bash
dotnet test
```

## 📦 Deployment

### Docker

```bash
docker build -t winapps .
docker run -p 7001:7001 winapps
```

### Production

1. Set up production environment variables
2. Configure database connections
3. Set up Redis and RabbitMQ clusters
4. Deploy using your preferred method (Azure, AWS, etc.)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support, please open an issue in the GitHub repository or contact the development team.

---

**Note**: This is a modular monolith application designed for scalability and maintainability. Each module can potentially be extracted into a microservice in the future if needed. 