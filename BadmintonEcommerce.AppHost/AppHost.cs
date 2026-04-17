var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.BadmintonEcommerce_API>("api");

builder.Build().Run();
