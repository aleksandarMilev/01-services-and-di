namespace Demo.Services.LifetimeDemo.Scoped;

public sealed class ScopedService : IScopedService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
}
