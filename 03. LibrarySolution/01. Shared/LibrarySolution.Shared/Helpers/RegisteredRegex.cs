using System.Text.RegularExpressions;

namespace LibrarySolution.Shared.Helpers;

public static class RegisteredRegex
{
    /// <summary>이메일 형식 검사 정규식</summary>
    public static readonly Regex Email = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
}
