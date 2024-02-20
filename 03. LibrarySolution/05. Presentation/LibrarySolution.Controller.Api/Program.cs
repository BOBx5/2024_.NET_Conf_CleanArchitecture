using LibrarySolution.Application;
using LibrarySolution.Controller.Api.Middlewares;
using LibrarySolution.Infrastructure.DateTimeProvider;
using LibrarySolution.Infrastructure.EmailService;
using LibrarySolution.Infrastructure.Persistence;

#region Buidler


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// Application Layer
builder.Services.AddApplication(builder.Configuration);

// Infrastructure Layer
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddDateTimeService(builder.Configuration);
builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

// Swagger ����
builder.Services.AddSwaggerGen(options =>
{
    // Swagger�� ���� �ּ� �ڵ����� (����: https://www.notion.so/desperate-tech-chaser/ASP-NET-Xml-Swagger-e72cf980c8ce42808a13629d51c8e087?pvs=4)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        // Swagger [Try it out] �ȴ����� �ǰ� ����
        option.EnableTryItOutByDefault();
    });
}

app.UseHttpsRedirection();

// �̵���� �߰�
app.UseValidationExceptionMiddleware();
app.UseDomainExceptionMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
