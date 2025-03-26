# BookHaven MVC Project

# Project Mind Map
![Image](https://github.com/user-attachments/assets/cb2c1273-e969-4738-bd58-fa6021b14882)

![Image](https://github.com/user-attachments/assets/f9c87150-be34-4f81-8944-85e83fa748cb)
## Glimpse of the working solution
![Image](https://github.com/user-attachments/assets/d0dbf38a-2223-4852-aca8-c3404e07e8d6)

![Image](https://github.com/user-attachments/assets/10fb4b30-54ea-434d-a67e-4e2629ebac8d)

![Image](https://github.com/user-attachments/assets/da7c545d-20b5-40be-8f76-bdb652f0a1b1)

![Image](https://github.com/user-attachments/assets/3348172a-39da-42d4-b7b8-a2aea938ffd3)

![Image](https://github.com/user-attachments/assets/e0e7dbba-e7c7-45f8-91c9-7d77d627c403)

![Image](https://github.com/user-attachments/assets/c9532014-a832-4a8f-af91-cd8ca5b4614b)

![Image](https://github.com/user-attachments/assets/ca2370f2-6fda-41bd-9a42-6b3043060628)

![Image](https://github.com/user-attachments/assets/7d553419-ab97-4b9c-973c-bd944ddbe900)

![Image](https://github.com/user-attachments/assets/b2581740-f94c-46cd-8e41-863eed23264e)

![Image](https://github.com/user-attachments/assets/f499ecb0-f70e-4f8e-9492-892025174cbf)

![Image](https://github.com/user-attachments/assets/75a601cf-fe92-4665-9fed-ae09645bbc5d)

![Image](https://github.com/user-attachments/assets/2393a3bf-0351-492a-b1f5-823c9abeccac)

![Image](https://github.com/user-attachments/assets/7ba59cad-a6c7-4b42-bee7-eb0578923f0f)

![Image](https://github.com/user-attachments/assets/d62923d1-a1a8-472d-b783-4eeaf5903e4e)

![Image](https://github.com/user-attachments/assets/c6599e31-5010-4fc5-b9e1-5f7a6f2787aa)

![Image](https://github.com/user-attachments/assets/a7a25a79-188f-49e5-9acc-977abe58bcc3)

![Image](https://github.com/user-attachments/assets/64e0ac99-5640-4519-8a6a-749c5b1502c7)

![Image](https://github.com/user-attachments/assets/dfaed8fc-efd0-4e5b-a686-24602ccd747e)

![Image](https://github.com/user-attachments/assets/73c8d7c9-7de9-4699-a537-2819113427e3)

![Image](https://github.com/user-attachments/assets/7941477e-323f-46c7-b0b1-4156d6fa66d0)

![Image](https://github.com/user-attachments/assets/7763fef8-b2bb-4cb6-81d8-8fc6bfad3a23)

![Image](https://github.com/user-attachments/assets/63e49622-a87a-409b-bc8f-047133378a13)

![Image](https://github.com/user-attachments/assets/f2da75af-a08c-444d-8834-843604dcec21)

![Image](https://github.com/user-attachments/assets/bfb87536-3a94-4484-8b8b-206a3addf66e)

## Project Overview

**Objective:** This is a Library Management System (LMS) web application built with .NET Core, designed to streamline the process of managing books, users, and borrowing records in a library. The system allows librarians and administrators to track books, users, issue/return books, and manage user roles.

## Tech Stack
-Backend: .NET Core 8 
-Frontend: HTML, CSS (Bootstrap or other frameworks can be used)
-Database: SQL Server (or SQLite for local setup)
-Authentication: ASP.NET Core Identity for user authentication
-ORM: Entity Framework Core for database interactions

## Key Features
-**Clean Architecture**: design for building robust, maintainable, testable systems and separation of concerns, independence from frameworks,and a clear structure of dependencies.it’st suited for larger projects 
-**WhatsApp Integration**: Subscribers and Users can receive notifications and updates via WhatsApp (e.g., book availability, due dates, reminders).

-**Email notificatin**: Subscribers and Users can receive notifications like welcom notification and renew notification. 

-**background tasks**: operations that run asynchronously or in the background, typically without direct interaction with a user.

-**Export Reports**:Export Excel sheet and Pdf for books , rentals and delay in rentals.

-**Save Images**:Save images on cloud (Cloudinary).

-**User Authentication**: Admin and user login functionality.

-**Book Management**: Add, update, delete, and search books in the library.

-**Borrowing System**: Issue and return books with due dates.

-**User Management**: Manage user profiles and roles (Admin/User).

-**Search Functionality**: Search for books by title and search for subscriber by email ,notional id phon number.

-**Book Categories**: Organize books by genre/category.

-**Responsive UI**: A simple, clean, and responsive user interface.




## Development Focus

### 1. [Genaric Repository Pattern](#repository-pattern)
- **Description:** Implement the Repository Pattern to abstract data access logic, making the code more testable and maintainable. 
- **Functionality:**
  - **Genaric Repository Pattern:** Simplifies data access by providing a consistent API for CRUD operations.
  - **Unit of Work:** Manages transactions across multiple repositories, ensuring data integrity.


### 2. [Entity Framework Core](#entity-framework-core)
- **Description:** Handle database interactions using Entity Framework Core, allowing for seamless integration with the database. The use of code-first migrations ensures that the database schema is in sync with the application models.
- **Features:**
  - **Code-First Migrations:** Automatically generate database schema from your code.
  - **Entity Mapping:** Ensure proper mapping of domain entities to database tables.

### 3. [Theme Integration](#theme-integration)
- **Description:** Integrate a pre-built theme into the project. Customize the theme to match the project’s requirements, ensuring it works well with ASP.NET Core.
- **Customization:** Modify the theme to align with the branding and functional needs of the project, ensuring consistency across all pages.

### 4. [User and Role Management](#user-and-role-management)
- **Description:** Implement user and role management using Microsoft ASP.NET Identity. This includes functionalities for user registration, login, role assignment, and permissions management.

### 5. Admin Capabilities
- **Create, Update, Delete Books:** Manage the books inventory with full CRUD operations.
- **Manage Categories:** Add, edit, or remove categories.
- **Manage Books:** Add, edit, or remove books.
- **Manage Subscriber** Add ,renew Subscriber's subscribtion,delete Subscriber's subscribtion,check if subscription paid penality and check if return book that he rantal.
- **User Management:** Create users, assign roles, lock or unlock users, and delete users as needed.
- **Role Assignment:** Define and assign roles such as Admin, Achieve, and Reception, with specific permissions.

### 6. Auth Section
- **Login:** Secure user authentication.
- **Reset Password:** Provide password recovery options.
- **Confirm Email** Sent Email virefication to user to avoid fake emails.
- **Edit Profile:** Enable users to update their personal information and settings.

### 7. [JavaScript Libraries](#javascript-libraries)
- **Description:** Utilize JavaScript libraries to enhance the user interface and provide dynamic functionality.
- **Libraries:**
  - **jQuery DataTables:** For dynamic tables with features like sorting, searching, and pagination.
  - **Toaster JS:** For displaying user-friendly notifications throughout the application.
  - **TinyMCE:**  user-friendly To formattin description book input.
  -  **Select2:**  user-friendly To select book's category and book's author with search.
  - **DateRangPicker:**  user-friendly To select range date time.
  - **Countup.Js:**  user-friendly for numbers.
  - **Flightbox:**  user-friendly for dispblay images.
  - **Bootbox Js confirmed:**  user-friendly to suring action.
  - **TypeAhead:**  user-friendly to searching.

### 8. [Admin Dashboard](#admin-dashboard)
- **Description:** Develop an admin dashboard that allows system administrators to manage the project effectively.
- **Features:**
  - **User Management:** View, create, update, and delete users.
  - **Role Management:** Assign and manage user roles.
  - **Book Management:** Oversee book listings, including adding, editing, and deleting products.
  - **rentals Management:** Add ,renew Subscriber's subscribtion,delete Subscriber's subscribtion,check if subscription paid penality and check if return book that he rantal.
  - **Charts:** Rentals Per Day,Subscribers Per City.

### 9. [Pagination](#pagination)
- **Description:** Implement pagination to manage large sets of products across multiple pages, ensuring a user-friendly experience.
- **Functionality:** Pagination will be integrated with search and filter functionalities to allow users to easily navigate through products.

### 10. [Session Management](#session-management)
- **Description:** Manage user sessions effectively, ensuring data is maintained across user interactions with the site.
- **Session Features:** 
  - Maintain user state across different pages.
  - Secure sensitive session data.

### 12. [Publishing to Monester](#publishing-to-monester)
- **Description:** Deploy the project on Monester, ensuring the deployment process is smooth and the application is optimized for the platform.
- **Deployment Focus:** Ensure the application is configured for performance, security, and scalability in a cloud environment.

### 13. [Publishing locally on IIS (Internet Information Services)](#Publishing-locally-on-IIS-(Internet-Information-Services))
- **Isolated Environment:** Running your website locally on IIS allows you to test and debug in an environment
    that is isolated from your production server. This helps in identifying and fixing issues without affecting live users.

### 14. [Data Seeding](#data-seeding)
- **Description:** Seed initial data for the admin role and users to ensure the system starts with essential data, improving ease of testing and initial use.
- **Seeded Data:**
  - **Admin Role:** Pre-configured admin role with full access.
  - **Sample Users:** Initial users with different roles for testing purposes.
---

## Links
- **Video Demo:** https://www.linkedin.com/feed/update/urn:li:activity:7289225228549812224/
- **Live Demo:** https://bookhaven.runasp.net
- **Email:** MoAdmin@BookHaven.Com 
- **Password:** P@$$0Rd123
