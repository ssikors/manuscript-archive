using ManuscriptApi.Business.Services;
using ManuscriptApi.DapperDAL;
using ManuscriptApi.Presentation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen
(
    c =>
    {
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// builder.Services.AddDbContext<ManuscriptDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICrudService<Country>, CountryService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ICrudService<Location>, LocationService>();

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICrudService<Tag>, TagService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICrudService<User>, UserService>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();    
builder.Services.AddScoped<ICrudService<Image>, ImageService>();

builder.Services.AddScoped<IManuscriptRepository, ManuscriptRepository>();
builder.Services.AddScoped<ICrudService<Manuscript>, ManuscriptService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
