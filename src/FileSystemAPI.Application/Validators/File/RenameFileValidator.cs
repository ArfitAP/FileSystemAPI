using FileSystemAPI.Application.Requests.File;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Validators.File
{
    public class RenameFileValidator : AbstractValidator<RenameFileRequest>
    {
        public RenameFileValidator()
        {
            RuleFor(p => p.NewFileName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(128).WithMessage("{PropertyName} must not exceed 128 characters.");

            RuleFor(p => p.FileID)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
