namespace LibrarySolution.Shared.Extensions;

public static class GenericTypeExtensions
{

    private static Dictionary<Type, string> _cachedTypeNames = new Dictionary<Type, string>();

    /// <inheritdoc cref="GetGenericTypeName"/>
    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetCachedGenericTypeName();
    }

    /// <inheritdoc cref="GetGenericTypeName"/>/>
    private static string GetCachedGenericTypeName(this Type type)
    {
        if (_cachedTypeNames.TryGetValue(type, out string? typeName))
            return typeName;

        typeName = GetGenericTypeName(type);
        _cachedTypeNames[type] = typeName;
        return typeName;
    }

    /// <summary>
    /// 일반 Type의 경우 <see cref="Type"/>.Name 을 반환하고,<br/>
    /// GenericType인 경우 <u><i>TypeName`T1,T2,T3`</i></u> 형태를 
    /// <u><i>TypeName&lt;T1,T2,T3&gt;</i></u> 형태로 보기 좋게 변환해줍니다.
    /// </summary>
    /// <remarks>
    /// GetBookQuery&lt;Book&gt;<br/>
    /// 기본 Name: <u><i>GetBookQuery`Book`</i></u><br/>  
    /// 변경 Name: <u><i>GetBookQuery&lt;Book&gt;</i></u>
    /// </remarks>
    private static string GetGenericTypeName(this Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());

        return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
    }
}
