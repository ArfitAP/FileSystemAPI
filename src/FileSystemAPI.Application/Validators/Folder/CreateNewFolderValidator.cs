using FileSystemAPI.Application.Requests.Folder;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Validators.Folder
{
    public class CreateNewFolderValidator : AbstractValidator<CreateNewFolderRequest>
    {
        public CreateNewFolderValidator()
        {
            RuleFor(p => p.FolderName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(128).WithMessage("{PropertyName} must not exceed 128 characters.");

            RuleFor(p => p.ParentFolderID)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
