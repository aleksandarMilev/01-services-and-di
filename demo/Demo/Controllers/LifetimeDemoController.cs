namespace Demo.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.LifetimeDemo.Scoped;
using Services.LifetimeDemo.Singleton;
using Services.LifetimeDemo.Transient;

[ApiController]
[Route("api/lifetime-demo")]
public sealed class LifetimeDemoController(
    ITransientService transientService,
    IScopedService scopedService,
    ISingletonService singletonService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var transient = new Lifetime(
            InstanceId: transientService.InstanceId,
            Description: "New instance every time the service is requested. " +
                         "Hit this endpoint again — this GUID will change.");

        var scoped = new Lifetime(
            InstanceId: scopedService.InstanceId,
            Description: "One instance per HTTP request. " +
                         "Hit this endpoint again — this GUID will change. " +
                         "But within this request, every class gets the same instance.");

        var singleton = new Lifetime(
            InstanceId: singletonService.InstanceId,
            Description: "One instance for the entire application lifetime. " +
                         "Hit this endpoint 100 times — this GUID will never change.");

        var response = new Response(
            transient,
            scoped,
            singleton);

        return this.Ok(response);
    }
}

public sealed record Response(
    Lifetime Transient,
    Lifetime Scoped,
    Lifetime Singleton);

public sealed record Lifetime(
    Guid InstanceId,
    string Description);
