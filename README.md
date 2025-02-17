# Company

This project consists of two parts:
- **Server-side (Backend)**: Built using **.NET Core (ASP.NET Core)** for managing the API, authentication, and business logic.
- **Client-side (Frontend)**: Built using **React** for the user interface, interacting with the backend API.
       
## Steps to Set Up and Run the Project

### Prerequisites

Before starting the project, ensure that the following steps are followed for both **server-side** and **client-side** setups.

### 1. Server-Side Setup

#### 1.1 Clone the Repository

Clone the repository to your local machine:

git clone https://github.com/GergoHalasz/Company.git

#### 1.2 Restore the NuGet Packages

Before running the project, restore the required NuGet packages:
dotnet restore

#### 1.3 Apply Migrations

Once the project is restored, apply the database migrations to update the database schema:
dotnet ef database update
It adds also automatically the SQL records provided in the task.

#### 1.4 Run the API Server

Now you can start the .NET Core API server. host: https://localhost:7185

### 2. Authentication and Testing APIs
#### 2.1 Get JWT Token

Open the Swagger UI for your API by opening simply the https://localhost:7185
Go to the /api/auth/login endpoint.
In the "Try it out" section, provide the necessary credentials username:'testUser', password:'testPassword'
Click the Execute button to receive the JWT token.
Copy the generated JWT token for use in Postman.


2.2 Test APIs Using Postman
Open Postman and create a new request.
In the request settings, set the Authorization type to Bearer Token.
Paste the JWT token you received from Swagger into the token field.
Now you can start testing the other API endpoints with authentication.

More details about the APIs in Swagger. 

Unit tests can be checked at Company.Tests

### 3. Client-Side Setup
#### 3.1 Install Dependencies
Navigate to the Client directory:

cd Company/clientapp

Install the necessary npm packages:

npm install
#### 3.2 Start the React Development Server
Once the packages are installed, run the React app:
npm run dev

Now you can see the records fetched in browser as it automatically gets jwt token(doesn't require login). host: http://localhost:51776


