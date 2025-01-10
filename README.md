# Clean Architecture Template for .NET

A **Clean Architecture** template for the .NET framework, designed to provide a solid foundation for starting new projects. It comes with a pre-implemented simple REST API.

---

## How to Use

1. Clone this repository.
2. Create a `.env` file in the `src` folder with the following content:

    ```dotenv
    DATABASE_HOST=your_database_host
    DATABASE_PORT=your_database_port
    DATABASE_NAME=your_database_name
    DATABASE_USERNAME=your_database_username
    DATABASE_PASSWORD=your_database_password
    ```

3. Clean and build the solution before running the application:

    ```bash
    cd src/WebApi
    dotnet clean
    dotnet build
    dotnet run
    ```

    The application will automatically scaffold entities and generate the necessary migrations when detected changed in database.

    To scaffold and migration when needed even database not change, please delete `track` folder in `src/infrastructure/Build` and `dotnet run` again.

---

## Running Tests

To execute the tests, navigate to the test directory and run the following command:

```bash
cd test
dotnet test
