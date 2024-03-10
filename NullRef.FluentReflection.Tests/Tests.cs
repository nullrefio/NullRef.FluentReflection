using NullRef.FluentReflection.ModelSample1;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

namespace NullRef.FluentReflection.Tests
{
    public class Tests
    {
        private Assembly _assembly = typeof(WidgetModel).Assembly;

        //[Fact]
        //public void MethodReturnTypeTest_Success()
        //{
        //    var result = _assembly
        //        .ForAssembly()
        //        .ForType<WidgetService>()
        //        .Methods()
        //        .Returns<IReturn>();

        //    if (result.Errors.Any())
        //        Assert.Fail("Test failed");
        //}

        //[Fact]
        //public void MethodReturnTypeTest_Fail()
        //{
        //    var result = _assembly
        //        .ForAssembly()
        //        .ForType<FailedService>()
        //        .Methods()
        //        .Returns<IReturn>();

        //    if (!result.Errors.Any())
        //        Assert.Fail("Test failed");
        //}

        //[Fact]
        //public void MethodReturnTaskTypeTest_Success()
        //{
        //    var result = _assembly
        //        .ForAssembly()
        //        .ForType<CollateralService>()
        //        .Methods()
        //        .ReturnsTask<IReturn>();

        //    if (result.Errors.Any())
        //        Assert.Fail("Test failed");
        //}

        [Fact]
        public void MethodMatchingTest()
        {
            Assert.Single(_assembly
               .ForAssembly()
               .ForType<WidgetService>()
               .Methods()
               .Matching());

            Assert.Single(_assembly
               .ForAssembly()
               .ImplementsType<WidgetService>()
               .Methods()
               .Matching());

            Assert.Empty(_assembly
               .ForAssembly()
               .ForType<IWidgetService>()
               .Methods()
               .Matching());

            Assert.Single(_assembly
               .ForAssembly()
               .ImplementsType<IWidgetService>()
               .Methods()
               .Matching());
        }

        //[Fact]
        //public void ModelHasAllRequiredTest()
        //{
        //    var result = _assembly
        //        .ForAssembly()
        //        .ForType<ModelAllRequiredAttributes>()
        //        .Properties()
        //        .WithAttribute<RequiredAttribute>();

        //    Assert.Empty(result.Errors);
        //}

        //[Fact]
        //public void ModelHasNotAllRequiredTest()
        //{
        //    var result = _assembly
        //        .ForAssembly()
        //        .ForType<ModelNotAllRequiredAttributes>()
        //        .Properties()
        //        .WithAttribute<RequiredAttribute>();

        //    Assert.Single(result.Errors);
        //}

        //[Fact]
        //public void ModelAllValueTypeRequiredTest_Success()
        //{
        //    var result = _assembly
        //        .ForAssembly()
        //        .ForType<ModelValueTypeRequiredAttributes>()
        //        .Properties()
        //        .ValueTypes()
        //        .WithAttribute<RequiredAttribute>();

        //    Assert.Empty(result.Errors);
        //}

        //[Fact]
        //public void ModelAllValueTypeRequiredTest_Fail()
        //{
        //    var result = _assembly
        //        .ForAssembly()
        //        .ForType<ModelValueTypeNotRequiredAttributes>()
        //        .Properties()
        //        .ValueTypes()
        //        .WithAttribute<RequiredAttribute>();

        //    Assert.Single(result.Errors);
        //}

        [Fact]
        public void PropertyMatchingTest()
        {
            var result = _assembly
                .ForAssembly()
                .ForType<ModelAllRequiredAttributes>()
                .Properties()
                .Matching();

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

            Assert.Equal(2, result.Matching().Length);
            Assert.Single(result.WithAttribute<NotNullAttribute>().Matching());
        }

        [Fact]
        public void NonAbstractTypesTest()
        {
            var source = _assembly.GetTypes().Where(x => !x.IsAbstract).ToList();

            var result = _assembly
                .ForAssembly()
                .Types()
                .Abstract(false)
                .Matching();

            Assert.Equal(source.Count, result.Length);
        }
    }
}
