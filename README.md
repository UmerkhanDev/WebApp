# WebApp
make sure your database is connected as it will connect to sqlServer. default at (.).
if you want to change then track appsettings.json in the project theri you can see "DatabaseContextConnectionString" now update "server=.;" like just give your server name  "server=test;" then save it and add migrations than hopefully it will connect to your sql server.

for postman 
here is the request url "https://localhost:44316/api/Payment"
and here is the JSON data that will be in the body
{"CreditCardNumber":"4245124512451245","CardHolder":"umer khan","ExpirationDate":"2030-01-06T17:16:40","SecurityCode":"123","Amount":20.0}