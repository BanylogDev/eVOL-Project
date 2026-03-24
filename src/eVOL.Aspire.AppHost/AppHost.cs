var builder = DistributedApplication.CreateBuilder(args);

// Parameters / secrets
var postgresUser = builder.AddParameter("postgres-user", "evol_user");
var postgresPassword = builder.AddParameter("postgres-password", secret: true);

var mongoUser = builder.AddParameter("mongo-user", "evol_user");
var mongoPassword = builder.AddParameter("mongo-password", secret: true);

var seqPassword = builder.AddParameter("seq-password", secret: true);

var rabbitUser = builder.AddParameter("rabbit-user", "guest");
var rabbitPassword = builder.AddParameter("rabbit-password", secret: true);

// Infrastructure
var postgres = builder.AddPostgres("postgres", postgresUser, postgresPassword, port: 5433);
var defaultConnection = postgres.AddDatabase("DefaultConnection", "evol_db");

var mongo = builder.AddMongoDB("MongoDB", port: 27018, userName: mongoUser, password: mongoPassword);

var redis = builder.AddRedis("RedisConnection", port: 6379);

var seq = builder.AddSeq("seq", seqPassword, port: 5341);

var rabbit = builder.AddRabbitMQ("rabbitmq", rabbitUser, rabbitPassword, port: 5672)
    .WithManagementPlugin(15672);

// API
builder.AddProject<Projects.eVOL_API>("evol-api")
    .WithReference(defaultConnection)
    .WithReference(mongo)
    .WithReference(redis)
    .WithReference(seq)
    .WithReference(rabbit)
    .WithReplicas(3); // enable this again after one instance starts cleanly

builder.Build().Run();