<p align="center">
  <img src="https://img.icons8.com/?size=512&id=55494&format=png" width="20%" alt="ELIBRARY-logo">
</p>
<p align="center">
    <h1 align="center">ELIBRARY</h1>
</p>

<p align="center">
	<img src="https://img.shields.io/github/license/TEGTO/ELibrary?style=flat&logo=opensourceinitiative&logoColor=white&color=0080ff" alt="license">
	<img src="https://img.shields.io/github/last-commit/TEGTO/ELibrary?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
	<img src="https://img.shields.io/github/languages/top/TEGTO/ELibrary?style=flat&color=0080ff" alt="repo-top-language">
	<img src="https://img.shields.io/github/languages/count/TEGTO/ELibrary?style=flat&color=0080ff" alt="repo-language-count">
</p>
<p align="center">
		<em>Built with the tools and technologies:</em>
</p>
<p align="center">
	<img src="https://img.shields.io/badge/JavaScript-F7DF1E.svg?style=flat&logo=JavaScript&logoColor=black" alt="JavaScript">
	<img src="https://img.shields.io/badge/HTML5-E34F26.svg?style=flat&logo=HTML5&logoColor=white" alt="HTML5">
	<img src="https://img.shields.io/badge/PostCSS-DD3A0A.svg?style=flat&logo=PostCSS&logoColor=white" alt="PostCSS">
	<img src="https://img.shields.io/badge/Autoprefixer-DD3735.svg?style=flat&logo=Autoprefixer&logoColor=white" alt="Autoprefixer">
	<img src="https://img.shields.io/badge/YAML-CB171E.svg?style=flat&logo=YAML&logoColor=white" alt="YAML">
	<br>
	<img src="https://img.shields.io/badge/ESLint-4B32C3.svg?style=flat&logo=ESLint&logoColor=white" alt="ESLint">
	<img src="https://img.shields.io/badge/TypeScript-3178C6.svg?style=flat&logo=TypeScript&logoColor=white" alt="TypeScript">
	<img src="https://img.shields.io/badge/Docker-2496ED.svg?style=flat&logo=Docker&logoColor=white" alt="Docker">
	<img src="https://img.shields.io/badge/GitHub%20Actions-2088FF.svg?style=flat&logo=GitHub-Actions&logoColor=white" alt="GitHub%20Actions">
	<img src="https://img.shields.io/badge/JSON-000000.svg?style=flat&logo=JSON&logoColor=white" alt="JSON">
</p>


# ELibrary 📚

This is a full-stack web application for managing books, authors, and genres, built using modern technologies including .NET for the backend, Angular for the frontend, and PostgreSQL for the database. The application features secure JWT-based authentication and state management on the frontend using the NgRx (Redux) pattern.


