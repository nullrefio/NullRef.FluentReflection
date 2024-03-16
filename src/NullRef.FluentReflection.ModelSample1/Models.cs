using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace NullRef.FluentReflection.ModelSample1;

public interface IModel
{
}

public interface IReturn
{
}

public class IdValue : IReturn
{
    public Guid Id { get; set; }
}

public class WidgetModel : IModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public int Age { get; set; }
}

public interface IWidgetService
{
}

public class WidgetService : IWidgetService
{
    public IdValue Create(string name, int age)
    {
        var r = new WidgetModel { Name = name, Age = age };
        return new IdValue { Id = r.Id };
    }
}

public interface ICollateralService
{
    Task<IdValue> Create(string name, int age);
}

public class CollateralService : ICollateralService
{
    public Task<IdValue> Create(string name, int age)
    {
        var r = new WidgetModel { Name = name, Age = age };
        return Task.FromResult(new IdValue { Id = r.Id });
    }
}

public class FailedService
{
    public Guid Create(string name, int age)
    {
        var r = new WidgetModel { Name = name, Age = age };
        return r.Id;
    }
}

public interface IRequiredModel
{
}

public class ModelAllRequiredAttributes : IRequiredModel
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; }
    [Required]
    public int Age { get; set; }
}

public class ModelNotAllRequiredAttributes : IRequiredModel
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; }
    public int Age { get; set; }
}

public class ModelValueTypeRequiredAttributes : IRequiredModel
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public bool IsRequired { get; set; }
}

public class ModelValueTypeNotRequiredAttributes : IRequiredModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public bool IsRequired { get; set; }
}

public class ObjectServiceWithParamWithAttributes
{
    public IdValue Create([NotNull] string name, int age)
    {
        var r = new WidgetModel { Name = name, Age = age };
        return new IdValue { Id = r.Id };
    }
}