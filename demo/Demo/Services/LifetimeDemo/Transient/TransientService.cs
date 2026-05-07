namespace Demo.Services.LifetimeDemo.Transient;

public sealed class TransientService : ITransientService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
}
