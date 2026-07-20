# 08 - Phase 7 - Containerization with Podman / Docker
 
Packaging the app in a container gives **identical behavior across dev, staging, and production**. Podman and Docker are largely interchangeable here (same `Dockerfile` format, similar CLI).
 
### The Dockerfile (multi-stage build)
 
Source: demo `Dockerfile` (targets **.NET 10**).
 
```dockerfile
# Stage 1 — BUILD: full SDK image, only used to compile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /src
 
# Copy only the project file first, then restore — so this layer is cached
# and NuGet restore re-runs only when the .csproj changes, not on every code edit.
COPY *.csproj ./
RUN dotnet restore
 
# Now copy the rest of the source and publish an optimized Release build
COPY . ./
RUN dotnet publish -c Release -o /app/publish
 
# Stage 2 — RUNTIME: slim ASP.NET image, no SDK/compilers shipped
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/publish .
 
EXPOSE 8080
ENTRYPOINT ["dotnet", "PodmanDemoApi.dll"]
```
 
**Why it's built this way:**
 
- **Two stages.** The `sdk` image (large, includes compilers) builds the app; the final image is based on the much smaller **`aspnet` runtime** image. `COPY --from=build-env` pulls *only* the published output forward — the SDK never ships. Result: a smaller image with **less attack surface**.
- **Restore before copy.** Copying `*.csproj` and running `dotnet restore` *before* `COPY . ./` exploits **Docker layer caching** — restore only re-runs when dependencies change, so ordinary code edits rebuild fast.
- **`-c Release`** produces optimized binaries (vs. the default `Debug`).
- **`EXPOSE 8080`** documents the port. Note: since **.NET 8**, the official container images default to listening on **port 8080** (not 80), because the container runs as a non-root user by default and can't bind low ports. Match your host mapping accordingly (`-p 8080:8080`).
- **`ENTRYPOINT`** runs the published DLL via the `dotnet` host.
 
> [!WARNING] `EXPOSE` does not publish the port
> `EXPOSE 8080` is documentation/metadata only — it does **not** make the port reachable from your host. You still must run with `-p 8080:8080` (Docker) / `-p 8080:8080` (Podman) to map it.
 
### The `.dockerignore` file
 
Keeps the **build context** small and images clean by excluding local/IDE artifacts:
 
```gitignore
# Local build output, IDE, and VCS metadata
bin/
obj/
.vs/
.git/
*.user
```
 
> [!TIP] Why `.dockerignore` matters
> Everything in the build context is sent to the build engine and can leak into layers. Excluding `bin/`, `obj/`, `.git/`, and IDE files makes builds **faster** (less to transfer), **smaller**, and **safer** (no stray secrets/history baked into the image). Think of it as `.gitignore` for the Docker build context.
