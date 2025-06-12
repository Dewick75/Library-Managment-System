
# 📚 iLibrary Management System

A comprehensive Library Management System built with C# and Windows Forms. This desktop application provides a robust solution for managing a library's members, books, and lending operations with a focus on security and usability.

![image](https://github.com/user-attachments/assets/b0f1bf10-1edb-4962-a68f-24423fb9d3e2)




---

## ✨ Features

*   **Secure Authentication:** Register and log in as an administrator with salted password hashing (PBKDF2).
*   **Member Management:** Easily add, view, search, and remove library members. Includes data validation and duplicate entry prevention.
*   **Book Cataloging:** Manage the library's book collection, including classification, publisher details, and the number of available copies.
*   **Lending & Returns:** A complete workflow to issue books to members and process their returns, with built-in rule enforcement (e.g., borrowing limits).
*   **Powerful Search:** Quickly find members and books using various search criteria.
*   **User-Friendly Interface:** An intuitive Windows Forms UI with clear navigation and helpful feedback dialogs.

---

## 🛠️ Technology Stack

*   **Language:** C# 7.3
*   **Framework:** .NET Framework 4.7.2
*   **UI:** Windows Forms (WinForms)
*   **Database:** Microsoft SQL Server
*   **Data Access:** ADO.NET
*   **Security:**
    *   PBKDF2 for password hashing (`Rfc2898DeriveBytes`)
    *   Parameterized SQL queries to prevent SQL Injection

---

## 🗄️ Database Design

The database schema is designed to efficiently manage library data. The core tables include:

| Table               | Description                                                                 |
| ------------------- | --------------------------------------------------------------------------- |
| `Admins`            | Stores secure admin credentials (username, hashed password, salt).          |
| `Members`           | Contains detailed information for each library member.                      |
| `Books`             | Stores metadata for each book title (e.g., classification, publisher).      |
| `ClassificationTable` | Defines book classification codes and descriptions.                         |
| `BookCopies`        | Tracks individual physical copies of each book and their availability.      |
| `Loans`             | Records all lending transactions, linking members, book copies, and dates.  |

![image](https://github.com/user-attachments/assets/3e8cc28e-57df-4573-a1b7-8df2e985813c)

---

## 📂 Project Structure

The project is organized into distinct forms, each responsible for a specific feature, promoting modularity and maintainability.

| File / Component        | Purpose                                                              |
| ----------------------- | -------------------------------------------------------------------- |
| `Login.cs`              | Handles the admin login form and authentication logic.               |
| `AdminReg.cs`           | Manages the registration of new administrator accounts.              |
| `Members.cs`            | Provides the form for registering new library members.               |
| `RegisterdMembers.cs`   | Displays, searches, and allows deletion of registered members.       |
| `BookAdding.cs`         | Form for adding new book titles and their physical copies.           |
| `BooksIssue.cs`         | The core interface for issuing and returning books.                  |
| `Loan_Return.cs` *(inferred)* | The main dashboard or navigation form after a successful login.      |

---

## 🔑 Key Components

### 🔐 Authentication

*   **`AdminReg.cs`**: Allows new administrators to register. Passwords are never stored in plain text; they are salted and hashed using the strong **PBKDF2** algorithm.
*   **`Login.cs`**: Verifies admin credentials by comparing the entered password against the stored hash and salt. Grants access to the main application dashboard upon success.

### 👥 Member Management

*   **`Members.cs`**:
    *   Registers new library members with validation for required fields, email, phone number, and national ID formats.
    *   Checks for duplicate `MemberId` to ensure data integrity.
*   **`RegisterdMembers.cs`**:
    *   Displays all registered members in a `DataGridView`.
    *   Features a search functionality to filter members by Name or ID.
    *   Allows deletion of members with a confirmation prompt to prevent accidental data loss.

### 📖 Book Management & Cataloging

*   **`BookAdding.cs`**:
    *   Adds new book titles with classification, publisher, year, and type (Reference/Borrowable).
    *   Manages individual book copies, enforcing a maximum limit (e.g., 10 copies per book title).
    *   Dynamically loads book classifications to assist the admin.

### 🔄 Book Lending & Returns

*   **`BooksIssue.cs`**:
    *   **Issuing Books**: Enforces business rules, such as a maximum of 5 borrowed books per member and ensuring a book copy is available and borrowable.
    *   **Returning Books**: Updates the `Loans` record and marks the book copy as available.
    *   Displays a member's current loans to provide a clear overview.

---

## 🛡️ Validation & Security

*   **Input Validation**: All forms perform client-side validation for required fields and use regular expressions for specific formats (email, phone).
*   **SQL Injection Prevention**: The application exclusively uses **parameterized queries** for all database interactions, eliminating the risk of SQL injection attacks.
*   **Password Security**: Admin passwords are protected with a strong, salted cryptographic hash, adhering to modern security standards.
*   **Error Handling**: Database operations are wrapped in `try-catch-finally` blocks to handle exceptions gracefully and ensure database connections are always closed, preventing resource leaks.

---

## 🚀 Getting Started

Follow these steps to set up and run the project on your local machine.

### Prerequisites

*   **Visual Studio** 2017 or later
*   **.NET Framework 4.7.2** Developer Pack
*   **Microsoft SQL Server** (Express, Developer, or Standard edition)

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/SarasaviLibrary.git
cd SarasaviLibrary
```
*(Replace `your-username` with the actual repository owner's username)*

### 2. Configure the Database

1.  Ensure your **SQL Server instance is running**.
2.  Create a new database (e.g., `SarasaviLibraryDB`).
3.  Run the SQL script provided in the `/database` folder (or create the tables manually based on the [Database Design](#-database-design) section) to set up the required schema.
4.  Open the project in Visual Studio and locate the database connection string in the code. Update it to point to your SQL Server instance and database. It will likely be in a central helper class or repeated in each form.

### 3. Build and Run

1.  Open the `SarasaviLibrary.sln` file in Visual Studio.
2.  Build the solution (**Build > Build Solution** or `Ctrl+Shift+B`).
3.  Press **F5** or click the "Start" button to run the application.
4.  The first step will be to register a new admin account.

![lb1](https://github.com/user-attachments/assets/9aaccd81-a98d-4c15-8a4b-ad5ba26bf28e)
![lb2](https://github.com/user-attachments/assets/4df89f25-0b29-40cb-afb6-15245988ffa9)
![lb3](https://github.com/user-attachments/assets/eeb25acd-47a2-4c25-a93a-fa9eb5947405)
![lb5](https://github.com/user-attachments/assets/eb1b7b33-8c1d-4918-a5a7-d54fa33bd5d7)
![lb6](https://github.com/user-attachments/assets/ea218e6f-1f8e-4c26-b658-c5c2be2f2997)

---

## 🙌 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for more details.
