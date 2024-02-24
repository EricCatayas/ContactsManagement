using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Services.ContactsManager;
using ContactsManagement.Core.Domain.IdentityEntities;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager;
using ContactsManagement.Web.Filters.ExceptionFilters;
using ContactsManagement.Web.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using ContactsManagement.Core.Services.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Infrastructure.Repositories.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;
using ContactsManagement.Core.Services.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactTags;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.Persons;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactTagsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactTags;
using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactLogsServices;
using ContactsManagement.Core.Services.ContactsManager.ContactLogs;
using ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactLogs;
using ContactsManagement.Web.ViewComponents;
using ContactsManagement.Core.Domain.RepositoryContracts.EventsManager;
using ContactsManagement.Infrastructure.Repositories.EventsManager;
using ContactsManagement.Core.ServiceContracts.EventsManager;
using ContactsManagement.Core.Services.EventsManager;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.Services.AccountManager;
using ContactsManagement.Core.Services.ContactsManager.Persons;
using ContactsManagement.Core.ServiceContracts.EmailServices;
using ContactsManagement.Core.Services.EmailServices;
using ContactsManagement.Core.ServiceContracts.Others.V2;
using IImageUploaderService = MediaStorageServices.Interfaces.v2.IImageUploaderService;
using ImageUploaderService = MediaStorageServices.Services.AzureStorageContainer.v2.ImageUploaderService;
using ImageResizeAndUploadService = ContactsManagement.Core.Services.Others.v2.ImageResizeAndUploadService;
using MediaStorageServices.Interfaces.v1;
using MediaStorageServices.Services.AzureStorageContainer.v1;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext builderContext, IServiceProvider serviceProvider, LoggerConfiguration config) =>
{
    config.ReadFrom.Configuration(builderContext.Configuration) // <- Reading the config of program.cs 
          .ReadFrom.Services(serviceProvider);
});

// TODO

// IOC CONTAINER

builder.Services.AddScoped<IPersonsAdderRepository, PersonsAdderRepository>();
builder.Services.AddScoped<IPersonsGetterRepository, PersonsGetterRepository>();
builder.Services.AddScoped<IPersonsUpdaterRepository, PersonsUpdaterRepository>();
builder.Services.AddScoped<IPersonsDeleterRepository, PersonsDeleterRepository>();

builder.Services.AddScoped<ICompaniesAdderRepository, CompaniesAdderRepository>();
builder.Services.AddScoped<ICompaniesGetterRepository, CompaniesGetterRepository>();
builder.Services.AddScoped<ICompaniesUpdaterRepository, CompaniesUpdaterRepository>();
builder.Services.AddScoped<ICompaniesDeleterRepository, CompaniesDeleterRepository>();

builder.Services.AddScoped<IContactTagsAdderRepository, ContactTagsAdderRepository>();
builder.Services.AddScoped<IContactTagsGetterRepository, ContactTagsGetterRepository>();
builder.Services.AddScoped<IContactTagsUpdaterRepository, ContactTagsUpdaterRepository>();
builder.Services.AddScoped<IContactTagsDeleterRepository, ContactTagsDeleterRepository>();

builder.Services.AddScoped<IContactGroupsAdderRepository, ContactGroupsAdderRepository>();
builder.Services.AddScoped<IContactGroupsGetterRepository, ContactGroupsGetterRepository>();
builder.Services.AddScoped<IContactGroupsUpdaterRepository, ContactGroupsUpdaterRepository>();
builder.Services.AddScoped<IContactGroupsDeleterRepository, ContactGroupsDeleterRepository>();

builder.Services.AddScoped<IContactLogsAdderRepository, ContactLogsAdderRepository>();
builder.Services.AddScoped<IContactLogsGetterRepository, ContactLogsGetterRepository>();
builder.Services.AddScoped<IContactLogsUpdaterRepository, ContactLogsUpdaterRepository>();
builder.Services.AddScoped<IContactLogsDeleterRepository, ContactLogsDeleterRepository>();

builder.Services.AddScoped<IEventsAdderRepository, EventsAdderRepository>();
builder.Services.AddScoped<IEventsGetterRepository, EventsGetterRepository>();
builder.Services.AddScoped<IEventsUpdaterRepository, EventsUpdaterRepository>();
builder.Services.AddScoped<IEventsDeleterRepository, EventsDeleterRepository>();
builder.Services.AddScoped<IEventsStatusUpdaterRepository, EventsStatusUpdaterRepository>();

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

builder.Services.AddTransient<IPersonsAdderService, PersonsAdderService>();
builder.Services.AddTransient<IPersonsGetterService, PersonsGetterServiceForDemo>();
builder.Services.AddTransient<IPersonsSorterService, PersonsSorterService>();
builder.Services.AddTransient<IPersonsUpdaterService, PersonsUpdaterService>();
builder.Services.AddTransient<IPersonsDeleterService, PersonsDeleterService>();
builder.Services.AddTransient<IPersonsGroupIdFilteredGetterService, PersonsGroupIdFilteredGetterServiceForDemo>();

builder.Services.AddTransient<ICompanyAdderService, CompanyAdderService>();
builder.Services.AddTransient<ICompanyAdderByNameService, CompanyAdderByNameService>();
builder.Services.AddTransient<ICompanyGetterService, CompanyGetterServiceForDemo>();
builder.Services.AddTransient<ICompanyDeleterService, CompanyDeleterService>();
builder.Services.AddTransient<ICompanyUpdaterService, CompanyUpdaterService>();

