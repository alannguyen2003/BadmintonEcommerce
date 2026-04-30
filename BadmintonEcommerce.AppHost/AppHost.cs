var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sql-server");

var badmintonShopDb = sqlServer.AddDatabase("BadmintonShopDb");
var api = builder.AddProject<Projects.BadmintonEcommerce_API>("api")
    .WithReference(badmintonShopDb)
    .WaitFor(badmintonShopDb);

var blazorApplication = builder.AddProject<Projects.BadmintonEcommerce_BlazorApplication>("web")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
