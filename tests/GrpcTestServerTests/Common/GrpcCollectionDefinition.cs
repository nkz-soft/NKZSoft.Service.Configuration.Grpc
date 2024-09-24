namespace GrpcTestServerTests.Common;

[CollectionDefinition(nameof(GrpcCollectionDefinition))]
public sealed class GrpcCollectionDefinition : CoreCollectionDefinition<GrpcWebApplicationFactory<Program>>;
