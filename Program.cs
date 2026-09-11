
using MultipleFileReader.Interface;
using MultipleFileReader.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IFileServices, FileService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore-6-baseline
builder.Services.AddSwaggerGen(c => c.OperationFilter<FileUploadOperationFilter>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
