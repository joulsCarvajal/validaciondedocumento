# documentvalidation Document Validation API
This project is a document validation API that uses a specific access token, account_id and custom_type, along with .jpeg files of the identity document. The steps required to set up and run the API are described below.

Requirements
Visual Studio 2022
Postman
Project Setup
Open the "pruebatr.sln" project in Visual Studio 2022.
In the project file, locate the line containing the fileBytes variable to load the path where the front image of the document is stored.
Similarly, find the fileBytes2 variable to load the file path of the image file on the back of the document. Make sure the paths are correct and point to the local file locations.
Running the Server
Compile and run the project.
The server will be listening for requests on port http://localhost:8080/.
Using Postman
To perform the document validation test, follow these steps:

Open Postman.
Create a new request using the POST method.
Set the URL of the request as: http://localhost:8080/.
Submit the request to start the document validation process.


** A basic page was created in react to make the calls to the server, but the cors problems continued. However the tests with postman work perfectly. I leave the repository so you can see the work done there. 

