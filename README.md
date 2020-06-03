# Bookstore
Bookstore app made with .NET Core 3.1 MVC and Microsoft SQL Server database.
<img width="1000" alt="Screenshot at Jun 03 11-09-30" src="https://user-images.githubusercontent.com/48388060/83618456-c3f67400-a58a-11ea-9174-f351a4fffbcc.png">

## Functionality

* In the app users can log in, register, edit profile, place orders, make the payment with Stripe and filter their order history. Admin can register users as company users (authorized or non-authorized), lock users, confirm/reject orders and manage everything. Price is calculated based on quantity.
* Authentication is done with ASP.NET Identity
* Users can sign up / log in with Google account
* Repository and Unit of Work patterns are used
* Unit tests are made with NUnit
* jQuery DataTables plug-in is used for displaying data from the database

<img width="1186" alt="book2" src="https://user-images.githubusercontent.com/48388060/83620092-218bc000-a58d-11ea-837e-294e4de8d5c4.png">

## Problems

* Email Sender - can't send confirmation emails due to the issues with the provider
