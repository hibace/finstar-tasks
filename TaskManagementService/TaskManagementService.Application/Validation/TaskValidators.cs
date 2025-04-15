using FluentValidation;
using TaskManagementService.Shared.Dtos;

namespace TaskManagementService.Application.Validation
{
    /// <summary>
    /// Валидатор для CreateTaskDto
    /// </summary>
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название задачи обязательно")
                .MaximumLength(200).WithMessage("Название задачи не должно превышать 200 символов");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание задачи обязательно")
                .MaximumLength(1000).WithMessage("Описание задачи не должно превышать 1000 символов");
        }
    }

    /// <summary>
    /// Валидатор для UpdateTaskDto
    /// </summary>
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название задачи обязательно")
                .MaximumLength(200).WithMessage("Название задачи не должно превышать 200 символов");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание задачи обязательно")
                .MaximumLength(1000).WithMessage("Описание задачи не должно превышать 1000 символов");
        }
    }
} 