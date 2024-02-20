using System.Reflection;

namespace LibrarySolution.Application;

/// <summary>
/// DI 등록을 위한 Assembly 참조
/// </summary>
public class ApplicationAssembly
{
    internal static readonly Assembly Assembly = typeof(ApplicationAssembly).Assembly;
}
