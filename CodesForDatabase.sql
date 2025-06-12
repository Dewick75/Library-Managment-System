
create database DatabaseLibrary;
use DatabaseLibrary;


CREATE TABLE ClassificationTable (
    ClassificationId INT PRIMARY KEY IDENTITY(1,1),
    Classification VARCHAR(1) 
);

-- Create the Books table to store book details
CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1), 
    ClassificationId INT NOT NULL, 
    Title VARCHAR(100) NOT NULL,
    Publisher VARCHAR(100) NOT NULL, 
    Year VARCHAR(20) NOT NULL, 
    Reference BIT NOT NULL,
    Borrowable BIT NOT NULL, 
    CONSTRAINT FK_Classification FOREIGN KEY (ClassificationId) REFERENCES ClassificationTable(ClassificationId) 
);



-- Create the BookCopies table to store details about each copy of a book
CREATE TABLE BookCopies (
    CopyId INT PRIMARY KEY IDENTITY(1,1), 
    BookId INT, -- 
    CopyNumber VARCHAR(20) NOT NULL, 
    Available BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Book FOREIGN KEY (BookId) REFERENCES Books(BookId) 
);

-- Create the Members table to store member details
CREATE TABLE Members (
    MemberId VARCHAR(20) PRIMARY KEY, -- Unique identifier for each member
    Name VARCHAR(100) NOT NULL, 
    Sex VARCHAR(100) NOT NULL, 
    NationalID VARCHAR(20) NOT NULL, 
    Address VARCHAR(200) NOT NULL, 
    Telephone VARCHAR(20)
	Email VARCHAR(20),
  
);

-- Create the Loans table to store loan details
CREATE TABLE Loans (
    LoanId INT PRIMARY KEY IDENTITY(1,1), -- Unique identifier for each loan
    MemberId VARCHAR(20), 
    CopyId INT, 
    LoanDate DATE, 
    ReturnDate DATE,
    CONSTRAINT FK_Members_Loans FOREIGN KEY (MemberId) REFERENCES Members(MemberId), 
    CONSTRAINT FK_Copies_Loans FOREIGN KEY (CopyId) REFERENCES BookCopies(CopyId) 
);

-- Create the Reservations table to store reservation details
CREATE TABLE Reservations (
    ReservationId INT PRIMARY KEY IDENTITY(1,1), 
    MemberId VARCHAR(20),
    CopyId INT, 
    ReservationDate DATETIME, 
    FOREIGN KEY (MemberId) REFERENCES Members(MemberId), 
    FOREIGN KEY (CopyId) REFERENCES BookCopies(CopyId) 
);





-- Create the Admins table for admin login
CREATE TABLE Admins (
    AdminId INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(100) NOT NULL UNIQUE,      -- e.g., admin email
    PasswordHash VARBINARY(256) NOT NULL,       -- store hashed password
    PasswordSalt VARBINARY(128) NOT NULL,       -- store salt for hashing
    Name VARCHAR(100) NOT NULL,                 -- admin's name
    Email VARCHAR(100) NOT NULL UNIQUE,         -- admin's email
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
