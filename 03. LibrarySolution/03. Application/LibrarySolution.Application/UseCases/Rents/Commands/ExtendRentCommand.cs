using FluentValidation;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Interfaces;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Domain.Aggregates.Rents.Exceptions;
using LibrarySolution.Domain.Aggregates.Rents.Repositories;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Shared.Helpers;
using Microsoft.Extensions.Logging;
using LibrarySolution.Application.Extensions;

namespace LibrarySolution.Application.UseCases.Rents.Commands;

#region Command
[TargetValidator(typeof(ExtendRentCommandValidator))]
public sealed record ExtendRentCommand : ICommand<ExtendRentCommandResponse>
{
    public required string RentId { get; init; }
}
#endregion

#region Response
public sealed record ExtendRentCommandResponse
{
    public required string RentId { get; init; }
    public required DateTime DueDate { get; init; }
}
#endregion

#region Validator
internal sealed class ExtendRentCommandValidator : AbstractValidator<ExtendRentCommand>
{
    public ExtendRentCommandValidator()
    {
        RuleFor(x => x.RentId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("대여ID는 공백일 수 없습니다.")
            .Length(GuidHelper.LengthWithHypen)
            .WithMessage("올바른 대여ID 길이가 아닙니다.")
            .Must(id => RentId.TryParse(id, out _))
            .WithMessage("올바른 대여ID 형식이 아닙니다.");
    }
}
#endregion

#region Handler
internal sealed class ExtendRentCommandHandler : ICommandHandler<ExtendRentCommand, ExtendRentCommandResponse>
{
    #region Constructor
    private readonly ILogger<ExtendRentCommandHandler> _logger;
    private readonly IRentRepository _rentRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ExtendRentCommandHandler(
        ILogger<ExtendRentCommandHandler> logger,
        IRentRepository rentRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _rentRepository = rentRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region Handle
    public async Task<ExtendRentCommandResponse> Handle(ExtendRentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {commandName}: {command}", request.GetGenericTypeName(), request);
        try
        {
            var rentId = RentId.Parse(request.RentId);
            var rent = await _rentRepository.GetRent(rentId, cancellationToken);
            if (rent is null)
                throw new RentNotFoundException(rentId);

            rent.Extend();
            await _rentRepository.Update(rent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ExtendRentCommandResponse
            {
                RentId = rent.Id.ToString(),
                DueDate = rent.DueDate
            };
        }
        finally
        {
            _logger.LogInformation("Finishing {commandName}: {command}", request.GetGenericTypeName(), request);
        }
    }
    #endregion
}
#endregion