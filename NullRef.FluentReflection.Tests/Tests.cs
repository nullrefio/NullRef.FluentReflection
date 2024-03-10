using NullRef.FluentReflection.ModelSample1;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

namespace NullRef.FluentReflection.Tests
{
    public class Tests
    {
        private Assembly _assembly = typeof(WidgetModel).Assembly;

        [Fact]
        public void MethodReturnTypeTest_Success()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<WidgetService>()
                .Methods()
                .Returns<IReturn>()
                .ToList();

            Assert.Single(result);
        }

        [Fact]
        public void MethodReturnTypeTest_Fail()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<FailedService>()
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
                .ForType<CollateralService>()
                .Methods()
                .ReturnsTask()
                .ToList();

            Assert.Single(result);
        }

        [Fact]
        public void MethodReturnTaskOfTypeTest_Success()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<CollateralService>()
                .Methods()
                .ReturnsTaskOf<IdValue>()
                .ToList();

            Assert.Single(result);
        }

        [Fact]
        public void MethodMatchingTest()
        {
            Assert.Single(_assembly
               .ForAssembly()
               .ForType<WidgetService>()
               .Methods()
               .ToList());

            Assert.Single(_assembly
               .ForAssembly()
               .ImplementsType<WidgetService>()
               .Methods()
               .ToList());

            Assert.Empty(_assembly
               .ForAssembly()
               .ForType<IWidgetService>()
               .Methods()
               .ToList());

            Assert.Single(_assembly
               .ForAssembly()
               .ImplementsType<IWidgetService>()
               .Methods()
               .ToList());
        }

        [Fact]
        public void ModelHasAllRequiredTest()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<ModelAllRequiredAttributes>()
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
                .ForType<ModelNotAllRequiredAttributes>()
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
                .ForType<ModelValueTypeRequiredAttributes>()
                .Properties()
                .ValueTypes()
                .WithAttribute<RequiredAttribute>()
                .ToList();

            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void ModelAllValueTypeRequiredTest_Fail()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<ModelValueTypeNotRequiredAttributes>()
                .Properties()
                .ValueTypes()
                .WithAttribute<RequiredAttribute>()
                .ToList();

            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void PropertyMatchingTest()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<ModelAllRequiredAttributes>()
                .Properties()
                .ToList();

            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void MethodParameterTest()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<ObjectServiceWithParamWithAttributes>()
                .Methods()
                .Parameters();

            Assert.Equal(2, result.ToList().Length);
            Assert.Single(result.WithAttribute<NotNullAttribute>().ToList());
        }

        [Fact]
        public void NonAbstractTypesTest()
        {
            var source = _assembly.GetTypes().Where(x => !x.IsAbstract).ToList();

            var result = _assembly
                .ForAssembly()
                .Types()
                .Abstract(false)
                .ToList();

            Assert.Equal(source.Count, result.Length);
        }
    }
}
