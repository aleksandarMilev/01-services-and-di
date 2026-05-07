# Services and DI (ASP.NET Web API)

This solution covers **Dependency Injection (DI)**, **Inversion of Control (IoC)**, and **service-oriented design** in ASP.NET Core.

It has two parts:

- `demo/` — a broken books feature and a fixed authors feature demonstrating IoC principles, plus a `LifetimeDemo` to visualize how different service lifetimes are instantiated. We'll also do some live coding to explain each topic and cover more advanced examples like Keyed Services.
- `exercise/` — an intentionally incomplete student lab. Your task is to apply IoC principles to a Movies feature.

---

## Prerequisites

- .NET 10
- Docker

---

## Running the Projects

For the demo:

```powershell
cd demo
docker compose up --build -d
```

For the exercise:

```powershell
cd exercise
docker compose up --build -d
```

This starts:

- A SQL Server container on port `1433`
- An ASP.NET Web API container on port `8080`

Both run in the Development environment, so OpenAPI and the Scalar UI are enabled. Open `http://localhost:8080/scalar/v1`.

> You can also run locally with `dotnet run`, but you'll need a SQL Server instance matching the connection string in `appsettings*.json`. Always prefer Docker.

---

## Part 1: `demo`

### What's registered in `Program.cs`

```csharp
builder
    .Services
    .AddDbContext<LibraryDbContext>(...)
    .AddScoped<IAuthorService, AuthorService>()
    .AddTransient<ITransientService, TransientService>()
    .AddScoped<IScopedService, ScopedService>()
    .AddSingleton<ISingletonService, SingletonService>()
    .AddControllers();
```

On startup, it applies migrations and seeds some demo data.

### Endpoints

| Method   | Route                    | Description                      |
| -------- | ------------------------ | -------------------------------- |
| `GET`    | `/api/authors`           | Get all authors with their books |
| `GET`    | `/api/authors/{id}`      | Get a single author              |
| `GET`    | `/api/books`             | Get all books                    |
| `GET`    | `/api/books/{id}`        | Get a single book                |
| `POST`   | `/api/books`             | Create a book                    |
| `POST`   | `/api/books/{id}/borrow` | Borrow a book                    |
| `POST`   | `/api/books/{id}/return` | Return a book                    |
| `DELETE` | `/api/books/{id}`        | Delete a book                    |
| `GET`    | `/api/lifetime-demo`     | Visualize service lifetimes      |

### Seeded data

Authors: George Orwell, Jane Austen, Fyodor Dostoevsky

Books: 1984, Animal Farm, Pride and Prejudice, Crime and Punishment

---

## Part 2: `exercise`

Note — the app **will crash** on startup as-is. That's expected.

### What's missing

1. `CinemaDbContext` is not registered in `Program.cs`
   > The app tries to resolve it during startup, so it will crash immediately.
2. `IMovieService` is not registered in `Program.cs`
3. `MoviesController` has no `IMovieService` injected and all actions throw `NotImplementedException`
4. `MovieService` has no implementation

### Your tasks

1. Define the method signatures in `IMovieService`:
   - get all movies
   - get movie by id
   - create movie
   - delete movie
2. Implement `MovieService` — inject `CinemaDbContext` via primary constructor and implement each method
3. Register `CinemaDbContext` in `Program.cs`
4. Register `IMovieService` → `MovieService` with the appropriate lifetime
5. Inject `IMovieService` into `MoviesController` via primary constructor
6. Implement each controller action using the service
7. Return `ActionResult<T>` instead of plain `IActionResult`
8. Propagate `CancellationToken` through async EF Core calls

### You are done when:

- The app starts without errors
- All endpoints in `MoviesController` are implemented
- `MoviesController` does not use `CinemaDbContext` directly
- `MoviesController` does not return models representing database tables (the `MovieDataModel` class)
- `MovieService` does not use web models (the `CreateMovieRequestModel` class) and is not aware of anything HTTP-related — feel free to create a custom service model for the `POST` endpoint or just pass the needed data as primitive values. Your choice.

### Seeded data

Movies: The Shawshank Redemption, The Godfather, The Dark Knight, Schindler's List

---

## Grading

| Points | Requirement                                                                       |
| ------ | --------------------------------------------------------------------------------- |
| 2 pts  | App starts without errors                                                         |
| 2 pts  | All endpoints in `MoviesController` are implemented                               |
| 2 pts  | `MoviesController` does not use `CinemaDbContext` directly                        |
| 2 pts  | `MoviesController` does not return models representing database tables            |
| 2 pts  | `MovieService` does not use web models and is not aware of anything HTTP-specific |
| **10** | **Total**                                                                         |

| Points   | Grade |
| -------- | ----- |
| 0–2 pts  | 2     |
| 3–4 pts  | 3     |
| 5–6 pts  | 4     |
| 7–8 pts  | 5     |
| 9–10 pts | 6     |

### Running the grading tests

The exercise comes with an automated grader. You'll need Docker for it.

From the `exercise/` folder:

```bash
docker compose run --rm --build grader
```

Each criterion shows as `[PASS]` or `[FAIL]` with a message explaining what's wrong.
