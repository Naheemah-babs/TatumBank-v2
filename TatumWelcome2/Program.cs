
using TatumWelcome2.BLL;
using TatumWelcome2.Controllers;
using TatumWelcome2.Interfaces;
using TatumWelcome2.Models;
using TatumWelcome2.Presentation;
using TatumWelcome2.Repositories;
using TatumWelcome2.Services;
using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

CultureInfo.DefaultThreadCurrentCulture =
    CultureInfo.GetCultureInfo("en-NG");
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter(
    "System.Net.Http.HttpClient",
    LogLevel.Warning);

builder.Logging.AddFilter(
    "Microsoft.AspNetCore",
    LogLevel.Warning);

// Bank A API address.
builder.WebHost.UseUrls("http://localhost:5000");

// Shared in-memory account list.
List<Account> accounts = AccountData.GetAccounts();

// Repository.
builder.Services.AddSingleton<IAccountRepository>(
    new AccountRepository(accounts));


// HTTP communication.
builder.Services.AddHttpClient<IBankApiClient, BankApiClient>();

// Business logic services.
builder.Services.AddSingleton<InterbankTransferService>();
builder.Services.AddSingleton<InterbankOutgoingTransferService>();
builder.Services.AddSingleton(new AdminService(accounts));
builder.Services.AddSingleton<BillPaymentService>();

var app = builder.Build();

// Resolve services.
IBankApiClient bankApiClient =
    app.Services.GetRequiredService<IBankApiClient>();

InterbankOutgoingTransferService outgoingTransferService =
    app.Services.GetRequiredService<InterbankOutgoingTransferService>();

AdminService adminService =
    app.Services.GetRequiredService<AdminService>();

BillPaymentService billPaymentService =
    app.Services.GetRequiredService<BillPaymentService>();

// API endpoints.
app.MapGet(
    "/health",
    () => "Tatum Bank API is running");

app.MapAccountEndpoints();
app.MapInterbankTransferEndpoints();
app.MapBillPaymentEndpoints();

// Start API.
var serverTask = app.RunAsync();

try
{
    BankApp.Run(
        accounts,
        bankApiClient,
        outgoingTransferService,
        adminService,
        app.Configuration,
        billPaymentService);
}
finally
{
    await app.StopAsync();
    await serverTask;
}
