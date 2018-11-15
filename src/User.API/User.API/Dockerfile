FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /code
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build /code/out ./

EXPOSE 80
ENTRYPOINT [ "dotnet","User.API.dll" ]