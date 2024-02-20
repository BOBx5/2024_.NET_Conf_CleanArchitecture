namespace LibrarySolution.Domain.Aggregates.Users.Enums;

public enum UserStatus
{
    /// <summary>일시정지</summary>
    /// <value>0000 0000 0000 0001 = 1</value>
    Suspended = 0,

    /// <summary>활동</summary>
    /// <value>0000 0000 0000 0010 = 2</value>
    Active = 1 << 0,

    /// <summary>정지</summary>
    /// <value>0000 0000 0000 0100 = 4</value>
    Stop = 1 << 1,
}
