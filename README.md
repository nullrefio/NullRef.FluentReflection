# Fluent Reflection

If you have a class and you want to validate that all Properties are marke with [Required] attribute then creaet a unit test to do so.

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
public void ModelHasAllRequiredTest()
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
