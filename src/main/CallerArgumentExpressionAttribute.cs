// Source - https://stackoverflow.com/a/70034587
// Posted by Matthew Watson, modified by community. See post 'Timeline' for change history
// Retrieved 2026-07-26, License - CC BY-SA 4.0

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}
