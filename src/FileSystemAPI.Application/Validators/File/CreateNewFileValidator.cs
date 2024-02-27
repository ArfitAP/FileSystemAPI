using FileSystemAPI.Application.Requests.File;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Validators.File
{
    public class CreateNewFileValidator : AbstractValidator<CreateNewFileRequest>
    {
        public CreateNewFileValidator()
        {
            RuleFor(p => p.FileName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(128).WithMessage("{PropertyName} must not exceed 128 characters.");

            RuleFor(p => p.FolderID)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
