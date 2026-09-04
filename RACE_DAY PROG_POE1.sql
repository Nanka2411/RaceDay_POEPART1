--Initialises the Race_Day database

CREATE DATABASE Race_Day;
GO

USE Race_Day;
GO

-- 1. Creates Account Table

CREATE TABLE Account (
UserID INT PRIMARY KEY,
FirstName VARCHAR(55) NOT NULL,
LastName VARCHAR(55) NOT NULL,
Email VARCHAR(100) NOT NULL UNIQUE,
PasswordHash VARCHAR(150) NOT NULL,
Phone VARCHAR(10) NOT NULL,
UserRole VARCHAR(25) NOT NULL,
CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- 2. Creates Organiser Table

CREATE TABLE Organiser (
OrganiserID INT PRIMARY KEY,
UserID INT NOT NULL UNIQUE,
OrganiserName VARCHAR(100) NOT NULL,
OrganisationName VARCHAR(100) NOT NULL,
FOREIGN KEY (UserID) REFERENCES Account(UserID)
);
GO

-- 3. Creates Partcicpant Table

CREATE TABLE Participant (
ParticipantID INT PRIMARY KEY,
UserID INT NOT NULL UNIQUE,
ParticipantAge INT NOT NULL,
FOREIGN KEY (UserID) REFERENCES Account(UserID)
);
GO

-- 4. Creates Event Table

CREATE TABLE Event (
EventID INT PRIMARY KEY,
EventName VARCHAR(55) NOT NULL,
EventDescription VARCHAR(100) NOT NULL,
EventDate DATE NOT NULL,
EventLocation VARCHAR(55) NOT NULL,
RouteInformation VARCHAR(200) NOT NULL,
OrganiserID INT NOT NULL,
FOREIGN KEY (OrganiserID) REFERENCES Organiser(OrganiserID)
);
GO

-- 5. Creates Category Table

CREATE TABLE Category (
CategoryID INT PRIMARY KEY,
CategoryName VARCHAR(55) NOT NULL,
CategoryDescription VARCHAR(150) NOT NULL
);
GO

-- 6. Creates event category table

CREATE TABLE EventCategory (
 EventID INT NOT NULL,
CategoryID INT NOT NULL,
PRIMARY KEY (EventID, CategoryID),
FOREIGN KEY (EventID)REFERENCES Event(EventID),
FOREIGN KEY (CategoryID)REFERENCES Category(CategoryID)
);
GO

-- 7. Creates Entry Table

CREATE TABLE Entry (
EntryID INT PRIMARY KEY,
ParticipantID INT NOT NULL,
EventID INT NOT NULL,
CategoryID INT NOT NULL,
EntryDate DATE NOT NULL DEFAULT GETDATE(),
EntryStatus VARCHAR(25) NOT NULL DEFAULT 'ENTERED',
FOREIGN KEY (ParticipantID) REFERENCES Participant(ParticipantID),
FOREIGN KEY (EventID) REFERENCES Event(EventID),
FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
UNIQUE (ParticipantID, EventID)
 );
GO

-- 8. Creates result table

CREATE TABLE Result (
 ResultID INT PRIMARY KEY,
 EntryID INT NOT NULL UNIQUE,
 ResultTime VARCHAR(20) NOT NULL,
 Position INT NOT NULL,
 ResultStatus VARCHAR(25) NOT NULL DEFAULT 'Completed',
 FOREIGN KEY (EntryID) REFERENCES Entry(EntryID)
 );
GO

-- 9. Inserterion of account data

INSERT INTO Account
(UserID, FirstName, LastName, Email, PasswordHash, Phone, UserRole)
VALUES
(1, 'Matthew', 'Davids','MatthewDavids@gmail.com', 'hashedpassword1','0824367180','Organiser'),
(2, 'Astrid', 'Kapinga','Astrid_Kapinga@gmail.com', 'hashedpassword2','0781219087','Organiser'),
(3, 'Ruth', 'Eyume','Ruth.Eyume11@gmail.com', 'hashedpassword3','0654321899','Participant'),
(4, 'Nikita', 'Johnson','NikitaJohnson25@gmail.com','hashedpassword4','0641238906','Participant');
GO

-- 10.Inserterion of organiser data

INSERT INTO Organiser
(OrganiserID, UserID, OrganiserName, OrganisationName)
VALUES
(1, 1, 'Matthew Davids', 'Comrades Marathon'),
(2, 2, 'Astrid Kapinga', 'Sanlam Cape Town Marathon');
GO

-- 11. Inserterion of participatant data

INSERT INTO Participant
(ParticipantID, UserID, ParticipantAge)
VALUES
(1, 3, 27),
(2, 4, 29);
GO

-- 12. Inserterion of event data

INSERT INTO Event
(EventID, EventName, EventDescription, EventDate,
EventLocation, RouteInformation, OrganiserID)
VALUES
(1,'Durban Beach Marathon','Annual beachside marathon event','2026-09-20','Durban','42km beachfront route',1),
(2,'Pretoria Fun Ride','Recreational cycling event for the community','2026-10-18','Pretoria','30km city and suburban route',2),
(3,'Gqeberha Fitness Challenge','Outdoor fitness and wellness event','2026-11-08','Gqeberha','8km fitness trail route',1),
(4,'Stellenbosch Charity Walk','Fundraising walk supporting local community projects','2026-12-12','Stellenbosch','7km vineyard and town route',2);
GO

-- 13. Inserterion of category data

INSERT INTO Category
(CategoryID, CategoryName, CategoryDescription)
VALUES
(1,'21km Half Marathon','21 kilometre half marathon running category'),
(2, '42km Full Marathon','42 kilometre full marathon running category'),
(3,'15km Trail Run','15 kilometre off-road trail running category'),
(4,'30km Mountain Bike','30 kilometre mountain biking category'),
(5,'7km Charity Walk','7 kilometre charity walking category');
GO

-- 14. Inserterion of entry category data

INSERT INTO EventCategory
(EventID, CategoryID)
VALUES
(1, 1),
(1, 2),
(2, 4),
(3, 3),
(4, 5);
GO


-- 15. Inserterion of entry data

INSERT INTO Entry
(EntryID, ParticipantID, EventID, EntryDate, EntryStatus)
VALUES
(1, 1, 1, '2026-08-20', 'Entered'),
(2, 2, 1, '2026-08-20', 'Entered'),
(3, 1, 2, '2026-08-20', 'Entered'),
(4, 2, 3, '2026-08-20', 'Entered');
GO



-- 16. Inserterion of result data

INSERT INTO Result
(ResultID, EntryID, ResultTime, Position, ResultStatus)
VALUES
(1, 1, '01:32:45', 3, 'Completed'),
(2, 2, '01:18:30', 1, 'Completed'),
(3, 3, '02:05:15', 2, 'Completed'),
(4, 4, '00:56:42', 4, 'Completed');
GO


-- 17. DISPLAYS ALL THE DATA

SELECT * FROM Account;
SELECT * FROM Organiser;
SELECT * FROM Participant;
SELECT * FROM Event;
SELECT * FROM Category;
SELECT * FROM EventCategory;
SELECT * FROM Entry;
SELECT * FROM Result;
GO