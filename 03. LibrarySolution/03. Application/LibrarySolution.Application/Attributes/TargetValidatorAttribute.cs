namespace LibrarySolution.Application.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class TargetValidatorAttribute : Attribute
{
    public Type TargetValidatorType { get; }
    public bool Ignore { get; }
    public TargetValidatorAttribute(Type targetValidatorType, bool ignore = false)
    {
        TargetValidatorType = targetValidatorType;
        Ignore = ignore;
    }
}

internal class TargetValidatorAttribute<TValidator> : TargetValidatorAttribute
{
    public TargetValidatorAttribute(bool ignore = false)
        : base(typeof(TValidator), ignore)
    {
    }
}
