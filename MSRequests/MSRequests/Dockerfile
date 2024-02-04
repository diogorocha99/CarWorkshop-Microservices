FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
ENV ASPNETCORE_ENVIRONMENT = Development
WORKDIR /app


 
#COPY the csproj file and restore any dependecies 
COPY *.csproj ./
RUN dotnet restore
 
# COPY the project files and build our release
COPY . ./
RUN dotnet publish -c Release -o out
 
#Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MSRequests.dll" , "--environment=Development"]