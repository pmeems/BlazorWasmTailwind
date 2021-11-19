# Blazor Starter Application

This template contains an example .NET 6 [Blazor WebAssembly](https://docs.microsoft.com/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-webassembly) client application, a .NET 6 C# [Azure Functions](https://docs.microsoft.com/azure/azure-functions/functions-overview), and a C# class library with shared code.

## Getting Started

1. Create a repository from the [GitHub template](https://docs.github.com/en/enterprise/2.22/user/github/creating-cloning-and-archiving-repositories/creating-a-repository-from-a-template) and then clone it locally to your machine.

1. In the **Api** folder, copy `local.settings.example.json` to `local.settings.json`

1. Continue using either Visual Studio or Visual Studio Code.

### Visual Studio 2022

Once you clone the project, open the solution in [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) and follow these steps:

1. Press **F5** to launch both the client application and the Functions API app. In Visual Studio, you can right click the solution and select both API project and client project as startup projects. 

    _Note: If you're using the Azure Functions CLI tools, refer to [the documentation](https://docs.microsoft.com/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash) on how to enable CORS._

### Visual Studio Code with Azure Static Web Apps CLI

1. Install the [Azure Static Web Apps CLI](https://www.npmjs.com/package/@azure/static-web-apps-cli) and [Azure Functions Core Tools CLI](https://www.npmjs.com/package/azure-functions-core-tools).

1. Open the folder in Visual Studio Code.

1. In the VS Code terminal, run the following command to start the Static Web Apps CLI, along with the Blazor WebAssembly client application and the Functions API app:

    ```bash
    swa start http://localhost:5000 --run "dotnet run --project Client/Client.csproj" --api-location Api
    ```

    The Static Web Apps CLI (`swa`) first starts the Blazor WebAssembly client application and connects to it at port 5000, and then starts the Functions API app.

1. Open a browser and navigate to the Static Web Apps CLI's address at `http://localhost:4280`. You'll be able to access both the client application and the Functions API app in this single address. When you navigate to the "Fetch Data" page, you'll see the data returned by the Functions API app.

1. Enter Ctrl-C to stop the Static Web Apps CLI.

## Template Structure

- **Client**: The Blazor WebAssembly sample application
- **Api**: A C# Azure Functions API, which the Blazor application will call
- **Shared**: A C# class library with a shared data model between the Blazor and Functions application
- **ApiIsolated**: A C# Azure Functions API using the .NET isolated execution model, which the Blazor application will call. This version can be used instead of the in-process function app in `Api`.

## Deploy to Azure Static Web Apps

This application can be deployed to [Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps), to learn how, check out [our quickstart guide](https://aka.ms/blazor-swa/quickstart).

# Speed Improvements

I started with the template mentioned above and published it to Azure. According to [GTmetrix](https://gtmetrix.com/) loading that site took 4.7 seconds which is way too slow.
With this project I'm trying to improve the loading time also known as the Speed Index. A good user experience for loading time of your site is 1.3s or less.

## Changes to csproj files

First I made some changes in the csproj files and added this block:
```xml
	<PropertyGroup Condition=" '$(Configuration)'!='Debug' ">
		<DebuggerSupport>false</DebuggerSupport>
		<DebugType>none</DebugType>
	</PropertyGroup>
	<PropertyGroup>
		<PublishTrimmed>true</PublishTrimmed>
		<EnableTrimAnalyzer>true</EnableTrimAnalyzer>
		<BlazorEnableTimeZoneSupport>false</BlazorEnableTimeZoneSupport>
		<EventSourceSupport>false</EventSourceSupport>
		<InvariantGlobalization>true</InvariantGlobalization>
	</PropertyGroup>
```
This change already results in 1 second less waiting time.



## Replace bootstrap.css with TailwindCss and minifying

I like [Tailwind CSS](https://tailwindcss.com/) better than Bootstrap CSS and I find it easier to customize Tailwind CSS using [PostCSS](https://postcss.org/).

For PostCSS you need NodeJS and npm. Look at the setup page of PostCSS how to set this up.

When you have npm installed, go to the Client folder of this project and run `npm install` in the Developer PowerShell of VS2022. It will look at the `package.json` and install all needed packages.

Next, another change in the csproj file is added:
```xml
	<Target Name="PostBuild" AfterTargets="PostBuildEvent">
		<Exec Command="npm run buildcss" />
	</Target>
```
This tells dotnet to run `buildcss` after building this project.

This command is in `pacakge.json` and looks like this:
```bash
"postcss wwwroot/css/app.css -o wwwroot/css/app.min.css && postcss obj/Debug/net6.0/scopedcss/bundle/Client.styles.css -o wwwroot/css/Client.min.css"
```
It tells to minify `app.css` to `app.min.css`. This is also changed in index.html.

I also tried to minify `Client.styles.css` which is the bundled CSS file from the isolated CSS files you can use with Blazor.

I don't like that I need to refer to the file in the `obj` folder, but for now I don't know of an alternative.

The Tailwind CLI handles the purging of the not-used Tailwind CSS, resulting in a very small app.min.css file.

I haven't yet removed all the bootstrap css classes from the razor pages and the isolated css file. When that is done (I leave that for you) Client.min.css will be even more smaller.

All these changes results in a `Largest Contentful Paint` of 2.8s and a `Total Blocking Time` of 1.5s.

This is still not very fast for such a minimal web app, but now the duration is mostly in downloading `/_framework/System.Private.CoreLib.dll`, `/_framework/dotnet.wasm` and other framework related dlls.

## Start simplified

I created `swa-cli.config.json` which contains the start-up code.

Now you can start the application by just typing `swa start app`

## Live demo
For now a live demo is available here: [purple-meadow-079619e03.azurestaticapps.net](https://purple-meadow-079619e03.azurestaticapps.net/)



