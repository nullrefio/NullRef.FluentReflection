using System;
using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection;

public interface IAnalysisValidateResult
{
    List<AnalysisError> Errors { get; }
    string ErrorMessage { get; }
}

public class AnalysisValidateResult : IAnalysisValidateResult
{
    public List<AnalysisError> Errors { get; } = new();

    public string ErrorMessage => string.Join(Environment.NewLine, Errors.Select(x => x.Text));
}
