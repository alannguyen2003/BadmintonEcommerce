var builder = DistributedApplication.CreateBuilder(args);

var minio = builder.AddMinIO("minio")
    .WithEnvironment("MINIO_ROOT_USER", "admin")
    .WithEnvironment("MINIO_ROOT_PASSWORD", "password");
var api = builder.AddProject<Projects.BadmintonEcommerce_API>("api");

builder.Build().Run();
