# Jira Board 

### Goal of this project was to learn how to use gRPC framework with C# language.

The project is made up of **4 services** (the code for the Next.js client service is not located in this repository). Originally, there were only meant to be **3 services** (without the ASP.NET service), which would connect the gRPC service with the frontend client using RPC. However, this solution was not well-supported, and complications arose.

Since this project is for learning purposes, I decided to add an additional **ASP.NET service** to "convert" gRPC requests into HTTP requests for the client. This approach makes more sense because the **RPC protocol** is typically used for more complex scenarios (with a higher throughput of data) — such as streaming large amounts of data between two services.

![image](https://github.com/user-attachments/assets/022950e6-670d-4dcd-8a48-45d1efd51c46)

#### gPRC Service (main component) - name: JiraBoardgRPC

This service uses **gRPC (Google Remote Procedure Call)** for efficient communication between distributed systems.

- Acts as the **main backend component** of the application.
- Implements a **gRPC server** exposing a set of remote procedures defined in `.proto` files.
- Supports **client-server communication** over HTTP/2 using **Protocol Buffers** for serialization.
- Follows modern architectural practices by utilizing **Dependency Injection** for service registration and component management.

![image](https://github.com/user-attachments/assets/d6775141-0b9a-4eaa-8735-9e08118a7d18)
![image](https://github.com/user-attachments/assets/57fc1920-6173-4aeb-9e07-f3b3bc036c6f)


#### ASP.NET Service (Second Main Component for HTTP Requests) - name: MiddleStepService

This service is built with **ASP.NET Core** and serves as the second main backend component, responsible for handling traditional **HTTP/REST requests**.

- Implements a **RESTful API** interface for clients that do not use gRPC.
- Allows for integration with browsers, frontend applications, or third-party services over standard **HTTP**.
- Complements the gRPC service by offering an alternative communication method, especially suitable for frontend and web-based clients.
- Internally, this service acts as a **gRPC client**, using the shared `.proto` file to communicate with the main gRPC backend.

![image](https://github.com/user-attachments/assets/3c200b1d-4f16-44da-b5c7-af2dbf8adabe)


#### Integration Tests Service - name: GrpcTests

This service is used to connect with both the **gRPC service** and the **ASP.NET service** to perform integration tests. It acts as a **gRPC client**, interacting with the backend services to simulate real-world scenarios and validate the system's behavior.

- Connects to the **gRPC service** to verify the correctness of remote procedures.
- Communicates with the **ASP.NET service** to test HTTP/REST endpoints.
- Ensures end-to-end functionality by simulating requests and validating responses across both services.

![image](https://github.com/user-attachments/assets/308312db-fbd5-4a45-b914-42449fefe7d0)

#### NEXT.JS Frontend Client

This is the **frontend client** built with **Next.js**, responsible for connecting with the **ASP.NET service** to interact with backend functionalities.

- **Connects to the ASP.NET service** via HTTP to make requests to the backend.
- Uses **JWT (JSON Web Tokens)** for authentication, requiring users to log in before accessing certain features.
- Provides a **simple user interface** designed to be intuitive and easy to navigate.
- Code for this client service is not located in this repository.
  
General board:

![image](https://github.com/user-attachments/assets/3b0a3702-ed1a-4b00-b219-a084e7824284)

Task preview:

![image](https://github.com/user-attachments/assets/ac8a9c8f-3fa5-48ae-a17e-98b05c21d51f)

Login:

![image](https://github.com/user-attachments/assets/e9be9d41-82c1-49e8-ab4c-c3c66f2b91cf)

Add Task:

![image](https://github.com/user-attachments/assets/4f1cea5c-e7ba-4484-8a38-95c4b274bcb1)

**This project was developed as part of the Advanced Internet Technologies course and meets the requirements for using the following technologies:**

- Microservices in C#
- gRPC
- ASP.NET (Web API)
- Dependency Injection
- Next.js

**Next Goals to Improve This Application:**

- Introduce a NoSQL database for data storage (this might be implemented in this project or a separate one for another course)
- Implement Redis for caching
