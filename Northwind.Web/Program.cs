var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//forces all communication to HTTPS, no HTTP allowed, safer.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles(); //to turn index.html into default (or similar names)
app.UseStaticFiles(); //allow static, like index is at the moment

app.MapGet("/hello", () => "Hello World!");

app.Run();

WriteLine("This executes after the web server has stopped!");