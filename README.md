# 🕌 Prayer Times Backend

A .NET backend service that provides **Islamic prayer times** for cities worldwide.  
It fetches accurate times from the [Aladhan API](https://aladhan.com/prayer-times-api), stores them in a database, and makes them available via REST API.  
Additionally, it integrates with **Firebase Cloud Messaging (FCM)** to send notifications before prayer times.

---

## ✨ Features
- Fetch daily and monthly prayer times from Aladhan API.
- REST API endpoints for cities and countries.
- Scheduled jobs to clean up old records.
- Firebase integration for push notifications.
- Database persistence with **Entity Framework Core**.
---

## 🛠 Tech Stack
- **Backend**: ASP.NET Core Web API (.NET 8)
- **Database**: SSMS (default, but can be swapped)
- **ORM**: Entity Framework Core
- **Notifications**: Firebase Cloud Messaging (FCM)
- **External API**: [Aladhan Prayer Times API](https://aladhan.com/prayer-times-api)

---


🔔 Firebase Integration
This project supports push notifications for upcoming prayers via Firebase Cloud Messaging (FCM).

Create a Firebase project.

Get your Server Key from Firebase console.

Add it to appsettings.json under "Firebase".

The backend will send notifications to registered devices.

---

## 🚀 Getting Started

### 1. Clone the Repository

git clone https://github.com/your-username/prayer-times-backend.git
cd prayer-times-backend 


### 2. Configure Settings
Create or edit appsettings.Development.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=PrayerTimes;Username=DB;Password=yourpassword"
  },
  "Firebase": {
    "ServerKey": "your-firebase-server-key",
    "SenderId": "your-sender-id",
    "ProjectId": "your-project-id"
  }
}

### 3. Apply Database Migrations
dotnet ef database update
