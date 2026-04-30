# WikiDK - Swordplay Clan Wiki Backend

A RESTful API backend for a community wiki platform designed for swordplay and archery enthusiasts. WikiDK enables members of swordplay clans to read, post, and share knowledge about rules, sword fighting techniques, archery tutorials, and other martial arts content.

## 🎯 Purpose

WikiDK serves as the backbone of a collaborative wiki platform where:
- **Clan members** can access organized tutorials and rules
- **Contributors** can post and share their expertise
- **Outsiders** can learn from the community's collective knowledge
- **Admins** can manage content and user permissions

## ✨ Features

- **User Authentication & Authorization** - Secure registration and JWT-based login
- **User Management** - Create accounts and retrieve user profiles
- **Content Management** *(In Progress)* - API endpoints for wiki articles and tutorials
- **Role-Based Access Control** - Different permission levels for members and admins
- **RESTful API Design** - Clean, intuitive endpoints for frontend integration

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core
- **Language:** C#
- **Containerization:** Docker
- **Authentication:** JWT (JSON Web Tokens)

## 📋 Current Status

🚧 **Work in Progress** - Core user authentication is complete, content management features are in active development.

### Completed
- ✅ User registration endpoint
- ✅ User login with JWT token generation
- ✅ Protected endpoints with authorization

### In Development
- 🔄 Wiki article CRUD operations
- 🔄 Content categorization (tutorials, rules, etc.)
- 🔄 User roles and permissions system
- 🔄 Comment/discussion features

## 🚀 Getting Started

### Prerequisites
- .NET 8.0+
- Docker (optional)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/K0kai/WikiDK.git
   cd WikiDK
   ```

2. **Build the project**
   ```bash
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

The API will be available at `http://localhost:5000`

### Docker

```bash
docker build -t wikidk .
docker run -p 5000:5000 wikidk
```

## 📚 API Endpoints

### Authentication
- `POST /users/register` - Register a new user
- `POST /users/login` - Login and receive JWT token
- `GET /users/get/me` - Get current user profile (requires authentication)

*Additional endpoints coming soon*

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

## 👤 Author

[K0kai](https://github.com/K0kai)

---

**Contributing:** Feel free to reach out if you're interested in contributing to WikiDK!