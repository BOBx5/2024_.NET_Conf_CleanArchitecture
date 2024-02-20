using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Books.Commands;

#region Command
[TargetValidator(typeof(ModifyBookInfoCommandValidator))]
public sealed record ModifyBookInfoCommand : ICommand<ModifyBookInfoCommandResponse>
{
    [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
    public required string BookId { get; init; }

    [DefaultValue("제목1")]
    public string? Title { get; init; }

    [DefaultValue("설명1")]
    public string? Description { get; init; }

    [DefaultValue("저자1")]
    public string? Author { get; init; }
}
#endregion

#region Response
public sealed record ModifyBookInfoCommandResponse
{
    public required string BookId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Author { get; init; }
}
#endregion

#region Validator
internal sealed class ModifyBookInfoCommandValidator : AbstractValidator<ModifyBookInfoCommand>
{
    public ModifyBookInfoCommandValidator()
    {
        RuleFor(x => x.BookId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("도서ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 도서ID 길이가 아닙니다.")
            .Must(id => BookId.TryParse(id, out _))
            .WithMessage("올바른 도서ID 형식이 아닙니다.");

        RuleFor(x => new { x.Title, x.Description, x.Author })
            .Must(x => x.Title != null || x.Description != null || x.Author != null)
            .WithMessage($"수정할 정보가 없습니다.")
            .ChildRules(x =>
            {
                x.RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage($"도서제목은 공백일 수 없습니다.")
                .MaximumLength(50)
                .WithMessage($"도서제목은 50자를 넘을 수 없습니다.")
                .When(x => x.Title != null);

                x.RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage($"도서설명은 공백일 수 없습니다.")
                .MaximumLength(50)
                .WithMessage($"도서설명은 50자를 넘을 수 없습니다.")
                .When(x => x.Description != null);

                x.RuleFor(x => x.Author)
                .NotEmpty()
                .WithMessage($"도서저자는 공백일 수 없습니다.")
                .MaximumLength(50)
                .WithMessage($"도서저자는 50자를 넘을 수 없습니다.")
                .When(x => x.Author != null);
            });
    }
}
#endregion

#region Handler
internal sealed class ModifyBookInfoCommandHandler : ICommandHandler<ModifyBookInfoCommand, ModifyBookInfoCommandResponse>
{
    #region Constructor
    private readonly ILogger<ModifyBookInfoCommandHandler> _logger;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ModifyBookInfoCommandHandler(
        ILogger<ModifyBookInfoCommandHandler> logger,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Handle
    public async Task<ModifyBookInfoCommandResponse> Handle(ModifyBookInfoCommand request, CancellationToken cancellationToken)
    {
        var bookId = BookId.Parse(request.BookId);
        var book = await _bookRepository.GetBook(bookId, cancellationToken) 
            ?? throw new BookNotFoundException(bookId);
        book.ModifyInfo(
            title: request.Title ?? book.Title,
            description: request.Description ?? book.Description,
            author: request.Author ?? book.Author);

        await _bookRepository.Update(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ModifyBookInfoCommandResponse
        {
            BookId = book.Id.ToString(),
            Title = book.Title,
            Description = book.Description,
            Author = book.Author
        };
    }
    #endregion
}
#endregion