FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH

WORKDIR /src
# The below allows layer caching for the restore.

# Copy the Razor Pages project and the SharedLib DLL
COPY RazorPageCampaignsWebsite/ ./RazorPageCampaignsWebsite/
COPY shared/ ./RazorPageCampaignsWebsite/libs/

# Set working directory to the web project
WORKDIR /src/RazorPageCampaignsWebsite

# Restore and publish the project
COPY --link /RazorPageCampaignsWebsite/*.csproj .
RUN dotnet restore -a $TARGETARCH

COPY --link /RazorPageCampaignsWebsite/. .
RUN dotnet publish --runtime linux-$TARGETARCH --self-contained false --no-restore -o /app/publish


#############################
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_URLS=http://*:3001
EXPOSE 3001
WORKDIR /app/publish
COPY --link --from=build /app/publish .
USER $APP_UID
COPY --link /.env .
COPY ./manifest.json /manifest.json
ENTRYPOINT ["dotnet", "RazorPageCampaignsWebsite.dll"]