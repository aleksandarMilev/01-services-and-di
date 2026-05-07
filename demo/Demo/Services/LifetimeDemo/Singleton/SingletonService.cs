namespace Demo.Services.LifetimeDemo.Singleton;

public sealed class SingletonService : ISingletonService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
}
