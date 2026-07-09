---
tags: [api, static-files, web, html]
---
# Static Files and wwwroot

By default, an ASP.NET Core Web API does not serve static files (like HTML, CSS, JavaScript, or images). To host a frontend directly from your Web API project, you must explicitly enable it.

## 1. Create the `wwwroot` Folder
Create a folder named exactly `wwwroot` in the root of your project (alongside `Program.cs` and `appsettings.json`). This is the default root folder for serving static content in ASP.NET Core.

Place your `index.html` or `course-gallery.html` inside this folder.

## 2. Update `Program.cs`
You must add the middleware to serve default files (like `index.html`) and static files from the `wwwroot` directory.

```csharp
var app = builder.Build();

// Enable serving default files (e.g., index.html)
app.UseDefaultFiles();

// Enable serving static files from the wwwroot folder
app.UseStaticFiles();

app.MapControllers();
app.Run();
```

**Order matters!** `UseDefaultFiles()` must be called *before* `UseStaticFiles()` to work properly.

## 3. Accessing the Files
If you placed `course-gallery.html` in `wwwroot`, it can be accessed directly via the browser at:
`http://localhost:<port>/course-gallery.html`
