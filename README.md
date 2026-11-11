# 🏃‍♀️🏁 Race_Day

## 📌 Project Overview

Race_Day is a race and event management system designed to manage users, organisers, participants, events, event categories, enrolments, and race results.

The system uses a relational database to store and manage race-day information and provides REST API endpoints for interacting with the system. 🗄️💻

---

## 📚 Assignment Description

This assignment focuses on the design and development of a Race_Day race and event management system. The system demonstrates the use of database design, relational database management, REST API development, user roles, application development, and version control.

The project demonstrates the ability to:

- 🗄️ Design and implement a relational database.
- 🔗 Establish relationships between database tables using Primary Keys (PKs) and Foreign Keys (FKs).
- 🔌 Design REST API endpoints for system functionality.
- 👥 Implement different user roles and responsibilities.
- 📝 Manage participant enrolments in race events.
- 🏆 Record and manage race results.
- 💻 Develop application functionality using MVC.
- 🐙 Use GitHub for version control and project documentation.
- ⚙️ Use GitHub Actions to validate the repository structure.

---

# 🗄️ Part 1 – Database Design

## Database

The database is named:

**Race_Day**

### 📋 Database Tables

| Table | Description |
|---|---|
| 👤 Account | Stores user account information |
| 🧑‍💼 Organiser | Stores information about event organisers |
| 🏃 Participant | Stores information about race participants |
| 🏁 Event | Stores race and event information |
| 🏷️ Category | Stores available event categories |
| 🔗 EventCategory | Links events with their available categories |
| 📝 Entry | Stores participant enrolments in events |
| 🏆 Result | Stores participant race results |

### 🔗 Database Relationships

The database uses Primary Keys (PKs) and Foreign Keys (FKs) to maintain relationships and data integrity.

Key relationships include:

- 👤 An Account can be associated with an Organiser or Participant.
- 🧑‍💼 An Organiser can create multiple Events.
- 🏁 An Event can have multiple Categories.
- 🏃 A Participant can enter Events through the Entry table.
- 📝 Each Entry can have a corresponding Result.
- 🔗 Events and Categories are connected through the EventCategory relationship.

---

# 🔌 Part 2 – REST API Development

The Race_Day system provides REST API endpoints for managing accounts, profiles, events, categories, enrolments, and race results.

## 🔐 Authentication

| Method | Route | Description |
|---|---|---|
| 🟢 POST | `/api/auth/register` | Registers a new user account |
| 🟢 POST | `/api/auth/login` | Authenticates a registered user |

## 👤 Profile

| Method | Route | Description |
|---|---|---|
| 🔵 GET | `/api/profile` | Retrieves the logged-in user's profile |
| 🟡 PUT | `/api/profile` | Updates the logged-in user's profile |

## 🏁 Events

| Method | Route | Description |
|---|---|---|
| 🔵 GET | `/api/events` | Retrieves all available events |
| 🔵 GET | `/api/events/{id}` | Retrieves details of a specific event |
| 🟢 POST | `/api/events` | Creates a new event |
| 🟡 PUT | `/api/events/{id}` | Updates an existing event |
| 🔴 DELETE | `/api/events/{id}` | Deletes an existing event |

## 🏷️ Categories

| Method | Route | Description |
|---|---|---|
| 🔵 GET | `/api/categories` | Retrieves all available categories |
| 🔵 GET | `/api/categories/{id}` | Retrieves details of a specific category |
| 🟢 POST | `/api/categories` | Creates a new category |

## 🔗 Event Categories

| Method | Route | Description |
|---|---|---|
| 🟢 POST | `/api/events/{eventId}/categories/{categoryId}` | Associates a category with an event |
| 🔵 GET | `/api/events/{eventId}/categories` | Retrieves categories associated with an event |

## 📝 Enrolments

| Method | Route | Description |
|---|---|---|
| 🟢 POST | `/api/events/{eventId}/enrolments` | Enrols a participant in an event |
| 🔵 GET | `/api/events/{eventId}/enrolments` | Retrieves participants enrolled in an event |
| 🔵 GET | `/api/enrolments` | Retrieves the logged-in participant's enrolments |
| 🔴 DELETE | `/api/events/{eventId}/enrolments` | Cancels a participant's enrolment |

## 🏆 Results

| Method | Route | Description |
|---|---|---|
| 🔵 GET | `/api/events/{eventId}/results` | Retrieves results for an event |
| 🔵 GET | `/api/results/{id}` | Retrieves a specific race result |
| 🟢 POST | `/api/entries/{entryId}/results` | Records a participant's result |
| 🟡 PUT | `/api/results/{id}` | Updates an existing result |

---

# 💻 Part 3 – MVC Application Development

Part 3 focuses on developing the application layer of the Race_Day system using the Model-View-Controller (MVC) architecture.

