#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TechChallenge.csproj", "."]
RUN dotnet restore "./TechChallenge.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TechChallenge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet tool install -g dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet publish "TechChallenge.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN dotnet ef migrations bundle

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/efbundle .
RUN chmod +x efbundle
ENTRYPOINT ["dotnet", "TechChallenge.dll"]