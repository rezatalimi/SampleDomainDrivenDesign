# Sample Domain Driven Design (DDD)
DDD Architecture Sample Code
This project is a small code sample demonstrating the implementation of Domain-Driven Design (DDD) architecture. In this example, an effort has been made to adhere to design principles at both High-Level and Low-Level.

The project structure is designed to maintain proper separation between the Domain, Application, Infrastructure, and Presentation layers.

In this sample project, the implementation of Token usage is inspired by RFC 6749, with its link provided at the end of the text. Token authentication is handled on the server side, and JWT is not used.

https://datatracker.ietf.org/doc/html/rfc6749#section-4.4

Access level control is fully customized and configured using [DataAnnotation] attributes based on the user's role.

```csharp
        [AllUserRoles]
        [HttpGet("get-current-user")]
        public Task<GetCurrentUserResultQuery> GetCurrentUser()
        {
            var filterQuery = new GetCurrentUserFilterQuery();

            return Distributor.Pull<GetCurrentUserFilterQuery, GetCurrentUserResultQuery>(filterQuery);
        }
```

The naming convention for API methods follows the standard REST API Design Best Practices.

https://restfulapi.net/resource-naming/

To test and run the project, after installing the required NuGet packages, you need to update the database connection string (ConnectionStrings).

```csharp
  "ConnectionStrings": {
    "Sample_DB": "Initial Catalog=Sample_DB;Trusted_Connection=false;Trust Server Certificate=True;Data Source=DESKTOP-8RLOL7H;User Id=sa;Password=123"
  },
```
