## Description

This is a full-stack web application for managing books, authors, and genres, built using modern technologies including .NET for the backend, Angular for the frontend, and PostgreSQL for the database. The application features secure JWT-based authentication and state management on the frontend using the NgRx (Redux) pattern.

**[Try Out the Web App](https://icy-hill-0551b3903.5.azurestaticapps.net/)**

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


## Screenshots 
![image](https://github.com/user-attachments/assets/1ea134dc-aaa6-4c5b-9740-f4a6116ff1cb)

![image](https://github.com/user-attachments/assets/e7d3e117-197f-4f59-a987-f257c7322af3)

![image](https://github.com/user-attachments/assets/5f56919a-2239-4135-9825-1a7473d79194)

![image](https://github.com/user-attachments/assets/1f93cdce-592f-492f-a5a4-3ab3889beed3)

![image](https://github.com/user-attachments/assets/861376d5-fc9e-46c9-9e72-87849ef98c65)

![image](https://github.com/user-attachments/assets/bca80ee9-1982-485d-8d78-76f22cdf5a5f)

![image](https://github.com/user-attachments/assets/74d3d3e4-0cfb-4ec3-90d6-b7e6ac156a8c)