builder.Services.AddTransient<IContactTagsAdderService, ContactTagsAdderService>();
builder.Services.AddTransient<IContactTagsGetterService, ContactTagsGetterServiceForDemo>();
builder.Services.AddTransient<IContactTagsUpdaterService, ContactTagsUpdaterService>();
builder.Services.AddTransient<IContactTagsDeleterService, ContactTagsDeleterService>();
builder.Services.AddTransient<IContactTagsSeederService, ContactTagsSeederService>();

builder.Services.AddTransient<IContactGroupsAdderService, ContactGroupsAdderService>();
builder.Services.AddTransient<IContactGroupsGetterService, ContactGroupsGetterServiceForDemo>();
builder.Services.AddTransient<IContactGroupsDeleterService, ContactGroupsDeleterService>();
builder.Services.AddTransient<IContactGroupsUpdaterService, ContactGroupsUpdaterService>();
builder.Services.AddTransient<IContactGroupsSeederService, ContactGroupsSeederService>();

builder.Services.AddTransient<IContactLogsAdderService, ContactLogsAdderService>();
builder.Services.AddTransient<IContactLogsGetterService, ContactLogsGetterServiceForDemo>();
builder.Services.AddTransient<IContactLogsDeleterService, ContactLogsDeleterService>();
builder.Services.AddTransient<IContactLogsUpdaterService, ContactLogsUpdaterService>();
builder.Services.AddTransient<IContactLogsSorterService, ContactLogsSorterService>();

builder.Services.AddTransient<IEventsAdderService, EventsAdderService>();
builder.Services.AddTransient<IEventsGetterService, EventsGetterServiceForDemo>();
builder.Services.AddTransient<IEventsUpdaterService, EventsUpdaterService>();
builder.Services.AddTransient<IEventsDeleterService, EventsDeleterService>();
builder.Services.AddTransient<IEventsSeederService, EventsSeederService>();

builder.Services.AddTransient<IImageDeleterService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    string storageAccConnectionString = config["StorageAccountConnectionString"].ToString();
    string blobContainerName = config["BlobContainerName"].ToString();
    return new ImageDeleterService(storageAccConnectionString, blobContainerName);
});
builder.Services.AddTransient<IImageUploaderService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    string storageAccConnectionString = config["StorageAccountConnectionString"].ToString();
    string blobContainerName = config["BlobContainerName"].ToString();
    return new ImageUploaderService(storageAccConnectionString, blobContainerName);
});
builder.Services.AddTransient<IImageUploadService, ImageResizeAndUploadService>();

builder.Services.AddTransient<ICountriesService, CountriesService>();
builder.Services.AddTransient<IContactGroupsGetterService, ContactGroupsGetterServiceForDemo>();
builder.Services.AddTransient<IEmailService,UserEmailService>();


builder.Services.AddScoped<IDemoUserService, DemoUserAccountService>();
builder.Services.AddScoped<ISignedInUserService, SignedInUserService>();

// builder.Services.AddTransient<RedirectToIndexExceptionFilter>();  <-- ServiceFilter
builder.Services.AddTransient<ContactLogsCardViewComponent>();

// DB CONTEXT
var connectionString = builder.Configuration["AzureSQLDatabase"].ToString();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    // options.EnableSensitiveDataLogging(); // The seed entity for entity type 'Country' cannot be added because another seed entity with the same key value for {'CountryId'} has already been added.
});

// LOGGING 
builder.Services.AddLogging(builder =>
{
    builder.AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug); // Enable authentication logging
    builder.AddFilter("Microsoft.AspNetCore.Authorization", LogLevel.Debug); // Enable authorization logging
});
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestBody | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders;
});
/* Custom Config & Options Pattern */

// IDENTITY
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = false;
})   // Table for User and Role
    .AddEntityFrameworkStores<ApplicationDbContext>()              // In this DbContext
    .AddDefaultTokenProviders()   // Tokens generated at run time ex: resetting account info -> sends OTP confirmation
                                  //This is the Repository Layer
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()  // 'Persistence Store' for specified user and role
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();   // And ApplicationRole as the model for RoleStore

builder.Services.AddAuthorization(options =>
{
    /*Authorization Policy enforces the requests to contain the identity cookie. If no identity cookie is sent, then the user is not logged in
         In order to access any action method, ensure user is authenticated --except anonimous attribute*/
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});
builder.Services.ConfigureApplicationCookie(options =>
{
   /* So unauthenticated user will be redirected here*/
   options.LoginPath = "/Account/Login"; 
});


builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor(); 
var app = builder.Build();


app.UseSerilogRequestLogging("Message Template: Maggot Scientist!"); // <-- adds log message as soon as request response is complete

if (builder.Environment.IsEnvironment("Test") == false)  // Path unavailable in UnitTest
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa"); 
}
// Recommended Sequence
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else // Configure the HTTP request pipeline.
{
    app.UseHsts();
    app.UseExceptionHandler("/Error");     // Built-In Middleware
    app.UseExceptionHandlingMiddleware();  // Custom 
}
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();  
app.UseAuthentication();               // <-- The SignIn Cookie will be supplied auto to the http Req whenever you make another request
app.UseAuthorization();
app.MapControllers();

app.Run();
public partial class Program
{
   // Located on StartUp.cs, to be accessible in the xUnitTesting
   //     => Edit Project File, C: <ItemGroup<InternalsVisibleTo=...
}