# powerplant-coding-challenge

## Prerequisites
dotnet is installed on your machine.

## How to build & deploy the API

Go to the Application folder "PowerPlantChallenge"

Open a CMD and run the following command (replace the _OUTPUTDIRECTORY_ parameter name by the directory you want this to be copied to):
```
dotnet build "PowerPlantChallenge.API.csproj" -c Release -o "_OUTPUTDIRECTORY_"
```

Then go to your _OUTPUTDIRECTORY_ and run the following command:
```
dotnet "PowerPlantChallenge.API.dll"
```