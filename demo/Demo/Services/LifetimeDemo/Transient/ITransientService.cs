namespace Demo.Services.LifetimeDemo.Transient;

public interface ITransientService
{
    Guid InstanceId { get; }
}
