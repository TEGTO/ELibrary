<p align="center">
  <h1 align="center">
  <img src="https://github.com/user-attachments/assets/b48ab128-2d79-49cd-94f5-deb5eec2889e" width="50%" alt="ELIBRARY-logo">
  </h1>
</p>

<p align="center">
	<img src="https://img.shields.io/github/issues-pr-closed/TEGTO/ELibrary" alt="pull-requests-amount">
	<img src="https://img.shields.io/github/last-commit/TEGTO/ELibrary?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
	<img src="https://img.shields.io/github/languages/top/TEGTO/ELibrary?style=flat&color=0080ff" alt="repo-top-language">
	<img src="https://img.shields.io/github/languages/count/TEGTO/ELibrary?style=flat&color=0080ff" alt="repo-language-count">
	<img src="https://img.shields.io/sonar/coverage/TEGTO_ELibrary?server=https%3A%2F%2Fsonarcloud.io" alt="sonar-cloud-codecoverage">
</p>
<p align="center">
		<em>Built with the tools and technologies:</em>
</p>
<p align="center">
	<img src="https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff" alt=".NET">
	<img src="https://img.shields.io/badge/Angular-%23DD0031.svg?logo=angular&logoColor=white" alt="Angular">
	<img src="https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white" alt="C#">
	<img src="https://img.shields.io/badge/TypeScript-3178C6.svg?style=flat&logo=TypeScript&logoColor=white" alt="TypeScript">
	<img src="https://img.shields.io/badge/JavaScript-F7DF1E.svg?style=flat&logo=JavaScript&logoColor=black" alt="JavaScript">
	<img src="https://img.shields.io/badge/HTML5-E34F26.svg?style=flat&logo=HTML5&logoColor=white" alt="HTML5">
 	<img src="https://img.shields.io/badge/Sass-C69?logo=sass&logoColor=fff" alt="SASS">
	<img src="https://img.shields.io/badge/Postgres-%23316192.svg?logo=postgresql&logoColor=white" alt="PostgreSQL">
	<img src="https://img.shields.io/badge/Tailwind%20CSS-%2338B2AC.svg?logo=tailwind-css&logoColor=white" alt="Tailwind">
	<br>
	<img src="https://custom-icon-badges.demolab.com/badge/Microsoft%20Azure-0089D6?logo=msazure&logoColor=white" alt="Azure">
	<img src="https://img.shields.io/badge/Docker-2496ED.svg?style=flat&logo=Docker&logoColor=white" alt="Docker">
	<img src="https://img.shields.io/badge/Kubernetes-326CE5?logo=kubernetes&logoColor=fff" alt="Kubernetes">
 	<img src="https://img.shields.io/badge/ChatGPT-74aa9c?logo=openai&logoColor=white" alt="ChatGPT">
	<img src="https://img.shields.io/badge/ESLint-4B32C3.svg?style=flat&logo=ESLint&logoColor=white" alt="ESLint">
 	<img src="https://img.shields.io/badge/SonarCloud-F3702A?logo=sonarcloud&logoColor=fff" alt="ESLint">
	<img src="https://img.shields.io/badge/GitHub%20Actions-2088FF.svg?style=flat&logo=GitHub-Actions&logoColor=white" alt="GitHub%20Actions">
	<img src="https://img.shields.io/badge/JSON-000000.svg?style=flat&logo=JSON&logoColor=white" alt="JSON">
	<img src="https://img.shields.io/badge/YAML-CB171E.svg?style=flat&logo=YAML&logoColor=white" alt="YAML">
</p>

## Table of Contents
- [Description](#description)
- [Links](#links)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [Getting Started](#getting-started)
- [Contributors](#contributors)
- [Screenshots](#screenshots)

## Description

This is a full-stack web store application built with modern technologies, featuring an ASP.NET Web API backend, Angular frontend, and PostgreSQL database, all deployed on Azure. The application integrates AI from OpenAI, hosted on Azure, to provide book recommendations. It implements secure authentication with JWT and OAuth 2.0 and manages state on the frontend using the NgRx (Redux) pattern. For resilience, it leverages the Polly library, and employs various design patterns, including the Mediator pattern with MediatR. The project is containerized with Docker Compose and uses GitHub Actions for CI/CD, along with Nunit and Test Containers for testing.

## Links

Here are the main links for accessing the ELibrary project and its documentation:

- **Web Application**: [ELibrary Application](https://gray-dune-04583d603.5.azurestaticapps.net/)  
  Access the live version of the ELibrary application, deployed on Microsoft Azure.

- **Backend Documentation**: [ELibrary Backend Docs](https://tegto.github.io/ELibrary.Docs.Backend/)  
  Explore the backend's API documentation, generated with DocFX.

- **Frontend Documentation**: [ELibrary Frontend Docs](https://tegto.github.io/ELibrary.Docs.Frontend/)  
  View the frontend documentation, generated with Compodoc.

## Tech Stack
- **Frontend**: Angular with NgRx for state management
- **Backend**: ASP.NET Core Web API
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT and OAuth 2.0
- **API Gateway**: Ocelot for routing across microservices
- **Resilience**: Polly for retry policies and fault tolerance
- **Containerization**: Docker and Kubernetes
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

## Getting Started

### Prerequisites
Before you begin, ensure you have the following installed on your machine:
1. **Docker**: For building and running containers. [Install Docker](https://docs.docker.com/get-docker/).
2. **Minikube**: A lightweight Kubernetes implementation for local testing. [Install Minikube](https://minikube.sigs.k8s.io/docs/start/).
3. **kubectl**: Kubernetes command-line tool to manage clusters. [Install kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/).

---

### Kubernetes / Minikube Setup
1. **Clone the repository**:
```
git clone https://github.com/TEGTO/ELibrary.git
```

2. **Navigate into the Kubernetes folder**:
```
cd ELibrary/k8/dev
```

3. **Start Minikube**:
Open a terminal in the folder and start Minikube:
```
minikube start
```

If Minikube is already running, ensure you’re in the correct context:
```
kubectl config use-context minikube
```

4. **Optional: Enable Chat Service**:
    If you want to use the optional chat service:
    - Open the `chatbot-conf.yml` file.
    - Set the `OPENAI_API_KEY` environment variable with your OpenAI API key.

---

### Deployment Steps
Follow these steps in order:

#### 1. Configure ConfigMaps and Secrets
```
kubectl apply -f db-conf.yml
kubectl apply -f backend-conf.yml
kubectl apply -f chatbot-conf.yml # Optional
```

#### 2. Deploy the Database
Deploy the database and wait for it to be fully initialized:
```
kubectl apply -f db.yml
kubectl get pods # Verify that the database pod is running.
```

#### 3. Deploy the Backend
Deploy the backend services:
```
kubectl apply -f backend.yml
```

#### 4. Optional: Deploy the Chat Service
```
kubectl apply -f chatbot.yml
```

#### 5. Deploy the Frontend
Deploy the frontend application:
```
kubectl apply -f frontend.yml
```

#### 6. Access the Frontend
Expose and forward the frontend service using Minikube:
```
minikube service frontend
```

This command will open the frontend in your default web browser.

---

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



