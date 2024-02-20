using LibrarySolution.Domain.Aggregates.Users.DomainEvents;
using LibrarySolution.Domain.Aggregates.Users.Enums;
using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Users.Entities;

public sealed class User : AuditEntityBase, IAggregateRoot
{
    #region EF Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    #endregion

    #region Const
    /// <summary>기본 등록 유저 상태</summary>
    public const UserStatus DefaultUserStatus = UserStatus.Active;
    #endregion

    #region Property
    /// <summary>유저고유ID</summary>
    public UserId Id { get; private set; }

    /// <summary>성명</summary>
    public string Name { get; private set; }

    /// <summary>상태 (0: 가입대기, 1: 활동, 2: 정지)</summary>
    public UserStatus Status { get; private set; }

    /// <summary>이메일</summary>
    public string Email { get; private set; }
    #endregion

    #region Constructor
    private User(UserId id, string name, UserStatus status, string email)
    {
        Id = id;
        Name = name;
        Status = status;
        Email = email;
    }
    public static User Create(string name, string email)
    {
        var id = UserId.Create();
        var user = new User(id, name, DefaultUserStatus, email);
        user.Raise(new UserCreatedDomainEvent(user.Id, user.Name, user.Email));
        return user;
    }
    #endregion

    #region Domain Logic
    public void ModifyInfo(string? name, string? email)
    {
        if (name == null && name == email)
            throw new InvalidUserInfoException("변경할 유저 데이터가 없습니다.");

        if (name is not null) 
            this.Name = name;
        if (email is not null) 
            this.Email = email;
    }

    public void ChangeStatus(UserStatus status)
    {
        if (status == this.Status)
            throw new InvalidUserStatusException("변경하려는 유저 상태가 현재와 동일합니다.", status);

        this.Status = status;
    }
    #endregion
}
