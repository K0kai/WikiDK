# WikiDK

A C# ASP.NET Core backend for a collaborative wiki platform serving the swordplay clan Death Knights (DK). WikiDK provides a RESTful API for managing wiki content, user authentication, and role-based access control.

## Overview

WikiDK enables clan members to read, post, and share knowledge about swordplay and archery. The platform supports different user roles with varying levels of access:

- **Clan members** can access tutorials and community guidelines
- **Contributors** can create and share content
- **Guests** can view public knowledge
- **Administrators** can manage content and user permissions

## Features

- User authentication with JWT tokens
- User management and profile endpoints
- Role-based access control
- RESTful API design for frontend integration
- Content management (in progress)

## Technology Stack

- **Framework:** ASP.NET Core
- **Language:** C#
- **Containerization:** Docker
- **Authentication:** JWT (JSON Web Tokens)

## Project Status

The project is in active development. User authentication is complete, with content management features currently in progress.

**Completed:**
- User registration and login endpoints
- JWT token generation and validation
- Protected endpoints with authorization

**In Development:**
- Wiki article CRUD operations
- Content categorization and tagging
- User roles and permissions system
- Comment and discussion features

## Getting Started

### Requirements

- .NET 8.0 or later
- Docker (optional)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/K0kai/WikiDK.git
   cd WikiDK
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

The API will be accessible at `http://localhost:5000`

### Docker

```bash
docker build -t wikidk .
docker run -p 5000:5000 wikidk
```

## API Endpoints

### Authentication

- `POST /users/register` - Register a new user
- `POST /users/login` - Authenticate and receive JWT token
- `GET /users/get/me` - Retrieve current user profile (requires authentication)

Additional endpoints are planned for upcoming releases.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Author

[K0kai](https://github.com/K0kai)

## Contributing

Contributions are welcome. If you'd like to contribute to WikiDK, feel free to reach out or submit a pull request.
