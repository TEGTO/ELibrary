## Table of Contents
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Contributors](#contributors)
- [Screenshots](#screenshots)

## Tech Stack
- **Frontend**: Angular with NgRx for state management
- **Backend**: ASP.NET Core Web API
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT and OAuth 2.0
- **API Gateway**: Ocelot for routing across microservices
- **Resilience**: Polly for retry policies and fault tolerance
- **Containerization**: Docker and Docker Compose
- **CI/CD**: GitHub Actions for automated workflows
- **AI Integration**: OpenAI’s GPT for recommendations
 
##  Features

- **ASP.NET Web API Backend**: Built using ASP.NET Core, the backend provides a robust, RESTful API that supports a variety of CRUD operations and complex resource management. The application follows a “Code First” approach using Entity Framework Core with a PostgreSQL database, ensuring seamless database migrations and schema management.

- **Angular Frontend with NgRx**: The frontend is built with Angular, offering a dynamic and responsive user experience. It leverages NgRx (Redux pattern) for state management, making the codebase scalable and maintainable, even as the application grows.

- **User Authentication**: Implements secure user authentication and authorization through JWT-based tokens for session management, and supports OAuth 2.0 for seamless third-party integrations.

- **API Gateway with Ocelot**: An Ocelot API Gateway is used to route and manage requests between microservices, improving scalability and simplifying service management. This setup helps in optimizing requests and load balancing.

- **Polly for Resilience**: The Polly library is integrated to handle transient faults with retry policies, circuit breakers, and timeouts, improving application resilience and reliability under different conditions.

- **OpenAI Integration for AI-powered Book Recommendations**: Leveraging OpenAI’s GPT model, hosted on Azure, the application features an AI-based online consultant. The AI can provide personalized book recommendations by accessing real-time data from the database, enhancing user engagement.

- **Containerization with Docker Compose**: All services are containerized using Docker and managed via Docker Compose, ensuring consistent environments across development, testing, and production stages.

- **Continuous Integration and Continuous Deployment (CI/CD)**: The project uses GitHub Actions for automated CI/CD pipelines, enabling seamless deployments to Azure. Testing is supported with NUnit and Test Containers to ensure reliability and code quality.

- **Design Patterns**: Incorporates various design patterns for better code organization and maintainability. This includes the Mediator pattern (via MediatR), which facilitates decoupled communication between services.

- **Testing and Quality Assurance**: Implements NUnit and Test Containers for unit and integration tests, ensuring that all components are thoroughly tested in isolated environments.

## Contributors
<a href="https://github.com/TEGTO/ELibrary/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=TEGTO/ELibrary"/>
</a>

## Screenshots 
![image](https://github.com/user-attachments/assets/473221cd-25b5-4681-9139-60f66b48dd98)

![image](https://github.com/user-attachments/assets/dae00cc3-c261-498b-9893-0098a2d778ed)

![image](https://github.com/user-attachments/assets/44360e7d-c594-42a8-aeac-ba33c8e7f7ff)

![image](https://github.com/user-attachments/assets/c4855a4c-3b6c-4269-be33-4d45eace932e)

![image](https://github.com/user-attachments/assets/c9a8d14f-3cfd-4956-a35a-5d3839d30a15)

![image](https://github.com/user-attachments/assets/b0c12186-bb05-44aa-8f35-cd3b64969ed1)

![image](https://github.com/user-attachments/assets/76890fd6-6b08-4001-b5f7-520e6b6b6b91)