## Table of Contents
- [ELibrary](#elibrary)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Installation](#installation)
- [API Overview](#api-overview)
- [Contributing](#contributing)
- [License](#license)
- [Screenshots](#screenshots)
 
##  Features

- **User Authentication**: Secure login and registration with JWT-based authentication.
- **Resource Management**: CRUD functionality for books, authors, and genres.
- **Pagination**: Efficiently navigate large datasets.
- **RESTful APIs**: Organized endpoints for each resource.
- **Frontend State Management**: Uses NgRx (Redux pattern) for scalable and maintainable code.

## Tech Stack

- **Frontend**: Angular, NgRx
- **Backend**: .NET
- **Database**: PostgreSQL
- **Authentication**: JWT

## Getting Started

 **[Try Out the Web App](https://icy-hill-0551b3903.5.azurestaticapps.net/)**

Or to run the ELibrary project locally, follow these steps:

### Prerequisites

- **Node.js** (for Angular CLI)
- **.NET SDK** (for backend)
- **PostgreSQL** (for database)

### Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/TEGTO/ELibrary.git
    ```
2. **Navigate into the project directory**:
    ```bash
    cd ELibrary
    ```
3. **Set up the Backend**:
    - Navigate to the backend directory.
    - Configure database settings in `appsettings.json`.
    - Run the application:
        ```bash
        dotnet run


## API Overview

The backend provides various endpoints for managing library data, including:

- **Authentication**: Register, login, refresh tokens.
- **Books, Authors, Genres**: Full CRUD and pagination capabilities.
 <details closed><summary><h3>Endpoint specifications and example requests.</h3></summary>

### API URLs
- **Auth & User Info API**: https://elibrary-user-api-germanywestcentral-001.azurewebsites.net/
- **Library API**: https://elibrary-library-api-germanywestcentral-001.azurewebsites.net/

## Auth & User Info API Endpoints


### Register

```bash
[POST] /auth/register
```
**Request Body**:
```bash
{
  "userName": "example",
  "password": "123456QWERTY",
  "confirmPassword": "123456QWERTY",
  "userInfo": {
    "name": "name",
    "lastName": "lastName",
    "dateOfBirth": "2020-08-03T09:45:45.4656254Z",
    "address": "address"
  }
}
```



### Login
```bash
[POST] /auth/login
```
**Request Body**:
```bash
{
  "login": "example",
  "password": "123456QWERTY"
}
```

### Refresh Token
```bash
[POST] /auth/refresh
```
**Request Body**:
```bash
{
  "accessToken": "{{accessToken}}",
  "refreshToken": "{{refreshToken}}"
}
```

### Get User Information
```bash
[GET] /userinfo/user
```
**Authorization Required**: `Bearer {{accessToken}}`

## Library API Endpoints

### Author

#### Get an Author by ID
```bash
[GET] /author/{{author_id}}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Get Total Number of Authors
```bash
[GET] /author/amount
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Get Paginated Authors
```bash
[POST] /author/pagination
```
**Request Body**:
```bash
{
    "pageNumber": 1,
    "pageSize": 2
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Create a New Author
```bash
[POST] /author
```
**Request Body**:
```bash
{
    "name": "name",
    "lastName": "lastName",
    "dateOfBirth": "2020-08-03T09:45:45.4656254Z"
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Update an Author
```bash
[PUT] /author
```
**Request Body**:
```bash
{
    "id": {{author_id}},
    "name": "newName",
    "lastName": "lastName",
    "dateOfBirth": "2020-08-03T09:45:45.4656254Z"
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Delete an Author by ID
```bash
[DELETE] /author/{{author_id}}
```
**Authorization Required**: `Bearer {{accessToken}}`

### Genre

#### Get a Genre by ID
```bash
[GET] /genre/{{genre_id}}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Get Total Number of Genres
```bash
[GET] /genre/amount
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Get Paginated Genres
```bash
[POST] /genre/pagination
```
**Request Body**:
```bash
{
    "pageNumber": 1,
    "pageSize": 2
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Create a New Genre
```bash
[POST] /genre
```
**Request Body**:
```bash
{
    "name": "name"
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Update a Genre
```bash
[PUT] /genre
```
**Request Body**:
```bash
{
    "id": {{genre_id}},
    "name": "newName"
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Delete a Genre by ID
```bash
[DELETE] /genre/{{genre_id}}
```
**Authorization Required**: `Bearer {{accessToken}}`

### Book

#### Get a Book by ID
```bash
[GET] /book/{{book_id}}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Get Total Number of Books
```bash
[GET] /book/amount
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Get Paginated Books
```bash
[POST] /book/pagination
```
**Request Body**:
```bash
{
    "pageNumber": 1,
    "pageSize": 2
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Create a New Book
```bash
[POST] /book
```
**Request Body**:
```bash
{
    "title": "title",
    "publicationDate": "2020-08-03T09:45:45.4656254Z",
    "authorId": {{author_id}},
    "genreId": {{genre_id}}
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Update a Book
```bash
[PUT] /book
```
**Request Body**:
```bash
{
    "id": {{book_id}},
    "title": "newTitle",
    "publicationDate": "2020-08-03T09:45:45.4656254Z",
    "authorId": {{author_id}},
    "genreId": {{genre_id}}
}
```
**Authorization Required**: `Bearer {{accessToken}}`

#### Delete a Book by ID
```bash
[DELETE] /book/{{book_id}}
```
**Authorization Required**: `Bearer {{accessToken}}`

---
</details>

## Contributing

We welcome contributions! Please:

1. Fork the repository.
2. Create a new branch (`feature/YourFeature`).
3. Submit a pull request for review.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

This R

## Screenshots 
![image](https://github.com/user-attachments/assets/1ea134dc-aaa6-4c5b-9740-f4a6116ff1cb)

![image](https://github.com/user-attachments/assets/e7d3e117-197f-4f59-a987-f257c7322af3)

![image](https://github.com/user-attachments/assets/5f56919a-2239-4135-9825-1a7473d79194)

![image](https://github.com/user-attachments/assets/1f93cdce-592f-492f-a5a4-3ab3889beed3)

![image](https://github.com/user-attachments/assets/861376d5-fc9e-46c9-9e72-87849ef98c65)

![image](https://github.com/user-attachments/assets/bca80ee9-1982-485d-8d78-76f22cdf5a5f)

![image](https://github.com/user-attachments/assets/74d3d3e4-0cfb-4ec3-90d6-b7e6ac156a8c)



