<h2>Task Management Rest API</h2>
RESTFUL API for managing tasks in form of cards done in .Net Core 8 and MSSQL.<br>
Key technolgies, best practises and design pattersns used include:<br>
<ul>
  <li>Dependency injection and Separation of concerns</li>
  <li>CQRS Pattern with MediaTR</li>
  <li>Api Documentation with Swagger</li>
  <li>Pagination and Filtering</li>
  <li>Logging</li>
  <li>Identity, Authentication and Authorization using JWT tokens</li>
  <li>Request body validation</li>
  <li>API Versioning</li>
  <li>Containerization support using Docker</li>
  <li>Robust Error Handling</li>
  <li>Version Control using Git</li>
</ul>

<b>Requirements for Building and Testing the API:</b><br>
 <li>1. Visual Studio 2019 + </li>
  <li>2. Microsoft SQL Server 2017 + </li>
  <li>3. Optional(Docker Desktop)</li>
  <li>4. Any modern web browser/ Postman </li>
  
The only configuration required is changing database conection string in appsettings.json to enable connection to your chosen MSSQL instance<br>

Upon running the project, migrations will be applied automatically and afew users seeded to the database.<br>

Once Swagger UI tab is opened in the broswer, obtain an access token from the /api/v1/Login endpoint using credentials for any of the seeded users. see login request below:


  ```
  {
  "email": "admin@demo.com",
  "password": "12345"
  }
  ```

Use the token generated to authorize within Swagger UI or Postman and add some Cards using /api/v1/Card endpoint. See below sample payload:

```
{
  "name": "My first Task",
  "description": "This is my first demo task",
  "color": "#000000",
  "status": 0,
  "createdBy": 0
}
```

Note: Status is an int 0-ToDo, 1-InProgress, 2-Done mapped by an Enum<br>
CreatedBy will be overriden by userId of logged user (This will be handled better by a DTO object in the near future :blush:)<br>

Play around with the other endpoints by testing several test cases <br>

Thank you for checking out this repository :heart:



