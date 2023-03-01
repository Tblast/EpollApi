# Epoll Application
Epoll is a polling application that allows users to create and vote on polls.

## Technologies Used
The front-end of the application is built using Vue.js with Vite, while the backend API is built using C# .NET 7.

## Running the Backend
To run the backend, follow these steps:

1. Ensure that you have .NET Core SDK installed. If you don't have it, you can download and install it from the official .NET Core website: https://dotnet.microsoft.com/download.
2. Run dotnet restore in the project directory to restore any NuGet packages used in the project.
3. Run dotnet run in the project directory to start the server.

## Running the Front-end
To run the front-end, follow these steps:

1. Ensure that you have Node.js and npm installed. You can download Node.js and npm from the official website: https://nodejs.org/.
2. Install the project dependencies by running npm install in the project directory.
3. Start the development server by running npm run dev.
4. The URL to access the application will be displayed in the terminal.

## Usage
Once the application is running, you can create a poll by posting a question and its answer options to the backend. Polls can be listed and their options can be viewed by clicking them. A vote can be cast for a single option of a poll, and the backend will record the vote without identifying the voter. The backend will only count individual votes per option.