Location tRACKER API
This project provides a simple .NET Web API for recording and retrieving location data. It allows clients to submit location information and retrieve stored data.

Prerequisites
.NET Core SDK (version 8.0 or later)
A database (e.g., SQL Server, SQLite, or PostgreSQL) to store location data (you’ll need to configure the connection string in appsettings.json)
Getting Started
Clone this repository to your local machine.
Open the solution in Visual Studio or your preferred IDE.
Build the solution to restore NuGet packages.
Configure your database connection string in appsettings.json.
Run the application.
API Endpoints
1. Record Location Data
Endpoint: POST /waypoint/add
Request Body:
JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "latitude": 0,
  "longitude": 0,
  "stopTime": "2024-05-02T11:21:55.105Z",
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string"
  }
}}

Response:
Status: 200 Created

2. Get Location Data by User
Endpoint: GET /waypoint/{userid}
Response:
Status: 200 OK
Body: List of location records

3. Get Last Location for User
Endpoint: GET /waypoint/lastlocation/{userid}
Response:
Status: 200 OK
Body: Single location record

3. Get Locations all users after date
Endpoint: GET /waypoint/lastlocationall/{earliestdate}
Response:
Status: 200 OK
Body: List of location records

Error Handling
Not yet implemented

Authentication and Authorization
Not yet implemented

Deployment
Deploy the API to your preferred hosting environment (e.g., Azure, AWS, or self-hosted).