The MVC application provides a website through which organisers and participants can interact with the Race_Day system. It communicates with the RaceDay API to retrieve, send, and update information instead of directly handling all database operations through the website.

This part brings the database and API functionality together with a user-facing application. 🚀

## 🏗️ Understanding the MVC Architecture

The MVC architecture separates the application into three main components:

### 🎮 1. Model

The Model represents the information used by the application.

In Race_Day, the ViewModels hold information such as:

- 👤 User account details.
- 🏁 Race event information.
- 🏷️ Event categories.
- 📝 Participant enrolments.
- 🏆 Race results.
- 📋 Participant profiles.

### 🖥️ 2. View

The View is the part of the application that users see and interact with.

It displays information and provides forms through which users can enter details, log in, register, view events, enrol in races, and manage their profiles.

### ⚙️ 3. Controller

Controllers handle user requests and decide what actions the application should perform.

For example, when a participant wants to view available events, the ParticipantController can request event information from the API and return it to the appropriate view.

Together, these three components help organise the application and separate the different responsibilities. 🔗

## 🎮 MVC Controllers

Controllers manage the actions performed within the website.

The project includes the following controllers:

### 🏠 HomeController

The HomeController manages the website's home page.

- Displays the main landing page.
- Uses the `Index()` action to load the home page.
- Returns the appropriate view when a user visits the website.

### 🔐 AccountController

The AccountController manages user authentication and account registration.

- Displays the login form.
- Receives the user's email address and password.
- Sends login information to the API.
- Displays the registration form for new users.
- Sends registration information to the API.
- Handles the response received from the API.

This allows users to access their accounts or register for the Race_Day system.

### 🧑‍💼 OrganiserController

The OrganiserController handles functions available to race organisers.

- Retrieves race events from the API.
- Displays event information.
- Provides a form for creating new events.
- Sends new event information to the API.
- Allows organisers to manage race event information.

This helps organisers manage the events they are responsible for.

### 🏃 ParticipantController

The ParticipantController manages participant activities.

- Retrieves available race events.
- Displays event information to participants.
- Allows participants to select a race and category.
- Receives enrolment information.
- Sends participant enrolment details to the API.

This allows participants to interact with the available race events and register their interest in taking part.

### 👤 ProfileController

The ProfileController manages participant profile information.

- Retrieves participant profile details.
- Displays personal information.
- Receives updated profile information.
- Sends profile changes to the API.
- Uses update requests to modify existing information.

This allows participants to view and maintain their profile details.

## 📦 ViewModels

ViewModels are used to transfer information between the views and controllers.

They help organise the information entered by users and the information displayed by the application.

The project includes:

| ViewModel | Purpose |
|---|---|
| 🔐 `LoginViewModel` | Stores login information, including email and password |
| 📝 `RegisterViewModel` | Stores information required to register a new account |
| 🏁 `EventViewModel` | Stores race event information such as name, date, and location |
| 🏷️ `CategoryViewModel` | Stores information about race categories |
| 🔗 `EntryViewModel` | Stores participant enrolment information |
| 🏆 `ResultViewModel` | Stores race result information |
| 👤 `ProfileViewModel` | Stores participant profile information |

### 🔍 Why Are ViewModels Important?

- 📋 They organise information into clear properties.
- 🔄 They help transfer data between controllers and views.
- ✅ They can include validation rules to check user input.
- 🧩 They help keep the application organised.
- 👀 They make it easier to display the correct information to users.

## 🔌 API Communication – ApiService.cs

The `ApiService.cs` class helps the MVC website communicate with the RaceDay API.

It uses `HttpClient` to send requests and receive responses from the API.

The main HTTP operations include:

| HTTP Method | Purpose | Example |
|---|---|---|
| 🔵 GET | Retrieves existing information | Viewing available race events |
| 🟢 POST | Sends new information | Registering an account or creating an entry |
| 🟡 PUT | Updates existing information | Updating a participant's profile |
| 🔴 DELETE | Removes information | Deleting an existing record |

### 🔄 How Does the MVC Website Communicate with the API?

The communication process works as follows:

1. 👤 A user performs an action on the website, such as logging in or viewing events.
2. 🎮 The relevant controller receives the user's request.
3. 📦 The controller prepares the information using the appropriate ViewModel when needed.
4. 🔌 The application sends an HTTP request to the RaceDay API.
5. 🗄️ The API processes the request and communicates with the database when required.
6. 📩 The API sends a response back to the MVC application.
7. 🖥️ The controller uses the response to display the appropriate information to the user.

This communication allows the MVC website and API to work together as part of one system.

## ✅ Form Validation

Validation helps ensure that users enter the required information correctly.

The ViewModels can use validation attributes such as:

