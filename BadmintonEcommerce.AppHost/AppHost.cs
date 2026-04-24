var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sql-server");

var badmintonShopDb = sqlServer.AddDatabase("BadmintonShopDb");
var api = builder.AddProject<Projects.BadmintonEcommerce_API>("api")
    .WithReference(badmintonShopDb)
    .WaitFor(badmintonShopDb);

builder.Build().Run();
