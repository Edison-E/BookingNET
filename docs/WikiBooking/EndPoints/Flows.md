# EndPoints
## Flows
### Detail Authentication
For the authentication flow, the following endpoints are required:
1. /api/Account/Register
2. /api/Account/Login
3. /api/User/GetProfile

&nbsp;
&nbsp;
## Preview Endpoints
Each endpoint requires a prior action for the flow to be successful:
- **En /api/Account/Login** → You must have previously registered an account to successfully  login.
- **En /api/User/GetProfile** → You must have logged in and obtained a 'access_token' in the login response to access this protected resource. 

&nbsp;
&nbsp;
## Request and Response
The requests and responses of the enpoints are detailed below:

***`📌 /api/Account/Login:`***
**POST /api/Account/Login**
~~~
{
     "email" : "value",
     "password" : "value"
}
~~~
**Response**
~~~
{
    "token": "tokenAccess",
    "success": true,
    "message": [
        "Message",
        "Message"
    ]
}
~~~

***`📌/api/Account/Register:`***
**POST /api/Account/Register**
~~~
{
  "email": "value",
  "name": "value",
  "password": "value",
  "dateBirth": "value",
  "phoneNumber": 000000000
}
~~~
**Response**
~~~
{
    "success": true,
    "message": [
        "Message"
    ]
}
~~~

***`📌/api/Account/GetProfile:`***
**GET /api/Account/GetProfile**
~~~
Header: Authorization: Bearer {access_token}
~~~
**Response**
~~~
{
    "userDTO": {
        "email": "Value",
        "name": "Value"
    },
    "success": true,
    "message": [
        "Message"
    ]
}
~~~