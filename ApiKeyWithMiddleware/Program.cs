using TestMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseMiddleware<ApiKeyMiddleware>(); //Use all api
app.UseWhen(context => context.Request.Path.StartsWithSegments("/WeatherForecast"), builder => //Use specific controller
{
    builder.UseMiddleware<ApiKeyMiddleware>();
});

//Example use some endpoints in different controllers
// app.UseWhen(context =>
// {
//     var endpoint = context.GetEndpoint();
//     return endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName == "ControllerA"
//            && endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName == "SeuEndpointA";
// }, builder =>
// {
//     builder.UseMiddleware<SeuMiddleware>();
// });
//
// app.UseWhen(context =>
// {
//     var endpoint = context.GetEndpoint();
//     return endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName == "ControllerB"
//            && endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName == "SeuEndpointB";
// }, builder =>
// {
//     builder.UseMiddleware<SeuOutroMiddleware>();
// });
app.UseAuthorization();

app.MapControllers();

app.Run();