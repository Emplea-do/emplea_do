# Emplea.do #

## Getting the project up and running ##

### Install the following software ###

- [.NET SDK](https://www.microsoft.com/net/learn/get-started/)
- [Visual Studio Code](https://code.visualstudio.com)
- [Visual Studio Community](https://visualstudio.microsoft.com/es/vs/)

## Running the app for the first time ##

0. Run the init.sh script, this will install all command line tools
1. Open the solution
2. Restore the nuget packages
3. Run the Web project

## Workflow

0. Review the list of issues and pick the one you like.
1. Check if the issue has been assigned to someone.
2. Ask for the issue to be assigned to you with a comment.
3. Create a feature branch in your repo in the following format **feature/{issue_number}-simple-title**.
4. Work iteratively and commit often.
5. Ask for help when needed. If run into any impediment air it out in slack or comment in your issue,
6. Prepare the Pull Request following [the guidelines](https://github.com/developersdo/emplea_do/blob/development/.github/pull_request_template.md).

### JS libraries and libman ###

This project includes some third party js libraries, they have been included in the repo because they are integral for the correct workings of the theme we picked.
If you need to add another third party library make sure to do it using the [LibMan CLI](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/libman-cli?view=aspnetcore-2.2#installation). 
It gets installed when you run the `init.sh` script.

### Configuring Authentication ###

Here are the special instruction to configure authentication

#### Ignoring changes of the appsettings file ####

To ignore changes made to the `appsettings.Development.json` file run the following

```sh
git update-index --assume-unchanged Web/appsettings.Development.json
```

The `init.sh` does this automatically.

## If you have any questions or just want to hang out ##

- We have a [slack channel](https://empleado-slack.azurewebsites.net)
