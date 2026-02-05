# Task Management API

## 1. Tech Stack
*   **Framework:** ASP.NET Core 8.0
*   **Database:** SQL Server
*   **ORM:** Entity Framework Core
*   **Authentication:** JWT Bearer Authentication
*   **Logging:** Serilog
*   **Documentation:** Swagger / Swashbuckle
*   **Mapping:** AutoMapper

## 2. Live URL
*   **Base URL:** http://test-api.somee.com

## 3.. Production Access
*   **Live Swagger:** [http://test-api.somee.com/swagger/index.html](http://test-api.somee.com/swagger/index.html)
*   **Administrator Credentials:**
    *   **Email:** `Admin123@gmail.com` 
    *   **Password:** `Admin@123`

## 4. Features
*   **User Authentication:** Secure registration and login using JWT.
*   **Task Management:** Create, read, update, and delete tasks.
*   **Search & Filtering:** Filter tasks by status, priority, and assignee.
*   **Role-Based Access:** Admin-only privileges for specific actions (e.g., deleting tasks).
*   **Global Exception Handling:** Centralized error management for consistent API responses.
*   **Structured Logging:** Detailed logs utilizing Serilog for monitoring and debugging.
*   **Validation:** Input validation using Data Annotations / FluentValidation standards.

## 5. Local Setup
1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd TaskManagement
    ```

2.  **Configure Database:**
    *   Update the `ConnectionStrings:DefaultConnection` in `appsettings.json` to point to your local or hosted SQL Server instance.
    *   Example: `Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;`

3.  **Run Migrations:**
    ```bash
    dotnet ef database update --project TaskManagement.API
    ```

4.  **Run the Application:**
    ```bash
    dotnet run --project TaskManagement.API
    ```

5.  **Access Swagger:**
    *   Navigate to `http://localhost:8080/swagger`.
    *   **Note:** To change the port, modify the `Urls` section in `appsettings.json`.

## 6. Authentication Flow
1.  **Register:** Create a new account using `/api/v1/auth/register`. A JWT token is returned.
2.  **Login:** Users credentials are exchanged for a JWT token via `/api/v1/auth/login`.
3.  **Access Resources:** Include the JWT token in the `Authorization` header of subsequent requests.
    *   Format: `Bearer <your_token>`

## 7. API Endpoints

### Authentication
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/v1/auth/register` | Register a new user | No |
| `POST` | `/api/v1/auth/create-admin` | Create a new admin user (Local Testing Only) | No |
| `POST` | `/api/v1/auth/login` | Authenticate user and get token | No |

### Tasks
| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/v1/task` | Create a new task | Yes |
| `GET` | `/api/v1/task/{id}` | Get a task by ID | Yes |
| `GET` | `/api/v1/task` | Get all tasks (supports filtering) | Yes |
| `PUT` | `/api/v1/task/{id}` | Update an existing task | Yes |
| `PATCH` | `/api/v1/task/{id}/status` | Update task status | Yes |
| `DELETE` | `/api/v1/task/{id}` | Delete a task (Admin/Owner) | Yes |
| `GET` | `/api/v2/task` | Get tasks (supports Pagination) | Yes |

## 8. Testing
### Using Postman
1.  **Import the Collection:**
    *   Import the file `Task Management API.Production.postman_collection.json` (located in the root directory) into Postman.
2.  **Environment Setup:**
    *   Ensure the `BaseUrl` variable in Postman is set to `http://test-api.somee.com`.
3.  **Authentication:**
    *   Send a `POST` request to `/api/v1/auth/register` (or Login) to get a token.
    *   Copy the `token` from the response.
    *   For protected endpoints, go to the **Authorization** tab, select **Bearer Token**, and paste the token.
4.  **Execute Requests:** Run any request from the collection.

### Using Swagger
1.  Open the Swagger UI.
2.  Click the **Authorize** button at the top right.
3.  Enter `Bearer <your_token>` and click **Authorize**.
4.  Execute endpoints directly from the UI.

## 9. Architecture Decisions
*   **Layered Architecture:** The solution is structured into separate layers (Controllers, Services, Repositories, Data) to ensure separation of concerns and maintainability.
*   **Repository Pattern:** Used to abstract data access logic, making the code more testable and decoupling the business logic from the EF Core implementation.
*   **DTOs (Data Transfer Objects):** Used to transfer data between layers and the API, ensuring that internal domain models are not exposed directly to the client.
*   **Dependency Injection:** Heavy use of .NET Core's built-in DI container to manage dependencies and lifecycle.
