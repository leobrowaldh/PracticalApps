using Packt.Shared;
using Microsoft.AspNetCore.Server.Kestrel.Core; //HttpProtocols

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddNorthwindContext();
//can recieve HTTP request with compressed body content, rarely needed:
builder.Services.AddRequestDecompression();

//Enabling HTTP/3 support (not yet supported by every browser, not allways enable this.)
builder.WebHost.ConfigureKestrel((context, options) =>
{
	options.ListenAnyIP(5001, listenOptions =>
	{
		listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
		listenOptions.UseHttps(); // HTTP/3 requires secure connections
	});
});

var app = builder.Build();

// configure the HTTP pipeline

//forces all communication to HTTPS, no HTTP allowed, safer.
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.Use(async (HttpContext context, Func<Task> next) =>
{
	RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;

	if (rep is not null)
	{
		WriteLine($"Endpoint name: {rep.DisplayName}");
		WriteLine($"Endpoint route pattern: {rep.RoutePattern.RawText}");
	}

	if (context.Request.Path == "/bonjour")
	{
		// in the case of a match on URL path, this becomes a terminating
		// delegate that returns so does not call the next delegate
		await context.Response.WriteAsync("Bonjour Monde!");
		return;
	}

	// we could modify the request before calling the next delegate
	await next();

	// we could modify the response after calling the next delegate
});


app.UseHttpsRedirection();

app.UseRequestDecompression();

app.UseDefaultFiles(); //to turn index.html into default (or similar names)
app.UseStaticFiles(); //allow static, like index is at the moment (located in wwwroot)

app.MapRazorPages();

app.MapGet("/hello", () => "Hello World!");

app.Run();

WriteLine("This executes after the web server has stopped!");