# Freelance

Freelance Web is a platform designed to connect freelancers and clients for seamless collaboration on projects. It provides an intuitive interface for job postings, proposal submissions, and profile management, making it easy to hire talent or find work.

Features
Authentication & Authorization
Secure JWT token-based authentication.
Role-based access control (Client and Freelancer).
User Profiles
Profile photos, descriptions, and work experience sections.
Editable profiles for freelancers and clients.
Job Management
Clients can post jobs with details like budget, deadline, and category.
Freelancers can search, filter, and submit proposals.
Proposal Handling
Track proposal status—pending, accepted, or rejected.
Clients can review and accept proposals directly.
Ratings & Reviews
Clients can rate and review freelancers after project completion.
Admin Panel
Admins can manage users, jobs, and activity logs.
Notifications
Real-time updates on job activity, proposal status, and ratings.
Technologies Used
Backend: .NET Core, ASP.NET MVC
Frontend: HTML, CSS, JavaScript
Database: SQL Server
Authentication: IdentityUser with JWT Tokens
Mapping: AutoMapper for DTO conversions
Containerization: Docker support
Installation
Clone the Repository

bash
Copy code
git clone https://github.com/yourusername/freelance-web.git
cd freelance-web
Configure Database

Ensure SQL Server is running.
Update the connection string in appsettings.json.
Run the Application

bash
Copy code
dotnet restore  
dotnet build  
dotnet run  
Access the Web App
Open your browser and visit:
http://localhost:5000
