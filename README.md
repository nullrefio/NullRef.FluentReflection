# Fluent Reflection

Reflection can be daunting for many developers. It provides a way to interact with your code in a very "meta" way but is not like most programming.
Reflection can be used to look into the nature of code and add checks based on structure. This allows you to add a sort static analysis.
One way to use reflection is to add code analysis to unit tests and fail a build pipeline based on rule-structure.
Below are some examples of how to maintain code structure for a project using reflection in an easy to understand way.

## Validate Property Attributes

If you have a class and you want to validate that all Properties are marked with `[Required]` attribute then create a unit test to do so.

```csharp
//An example target class
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

This unit test gets all types in all loaded assemblies, filters by a specific type and finds all properties missing some attribute. If there are any results leftover, it generates a message to fail the unit test.

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
## Verify that all controller endpoints have a `[SwaggerOperation]` attribute

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

## Validate that all controller endpoints have an `[HttpMethod]` attribute.

This includes `[HttpGet]`, `[HttpPut]`, `[HttpPost]`, `[HttpDelete]`

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

## Validate that all controller endpoints have parameters decorated with a `[From...]` attribute that defines from where data originates.

```csharp
public class MyController : ControllerBase
{
    public Task Create([FromRoute] string id, [FromBody] CreateWidgetModel model)
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

## Validate objects marked with [ReadOnly] are actuallly read-only

```csharp
[ReadOnly(true)]
public class DocumentModel : IModel
{
    public Guid Id { get; internal set; }
    public DateTime ModifiedDate { get; internal set; }
    public long Size { get; internal set; }
    public string FileName { get; internal set; }
    public string ContentType { get; internal set; }
}
```

Searching for all properties that are publically writtable should result in 0 items. All properties should have `internal` or `private` setters.

```csharp
[Fact]
public void Test()
{
        var matching = GetAllAssemblies()
            .ForAssemblies()
            .ImplementsType<IModel>()
            .Properties()
            .WithAttribute<ReadOnlyAttribute>(x => x.IsReadOnly)
            .IsWritable(true)
            .ToList();

    Assert.Empty(matching);
}
```

## Ensure all string properties are marked with '[MaxLength]' attribute

```csharp
public class WidgetModel : IModel
{
    public Guid Id { get; set; }
    public DateTime ModifiedDate { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string LastName { get; set; }
}
```


```csharp
[Fact]
public void Test()
{
        var matching = GetAllAssemblies()
            .ForAssemblies()
            .ImplementsType<IModel>()
            .Properties()
            .OfType<string>()
            .MissingAttribute<MaxLengthAttribute>()
            .ToList();

    Assert.Empty(matching);
}
```
