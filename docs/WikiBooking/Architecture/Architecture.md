# Architecture

The architecture of this project is based on Vertical Slice Architecture, which divides the code by **slice**, each of which has responsibilities for a specific functionality of the system. This allows for greater modularity, scalability and ease of maintenance.

**Project Layers:**
 - BookPro.Api.
 - BookPro.Application.
 - BookPro.Common.
 - BookPro.Domain.
 - BookPro.Infrastructure.

Each layer is a separate project that has references to other projects based on your needs. These references are detailed below:

![ProjectReference](https://github.com/Haliam/BookPro/blob/34145297ec536a9433b0890c5b5fc9984b7ef143/docs/WikiBooking/Architecture/img/ReferenceProyect.jpg)


**Explanation of references:**
- API --> Application: The API layer depends on Application, as this is where the business logic is handled and responses to HTTP requests are generated.
- Application --> Infrastructure: The Application layer depends on infrastructure, as this layer is responsible for data persistence.
- Infrastructure --> Domain: Infrastructure depends on the Domain, because the latter contains the models and interfaces that define the main entities of the system.

# Layers explained
Below we will explain what responsibility each layer has.
|API|Application|Common|Infrastructure|Domain
|--|--|--|--|--|
|Contains the handlers that are responsible for receiving HTTP requests, validating them, and passing them to the Application layer services for processing.| Manage the business logic of the system. Services in this layer interact with the infrastructure layer to access the database.|It contains common classes and utilities that can be used by all other layers, such as global settings, validators, custom exceptions, etc. | Implement the services required for the system, such as data access through repositories, database migrations, and dependency injection configuration. |Defines domain models, which represent the central entities of the system (for example, User, RefreshToken) and their business rules. It also contains the interfaces for the repositories.

# Authentication
Authentication is done using JSON Web Tokens (JWT), which allows stateless session management. Each request is authenticated with a signed token, ensuring the security of communications.

# Technologies Used
The technologies used in this project are the following:

- **Lenguaje:** C#
- **Framework:** .NET Framework 6.0
- **Authentication:** Jwt
- **Base de datos:** Sql Server
- **NuGet packages:**
  - **AutoMapper:** To map objects between models of different layers.
  - **BCrypt.Net-Next:** For secure password encryption.
  - **Microsoft.AspNetCore.Localization:** For the internationalization of the application.
  - **Microsft.AspNetCore.Authenticacion.JwtBearer:** For JWT-based authentication.
  - **Serilog:** For logging and monitoring logs.
  - **Microsft.EntityFrameworkCore :** For interaction with the database using Entity Framework Core.
  - **Microsft.Extensions. :** For application configuration management.
