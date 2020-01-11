# Scheduler Demo App
## Web API for appointments scheduling

The app is developed in DotNet Core 3.0 using Visual Studio Code.
For data storage I used SQLite.

### Solution structure

```
SchedulerAPP
  |
  +-- SchedulerApi (WebApi)
  |       |
  |       +-- Controllers
  |
  +-- SchedulerData (data layer)
  |       |
  |       +-- DataModels
  |       |
  |       +-- Repository
  |
  +-- SchedulerDomainModel (domain model)
  |
  +-- SchedulerIntegrationTest (integration tests)
  |
  +-- SchedulerUnitTest (unit test)
```

### Compiling the solution
Move to the solution folder an type

```
dotnet buid
```

### To add a Database migration
Move to SchedulerApi folder and type:

```
dotnet ef migrations add <NAME> -p ..\SchedulerData
```

### To update the Database
Move to SchedulerApi folder and type:

```
dotnet ef database update -p ..\SchedulerData
```

### To run Unit Test
Move to SchedulerUnitTest folder and type:

```
dotnet test
```

### To run Integration Tests
Move to SchedulerIntegrationTest folder and type:

```
dotnet test
```

## Web API

### Patch an appointment
```
PATCH appointments/5
Body: "2019-11-20T10:06:00"
```

### Get appointments by date
```
GET appointments/2020-03-16
```

### Get appointments by attendeeId and date
```
GET appointments/jo@gmail.com/2020-03-16
```

### Get a meeting by id
```
GET appointments/meetings/5
```

### Post a new meeting
```
POST appointments/meetings
Body: 
  {
    "title": "Meeting 4",
    "description": "My meeting",
    "isDone": false,
    "dateAndTime": "2020-03-01T14:56:00",
    "recurrencyType": 1,
    "attendees": [
    	{"attendeeID": "jo@gmail.com", "name": "jo"}
    ]
  }
```

### Patch the attendees in existing meeting
```
PATCH appointments/meetings/5
Body:
[
	{"attendeeID": "mario@gmail.com", "name": "mario"},
	{"attendeeID": "paolo@gmail.com", "name": "paolo"},
	{"attendeeID": "jo@gmail.com", "name": "jo"},
	{"attendeeID": "sara@gmail.com", "name": "sara"}
]
```

### Get a Reminder by Id
```
GET appointments/reminders/5
```

### Post a new Reminder
```
POST appointments/reminders
Body:
 {
    "title": "Reminder 4",
    "description": "My reminder",
    "isDone": false,
    "dateAndTime": "2020-03-01T14:56:00"
  }
```