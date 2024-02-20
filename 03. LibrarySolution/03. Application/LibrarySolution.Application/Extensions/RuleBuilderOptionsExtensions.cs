using FluentValidation;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySolution.Application.Extensions;
internal static class RuleBuilderOptionsExtensions
{
    public static IRuleBuilderOptions<T, string?> BookIdValidation<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("도서ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 도서ID 길이가 아닙니다.")
            .Must(id => BookId.TryParse(id, out _))
            .WithMessage("올바른 도서ID 형식이 아닙니다.");
    }

    public static IRuleBuilderOptions<T, string?> UserIdValidation<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("유저ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 유저ID 길이가 아닙니다.")
            .Must(id => UserId.TryParse(id, out _))
            .WithMessage("올바른 유저ID 형식이 아닙니다.");
    }

    public static IRuleBuilderOptions<T, string?> RentIdValidation<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("대여ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 대여ID 길이가 아닙니다.")
            .Must(id => RentId.TryParse(id, out _))
            .WithMessage("올바른 대여ID 형식이 아닙니다.");
    }
}
