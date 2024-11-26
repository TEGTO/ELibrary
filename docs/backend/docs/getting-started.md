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