- ` [Required] ` – Makes sure a required field is completed.
- ` [EmailAddress] ` – Checks that an email address follows the expected format.
- ` [Compare] ` – Compares two values, such as a password and confirm password.
- ` [DataType(DataType.Password)] ` – Identifies a password field.

For example, when registering an account, the user must provide the required information. The validation rules help identify missing or incorrectly entered details before the information is processed.

## 🏆 ResultPublicationController

The ResultPublicationController belongs to the RaceDayAPI project.

It manages the retrieval and publication of race results.

Its responsibilities include:

- 🔎 Retrieving available race results.
- 📝 Receiving race result information.
- 🗄️ Using the database context to access the database.
- ➕ Adding new results to the database.
- 💾 Saving changes using `SaveChanges()`.
- 📩 Returning an appropriate API response.

The controller uses HTTP methods such as GET and POST to handle result-related requests.

This allows race results to be stored and made available through the API.

## 🎯 What Part 3 Demonstrates

Part 3 brings together the different components developed throughout the Race_Day project.

It demonstrates the ability to:

- 💻 Develop an application using ASP.NET Core MVC.
- 🏗️ Apply the Model-View-Controller architecture.
- 🎮 Use controllers to handle user requests.
- 📦 Create and use ViewModels to transfer information.
- 🔌 Connect an MVC application to a REST API.
- 📡 Work with HTTP requests and API responses.
- ✅ Apply validation to user input.
- 👥 Support organiser and participant activities.
- 🏁 Manage race events and participant enrolments.
- 🏆 Retrieve and publish race results.

**Overall, Part 3 connects the application functionality with the API, allowing users to interact with the Race_Day system through a website.** 🎉

---

# 👥 User Roles

The Race_Day system has two main user roles: Organiser and Participant.

## 🧑‍💼 Organiser

An Organiser is responsible for managing race events and overseeing participants and results.

Organisers can:

- 🏁 Create and manage race events.
- 🏷️ Create and manage event categories.
- 🔗 Associate categories with events.
- 👥 View participants enrolled in their events.
- 🏆 Record race results.
- ✏️ Update race results.
- 📋 Manage information relating to their events.

## 🏃 Participant

A Participant is a user who takes part in race events managed through the system.

Participants can:

- 🔎 View available race events.
- 📋 View event details.
- 📝 Enrol in available events.
- 👀 View their own enrolments.
- ❌ Cancel their enrolments.
- 🏆 View race results.

The two roles ensure that users have appropriate responsibilities within the system and that event management activities are separated from participant activities. 🔐

---

# 🛠️ Technologies Used

- 🗄️ Microsoft SQL Server
- 💾 SQL
- 🔌 REST API
- 💻 ASP.NET Core MVC
- 🌐 C#
- 🐙 GitHub
- ⚙️ GitHub Actions

---

# ⚙️ Continuous Integration (CI)

GitHub Actions is used as part of the project's Continuous Integration (CI) process.

The CI workflow automatically checks that the required project documentation files are present in the repository whenever changes are pushed to the main or master branches or when a pull request is created.

The workflow checks for:

- 📄 `API_Endpoint_Plan.pdf`
- 🖼️ `ERDDIAGRAM.drawio.png`
- 🗄️ `RACE_DAY PROG_POE1.sql`
- 📖 `README.md`

A successful workflow produces a green build, confirming that the required files have been found successfully. ✅

### 🟢 Successful CI Build

[Successful GitHub Actions Build](https://github.com/Nanka2411/RaceDay_POEPART1/blob/POE-PART-3-FINAL/github-actions-success.png)

[View Build Image](https://github.com/Nanka2411/RaceDay_POEPART1/raw/POE-PART-3-FINAL/github-actions-success.png)

---

# 🎥 Project Walkthrough Video

[▶️ Watch my Race_Day Project Walkthrough on YouTube](https://youtu.be/yLE7oBicH00)

---

# 💻 Part 3 – Code Explanation Video

Want to see how the Race_Day application code works? 🏁

Watch my Part 3 video, where I explain the code added to the project, including the controllers, ViewModels, API communication, and how the different classes contribute to the system.

▶️ 

---

# 🎯 Project Purpose

The purpose of Race_Day is to provide a structured and reliable system for managing race events and participants.

The system brings together:

🏁 Events  
👥 Participants  
🧑‍💼 Organisers  
🏷️ Categories  
📝 Enrolments  
🏆 Race Results  

All while maintaining data integrity, organised database relationships, and communication between the application and API. 🔐✨

---

# 🚀 Race Day Ready!

🏁 Plan the race.

🏃 Manage the participants.

🏆 Track the results.

💻 Build, connect, and manage it all with Race_Day!

**Let's get Race Day ready!** 🎉
