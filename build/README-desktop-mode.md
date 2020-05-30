# Client-side
1. Go to `vue-client` directory.
2. Build the application in desktop mode:
```
npm run build -- --mode desktop
```
3. The output files should appear in `dist` directory. They can be served by an HTTP server, e.g.:
```
npx serve dist/ -l 8080
```
You can also install the [serve package](https://www.npmjs.com/package/serve):
```
npm i -g serve
serve dist/ -l 8080
```

# Server-side
1. Go to `server` directory.
2. Build the application:
```
dotnet publish Episememe.Api/Episememe.Api.csproj -o out/
```
3. Go to `out` directory and adjust the configuration file: `appsettings.Desktop.json`. Modify 2 parameters: *ConnectionString* and *RootDirectory*.
4. In order to start the server, run:
    - in bash:
    ```
    ASPNETCORE_ENVIRONMENT=desktop dotnet Episememe.Api.dll
    ```
    - in Powershell:
    ```
    $env:ASPNETCORE_ENVIRONMENT = 'desktop'; dotnet .\Episememe.Api.dll
    ```
Note: the database file and root directory, if not provided, will be created automatically.
