# Freelance

🎨 Freelance Web
Freelance Web is a dynamic platform designed to connect freelancers and clients for seamless collaboration on projects. It offers an intuitive interface for job postings, proposal submissions, profile management, and ratings, making it easy to hire talent or find work.

🚀 Features
🔒 Authentication & Authorization
JWT token-based authentication for secure access.
Role-based access control for Clients and Freelancers.

📋 Job Management
Clients can post jobs with details: budget, deadline, and category.
Freelancers can search, filter, and submit proposals.
📧 Proposal Handling
Track proposal status—Pending, Accepted, or Rejected.
Clients can review and accept proposals.
⭐ Ratings & Reviews
Clients can rate and review freelancers after project completion.
🛠️ Admin Panel
Manage users, jobs, and activity logs.
🔔 Notifications
Get real-time updates for job activity, proposals, and ratings.
🛠️ Technologies Used
Backend: .NET Core, ASP.NET MVC
Frontend: HTML, CSS, JavaScript
Database: SQL Server
Authentication: IdentityUser with JWT Tokens
Mapping: AutoMapper for DTO conversions

📦 Installation
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
