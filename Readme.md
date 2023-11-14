#Challenge Description
The task is to develop a web API containing two endpoints, "Set Timer" and "Get Timer Status". The "Set Timer" endpoint should receive a JSON input including hours, minutes, seconds, and a web URL.
Upon receiving this input, the endpoint should generate a JSON output with a single field, "id" representing the timer’s unique identifier.

The endpoint must also define an internal timer that will send a webhook to the specified URL after the timer runs out (a POST HTTP call with an empty body).

The second endpoint, "Get Timer Status," should receive a timer ID as a resource ID in the URL. Upon receiving this input, the endpoint should generate a JSON output containing the number of seconds left until the timer expires. If the timer has expired, the endpoint should return "0".
Happy flow for example:
Create a new Timer request body, for example:
{ "hours": 0, "minutes": 1, "seconds": 0, "webhookUrl": "http/s://{domain}/api/timers/outpost" }

Get the status of the created Timer expected response, for example

{ "id": "dcfc8df0-ed93-48fc-8bb5-9e376be06fce", "timeLeft": 36 }

List the timers expected response for example (The specified timer status is "Started")

{ "pageNumber": 1, "pageSize": 100, "items": [ { "id": "dcfc8df0-ed93-48fc-8bb5-9e376be06fce", "dateCreated": "2023-05-07T05:31:16.3350923+00:00", "hours": 0, "minutes": 1, "seconds": 0, "timeLeft": 36, "webhookUrl": "http/s://{domain}/api/timers/outpost", "status": "Started" } ], "totalRowCount": 1 }

Wait for 1 min.

The webhook URL is triggered.

List the timers expected response for example (The specified timer status updated to "Finished")

{ "pageNumber": 1, "pageSize": 100, "items": [ { "id": "dcfc8df0-ed93-48fc-8bb5-9e376be06fce", "dateCreated": "2023-05-07T05:31:16.3350923+00:00", "hours": 0, "minutes": 1, "seconds": 0, "timeLeft": 36, "webhookUrl": "http/s://{domain}/api/timers/outpost", "status": "Finished" } ], "totalRowCount": 1 }

Your solution must have the following features:

Persistent timers will continue running even if the server is shut down and restarted.

The server should send the webhook if a timer is supposed to fire when the server is down.


#Database and ConnectionString
Create SQL Server db timers or update the connection string

"DefaultConnection": "Server=localhost;Database=timers;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

if migrations are not applied automatically plase run Update-Database ef migrations command from package manager console in VS


Api starts with swagger so you can pick all the relevant information and try api from there.


 
