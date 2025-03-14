# CRM-ERP-UNID

## 📌 Prerequisites
Before running the application, make sure you meet the following requirements:
- **Docker** is running.
- **.NET SDK 8.0** is installed. You can download it from [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

## 🛠 Run the Database
1. **Ensure Docker is running.**
2. **Navigate to the container folder:**
   ```sh
   cd ./backend/Container
   ```
3. **Run the following command based on your preferred mode:**
    - **Background mode:**
      ```sh
      docker-compose up -d
      ```
    - **Foreground mode:**
      ```sh
      docker-compose up
      ```
4. **If this is your first time running the database, the database script will not run, you need to re-run the container, follow these steps:**
   ```sh
   docker-compose down
   docker-compose up -d
   ```
5. **If the database script is not executing properly, try increasing the wait time:**
    - Edit the file `./backend/Container/scripts/entrypoint.sh`
    - Change the line:
      **sleep 5**

      to a higher value as needed.

---

## 🚀 Run the API
1. **Ensure the database is running.**
2. **Navigate to the backend directory:**
   ```sh
   cd ./backend
   ```
3. **Run the following command based on your preferred mode:**
    - **Foreground mode:**
      ```sh
      dotnet run
      ```
    - **Background mode:**
      ```sh
      dotnet run &
      ```
   > ⚠️ *This command will restore dependencies and build the project if necessary.*
4. **The API will be available at:**
    - **URL:** `http://localhost:5245`
    - **Swagger:** `http://localhost:5245/swagger`

---

## 🧪 Run API Tests
1. **Ensure .NET SDK 8.0 is installed.**
2. **Navigate to the backend directory:**
   ```sh
   cd ./backend
   ```
3. **Run the tests with:**
   ```sh
   dotnet test
   ```

---

You're all set! Now you can use and test the application. 🎉
