# Todo Application

This is a **Todo Application** built using **ASP.NET Core**, providing basic task management features such as adding, updating, viewing, and changing the status of tasks. The app uses a **Clean Architecture** approach with a **Repository Pattern**, ensuring separation of concerns and maintainability.

## Features

- **User Authentication**: Secure login using **ASP.NET Identity**.
- **Task Management**: Users can add, update, view, and delete tasks.
- **Image Upload**: Users can attach images to tasks (e.g., product images).
- **Task Status Management**: Tasks can be marked as completed or pending.
- **Authorization**: Only authorized users can perform certain actions on tasks (e.g., only the task creator can edit or delete the task).

## Prerequisites

Before you begin, ensure you have met the following requirements:

- **.NET 8 SDK** (or later)
- **SQL Server** or another supported database system
- **Visual Studio 2022** or later (or any IDE that supports .NET)

## Setup and Installation

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/yourusername/todo-app.git
   cd todo-app
   ```

2. **Install Dependencies**:

   If using Visual Studio:
   - Open the solution file (`.sln`) in Visual Studio.
   - Restore NuGet packages: `Tools -> NuGet Package Manager -> Restore Packages`.

   If using the command line, run:

   ```bash
   dotnet restore
   ```

3. **Database Setup**:

   - Set up a database connection in `appsettings.json` (default is SQL Server).
   - Create a migration and apply it:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run the Application**:

   To start the application locally, run:

   ```bash
   dotnet run
   ```

   The application will be accessible at `http://localhost:5000`.

## Application Structure

### 1. **Controllers**

The controllers are responsible for handling HTTP requests and responses. The `TodoController` handles all operations related to tasks (CRUD operations, changing task status, and file uploads).

### 2. **Models**

The models represent the data structures used in the application. 
- `TodoTask`: Represents a task in the Todo list.
- `TodoTaskResponse`: A DTO (Data Transfer Object) used to format the task data for display.
- `TodoTaskAddRequest`: A DTO used when adding a new task.
- `TodoTaskUpdateRequest`: A DTO used when updating an existing task.

### 3. **Services**

The `IUnitOfWork` interface and its implementation define the pattern used for database interaction. This provides a clean separation between the controllers and data layer.

### 4. **Views**

The views are Razor Pages responsible for rendering HTML. The views for task listing, adding, editing, and details are implemented using Razor.

### 5. **Image Management**

The application supports uploading and updating images associated with tasks. Uploaded images are stored in the `wwwroot/images/todos` directory.

## Features Walkthrough

### 1. **User Authentication**

Users are authenticated via **ASP.NET Identity**. When a user logs in, their `UserId` is stored and used to associate tasks with a specific user.

### 2. **Add Todo**

- Users can add a new task with a title, description, and an optional image.
- The task will be associated with the logged-in user.
- If an image is uploaded, it is stored in the `wwwroot/images/todos` folder.

### 3. **View Todo Details**

- Users can view the details of a task, including its image, status, and description.
- Only the owner of the task can edit or delete it.

### 4. **Edit Todo**

- Users can edit a task's title, description, and image.
- If an image is uploaded, the old image will be deleted and replaced with the new one.

### 5. **Change Task Status**

- Users can mark a task as completed or pending.
- The application ensures that tasks marked as completed cannot be edited.

## API Endpoints

| HTTP Method | Endpoint                | Description                       |
|-------------|-------------------------|-----------------------------------|
| `GET`       | `/Todo/Index`            | Displays all incomplete tasks     |
| `GET`       | `/Todo/AddTodo`          | Displays the form to add a new task|
| `POST`      | `/Todo/AddTodo`          | Submits a new task                |
| `GET`       | `/Todo/Details/{id}`     | Displays task details             |
| `GET`       | `/Todo/Edit/{id}`        | Displays the form to edit a task  |
| `POST`      | `/Todo/Edit/{id}`        | Updates an existing task          |
| `GET`       | `/Todo/ChangeStatus/{id}`| Displays the task status page     |
| `POST`      | `/Todo/ChangeStatus`     | Updates the task's completion status |

## Folder Structure

```
TodoApp
│
├── Controllers
│   └── TodoController.cs
│
├── Models
│   ├── TodoTask.cs
│   ├── TodoTaskResponse.cs
│   ├── TodoTaskAddRequest.cs
│   ├── TodoTaskUpdateRequest.cs
│
├── Repositories
│   └── IUnitOfWork.cs
│   └── TodoRepository.cs
│
├── Views
│   ├── Todo
│   │   ├── Index.cshtml
│   │   ├── AddTodo.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   ├── ChangeStatus.cshtml
│
└── wwwroot
    └── images
        └── todos (for storing task images)
```

## License

This project is licensed under the MIT License.
