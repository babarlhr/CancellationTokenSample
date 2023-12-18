# How to use CancellationToken in ASP.NET WebApi

#Cancellation Token Web API Sample
##Overview
This repository explores the usage of cancellation tokens in a web API, focusing on scenarios where tasks need to be canceled for effective resource management. The provided ASP.NET project demonstrates various API endpoints employing different methods of implementing cancellation tokens.

##Key Points
1. No Cancellation Token
- Simple API endpoint without cancellation token.
- Demonstrates a basic task that takes 30 seconds.
2. Cancellation Token Source
- API endpoint using **`'CancellationTokenSource'`**.
- Automatically cancels tasks running for more than 10 seconds.
3. Request Cancellation
- API endpoint utilizing built-in cancellation token from **`'HttpContext'`**.
- Cancels tasks if the client closes the browser or refreshes the page.
4. EF Query Cancellation
- API endpoint illustrating cancellation of long-running Entity Framework query.
5. Insights and Tips
- Emphasizes the importance of cancellation tokens in releasing resources.
- Use SQL Server Profiler tool for tracing database queries.
##Conclusion
This repository concludes by stressing the significance of cancellation tokens in managing asynchronous tasks, releasing resources, and ensuring efficient application performance. It provides practical insights for developers seeking to implement cancellation tokens in web APIs.
