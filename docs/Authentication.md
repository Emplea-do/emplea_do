# Authentication

ASP.NET Core Identity is a membership system that adds login functionality to ASP.NET Core apps.

## Instructions

Below you will find the instructions, step by step to configure the following external login provider in your development environment.

- [Facebook](#Facebook)
- [Microsoft](#Microsoft)
- [LinkedIn](#LinkedIn)
- [GitHub](#GitHub)
- [Google](#Google)

### Facebook

0. Log In in [Facebook](https://www.facebook.com/) using your account
1. Go to [All Apps - Facebook for Developers](https://developers.facebook.com/apps/)
2. In the upper right corner, select _My Apps_ and then click on _Create App_
3. Fill the fields _Display Name_ and _Contact Email_ then click _Create App ID_
4. You will see in the _Dashboard_ a list of _Products_ that you can add, we're going to configure _Facebook Login_, so, click in the _Set Up_ button for that product
   ![Step 4, Set Up Facebook Login](images/authentication-facebook-1.jpg)
5. In the menu on the left, under _Facebook Login_, click on _Settings_. Under the _Client OAuth Settings_, there is a field called Valid _OAuth Redirect URIs_, where you have to put the url of the project, eg. https://localhost:5001/, and then click on the _Save Changes_ button.
   ![Step 5, Configure Settings for Facebook Login](images/authentication-facebook-2.jpg)
6. In the menu on the left, under _Settings_, click on _Basic_, you will see the Facebook _App ID_ and _App Secret_, necessary to configure the Authentication in the project, copy them, you will need it in next step.
   ![Step 6, copying the App ID and App Secret](images/authentication-facebook-3.jpg)
7. In the _Web_ project go to the file _appsettings.Development.json_ go to the _Facebook_ object, under _Authentication_, paste the _App ID_ and _App Secret_ copied in the previous step.  
   ![Step 7, using the App ID and App Secret](images/authentication-facebook-4.jpg)
8. Run the project and Log In using Facebook.

   - Log In using _Facebook_  
     ![Step 8, Log In using Facebook](images/authentication-facebook-5.jpg)

   - Successful Log In
     ![Step 8, Successful Log In](images/authentication-facebook-6.jpg)

### Microsoft

0. Log In in [Microsoft Azure Portal](https://portal.azure.com/) using your Microsoft account
1. Go to [App registrations](https://portal.azure.com/#blade/Microsoft_AAD_RegisteredApps/ApplicationsListBlade)
2. In the upper left corner, select _New registration_, now we need to fill the basic information about the application:

   - Fill the _Name_ field for the application name, eg.: _Emplea.do_.
   - For _Supported account types_ choose _Accounts in any organizational directory (Any Azure AD directory - Multitenant) and personal Microsoft accounts (e.g. Skype, Xbox)_, this will allow to login anyone with a Microsoft account.
   - For _Redirect URI_ we need to use: https://localhost:5001/signin-microsoft, in the Development Environment this is the local url that the projects use, appending the '_signin-microsoft_' that identifies the login provider, in this case, Microsoft.
   - Click the Register button.

     ![Step 2, Register Application in Microsoft](images/authentication-microsoft-1.jpg)

3. In the left menu, select _Certificates & secrets_, under _Client secrets_:

   - Click the _New client secret_ button.
   - Add a _Description_ for the new _client secrete_, eg.: _ClientSecret_
   - Select when this secret _Expires_, eg.: _In 1 year_

   This will generate a new cliente secret, copy the value we will need it.

4. In the left menu, go to the application _Overview_, there you will see the _Application (client) ID_, copy it, we will need it in the next step.

5. Back in our Emplea.do _Web_ project go to the file _appsettings.Development.json_ to the _Microsoft_ object under _Authentication_ and paste the _App ID_ and _App Secret_ copied in the previous steps.  
   ![Step 7, using the App ID and App Secret](images/authentication-microsoft-2.jpg)

7) Run the project and Log In using Microsoft.

### LinkedIn

### GitHub

### Google

## References

- [Facebook, Google, and external provider authentication in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/index?view=aspnetcore-2.2&tabs=visual-studio)

- [External OAuth authentication providers](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/other-logins?view=aspnetcore-2.2)

- [Facebook external login setup in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-2.2)

- [Microsoft Account external login setup with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-2.2)

- [Authenticating with OAuth 2.0 Overview (LinkedIn)](https://docs.microsoft.com/en-us/linkedin/shared/authentication/authentication?context=linkedin/consumer/context)

- [Authorizing OAuth Apps (GitHub)](https://developer.github.com/apps/building-oauth-apps/authorizing-oauth-apps/)

- [Google external login setup in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-2.2)
