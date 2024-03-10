using System;
using System.Collections.Generic;
using System.Linq;

namespace NullRef.FluentReflection
{
    public class AnalysisValidateResult
    {
        public List<AnalysisError> Errors { get; internal set; } = new List<AnalysisError>();

        public string ErrorMessage => string.Join(Environment.NewLine, Errors.Select(x => x.Text));
    }
}
