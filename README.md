# Movie web task for JE.

In order to run the project and get successful requests, you need to:

1. Add your omdb api key to your user secrets, navigate to MovieWebAppJE\MovieWebApp -> `dotnet user-secrets init`
and then -> `dotnet user-secrets set "OmdbApiKey" "YOUR_API_KEY_HERE"`

2. Open the MovieWebApp.sln file and click the pretty green debug button with "https" next to it
#####  	OR
3. Navigate in your terminal to "MovieWebAppJE\MovieWebApp>" and run -> `dotnet build` followed by `dotnet run`
Open localhost to the port that was shown as "listening at " shortly after running `dotnet run`

###### If something doesnt work, maybe some port values from `Program.cs` or `launchSettings.json` need to be changed.
