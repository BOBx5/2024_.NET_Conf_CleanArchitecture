using FluentValidation;
using FluentValidation.Results;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.Entities;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySolution.Application.UseCases.Books.Commands;

#region Command
/// <summary>도서 생성 Command</summary>
[TargetValidator<CreateBookCommandValidator>]
public sealed record CreateBookCommand : ICommand<CreateBookCommandResponse>
{
    /// <summary>도서 제목</summary>
    [DefaultValue("제목1")]
    public required string Title { get; init; }

    /// <summary>도서 설명</summary>
    [DefaultValue("설명1")]
    public required string Description { get; init; }

    /// <summary>도서 저자</summary>
    [DefaultValue("저자1")]
    public required string Author { get; init; }

    /// <summary>보유 수량</summary>
    [DefaultValue(1)]
    public int? Quantity { get; init; }
}
#endregion

#region Response
/// <summary>도서 생성 Command의 응답</summary>
public sealed record CreateBookCommandResponse
{
    /// <summary>생성된 도서의 도서ID</summary>
    public required string BookId { get; init; } = null!;
}
#endregion

#region Validator
/// <summary>도서 생성 Command의 Validator</summary>
internal sealed class CreateBookCommandValidator : FluentValidation.AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("도서의 제목은 공백일 수 없습니다.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("도서의 설명은 공백일 수 없습니다.");

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage("도서의 저자는 공백일 수 없습니다.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("도서의 수량은 0 이상이여야 합니다.")
            .When(x => x.Quantity.HasValue);
    }
}
#endregion

#region Handler
/// <summary>도서 생성 Command의 Handler</summary>
internal sealed class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, CreateBookCommandResponse>
{
    #region Constructor
    private readonly ILogger<CreateBookCommandHandler> _logger;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateBookCommandHandler(
        ILogger<CreateBookCommandHandler> logger,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Handle
    public async Task<CreateBookCommandResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = Book.Create(
            title: request.Title!,
            description: request.Description!,
            author: request.Author!,
            quantity: request.Quantity ?? 1);

        await _bookRepository.Add(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new CreateBookCommandResponse
        {
            BookId = book.Id.ToString()
        };
    }
    #endregion
}
#endregion

