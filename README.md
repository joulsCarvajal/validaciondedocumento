Document Validation API

This project is a document validation API that allows you to verify identity documents using a specific access token, account_id, and custom_type. It processes .jpeg files of identity documents for validation. Follow the steps below to set up and run the API.

Requirements
* Visual Studio 2022
* Postman

Project Setup
1. Open the "pruebatr.sln" project in Visual Studio 2022.
2. In the project file, locate the fileBytes variable to load the path of the front image of the document.
3. Find the fileBytes2 variable to load the file path of the image on the back of the document.
4. Make sure the paths are correct and point to the local file locations.

Running the Server
1. Compile and run the project.
2. The server will be listening for requests on http://localhost:8080/.

Using Postman
To perform the document validation test, follow these steps:

1. Open Postman.
2. Create a new request using the POST method.
3. Set the URL of the request as: http://localhost:8080/.
4. Submit the request to start the document validation process.

Note
A basic React page was created to interact with the server, but there were CORS (Cross-Origin Resource Sharing) issues. However, tests using Postman work perfectly. You can view the repository to see the work done: https://github.com/joulsCarvajal/pruebasreactapi.git
