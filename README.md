# Fluent Reflection

If you have a class and you want to validate that all Properties are marke with `[Required]` attribute then creaet a unit test to do so.

```csharp
public class ModelAllRequiredAttributes : IRequiredModel
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; }
    [Required]
    public int Age { get; set; }
}
```

This unit test get all types in all loaded assemblies, filters by a specific type and finds all properties missing the attribute. If there are any, it generates a message to fail the unit test.

```csharp
[Fact]
public void Test()
{
    var result = allAssemblies
        .ForAssemblies()
        .ForType<ModelAllRequiredAttributes>()
        .Properties()
        .MissingAttribute<RequiredAttribute>();

    var m = result.PropertyMissingAttributeErrors<RequiredAttribute>().ErrorMessage;
    if (!string.IsNullOrEmpty(m))
        Assert.Fail(m);
}
```

Validate that all controller methods have been decorated with a `[SwaggerOperation]` attribute.

```csharp
[Fact]
public void Test()
{
    var matching = GetAllAssemblies()
        .ForAssemblies()
        .ImplementsType<ControllerBase>()
        .Abstract(false)
        .Methods()
        .MissingAttribute<SwaggerOperationAttribute>()
        .ToList();

    Assert.Empty(matching);
}
```

Validate that all controllers have an `[HttpMethod]` attribute. This includes `[HttpGet]`, `[HttpPut]`, `[HttpPost]`, `[HttpDelete]`

```csharp
[Fact]
public void Test()
{
        var matching = GetAllAssemblies()
            .ForAssemblies()
            .ImplementsType<ControllerBase>()
            .Abstract(false)
            .Methods()
            .MissingAttribute<HttpMethodAttribute>()
            .ToList();

    Assert.Empty(matching);
}
```
Validate that all controller method parameters have been decorated with a `[From]` attribute that defines from where data originates.

```csharp
public class MyController : ControllerBase
{
    public Task Create([FromRoute] string id)
    {
        return Task.CompletedTask;
    }
}
```

```csharp
[Fact]
public void Test()
{
        var matching = GetAllAssemblies()
            .ForAssemblies()
            .ImplementsType<ControllerBase>()
            .Abstract(false)
            .Methods()
            .Parameters()
            .MissingAllAttributes<FromBodyAttribute, FromFormAttribute, FromQueryAttribute, FromRouteAttribute, FromServicesAttribute>()
            .ToList();

    Assert.Empty(matching);
}
```
