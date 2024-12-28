using NullRef.FluentReflection.ModelSample1;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

namespace NullRef.FluentReflection.Tests;

public class Tests
{
    private Assembly _assembly = typeof(WidgetModel).Assembly;

    [Fact]
    public void MethodReturnTypeTest_Success()
    {
        var result = _assembly
            .ForAssembly()
            .Is<WidgetService>()
            .Methods()
            .Returns<IReturn>()
            .ToArray();

        Assert.Single(result);
    }

    [Fact]
    public void MethodReturnTypeTest_Fail()
    {
        var result = _assembly
            .ForAssembly()
            .Is<FailedService>()
            .Methods()
            .Returns<IReturn>();

        var m = result.DoesNotReturnErrors<IReturn>().ErrorMessage;
        if (!string.IsNullOrEmpty(m))
            Assert.Fail(m);
    }

    [Fact]
    public void MethodReturnTaskTypeTest_Success()
    {
        var result = _assembly
            .ForAssembly()
            .Is<CollateralService>()
            .Methods()
            .ReturnsTask()
            .ToArray();

        Assert.Single(result);
    }

    [Fact]
    public void MethodReturnTaskOfTypeTest_Success()
    {
        var result = _assembly
            .ForAssembly()
            .Is<CollateralService>()
            .Methods()
            .ReturnsTaskOf<IdValue>()
            .ToArray();

        Assert.Single(result);
    }

    [Fact]
    public void MethodMatchingTest()
    {
        Assert.Single(_assembly
           .ForAssembly()
           .Is<WidgetService>()
           .Methods()
           .ToArray());

        Assert.Single(_assembly
           .ForAssembly()
           .Implements<WidgetService>()
           .Methods()
           .ToArray());

        Assert.Empty(_assembly
           .ForAssembly()
           .Is<IWidgetService>()
           .Methods()
           .ToArray());

        Assert.Single(_assembly
           .ForAssembly()
           .Implements<IWidgetService>()
           .Methods()
           .ToArray());
    }

    [Fact]
    public void ModelHasAllRequiredTest()
    {
        var result = _assembly
            .ForAssembly()
            .Is<ModelAllRequiredAttributes>()
            .Properties()
            .MissingAttribute<RequiredAttribute>();

        var m = result.PropertyMissingAttributeErrors<RequiredAttribute>().ErrorMessage;
        if (!string.IsNullOrEmpty(m))
            Assert.Fail(m);
    }

    [Fact]
    public void ModelHasNotAllRequiredTest()
    {
        var result = _assembly
            .ForAssembly()
            .Is<ModelNotAllRequiredAttributes>()
            .Properties()
            .MissingAttribute<RequiredAttribute>();

        var m = result.PropertyMissingAttributeErrors<RequiredAttribute>();
        Assert.Single(m.Errors);
    }

    [Fact]
    public void ModelAllValueTypeRequiredTest_Success()
    {
        var result = _assembly
            .ForAssembly()
            .Is<ModelValueTypeRequiredAttributes>()
            .Properties()
            .ValueTypes()
            .WithAttribute<RequiredAttribute>()
            .ToArray();

        Assert.Equal(3, result.Length);
    }

    [Fact]
    public void ModelAllValueTypeRequiredTest_Fail()
    {
        var result = _assembly
            .ForAssembly()
            .Is<ModelValueTypeNotRequiredAttributes>()
            .Properties()
            .ValueTypes()
            .WithAttribute<RequiredAttribute>()
            .ToArray();

        Assert.Equal(2, result.Length);
    }

    [Fact]
    public void PropertyMatchingTest()
    {
        var result = _assembly
            .ForAssembly()
            .Is<ModelAllRequiredAttributes>()
            .Properties()
            .ToArray();

        Assert.Equal(3, result.Length);
    }

    [Fact]
    public void MethodParameterTest()
    {
        var result = _assembly
            .ForAssembly()
            .Is<ObjectServiceWithParamWithAttributes>()
            .Methods()
            .Parameters();

        Assert.Equal(2, result.ToArray().Length);
        Assert.Single(result.WithAttribute<NotNullAttribute>().ToArray());
    }

    [Fact]
    public void NonAbstractTypesTest()
    {
        var source = _assembly.GetTypes().Where(x => !x.IsAbstract).ToList();

        var result = _assembly
            .ForAssembly()
            .Types()
            .IsAbstract(false)
            .ToArray();

        Assert.Equal(source.Count, result.Length);
    }
}
