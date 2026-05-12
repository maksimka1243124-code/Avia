# Avia-site
Second pet-project. A backend for ticket sales with roles, booking, administrative panel and basic validation.
## ⚙️ Tech stack

- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** + PostgreSQL
- **JWT Bearer Authentication**
- **Role-based authorization** (Admin, Dispatcher, User)
- **FluentValidation**
- **Custom middleware** (logging, user online status, rate limiting, IP filtering)
- **DTO** for most responses
- **Global exception handling** (via middleware + `BaseResponse<T>`)
- **Pagination** (via `Paginationed`)
- **Soft delete** (e.g. for flights)
  
## ✅ Features

- User registration & login (JWT, BCrypt password hashing)
- Ticket search (by price range)
- Ticket purchase (with seat logic and balance check)
- Personal cabinet (view tickets, cancel booking)
- Admin panel: manage flights, users  
- Dispatcher: change flight status (Approve / Rejected / InProcess)
- Global admin: delete other admins
**Middleware:**
- Buy Ticket(`IticketService`)
- Checking user tickets on count(Cannot buy > 3 tickets)
- Checking user device(Windows,Iphone,Android,Postman)
- 'Throw new' packer in Generic(BaseResponse)
- Checking user Ip and blocking if with one ip gets > 2 queries in three minutes
- Pagination

## 📌 Known imperfections (first version, will be improved)

- Some methods are overloaded with logic
- Part of the code needs refactoring (extract services)
- Not all endpoints are covered by unit tests (planned)
- Some queries can be optimized
  20.04.2026-12.05.2026
