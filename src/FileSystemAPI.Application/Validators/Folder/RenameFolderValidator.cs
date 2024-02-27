using FileSystemAPI.Application.Requests.Folder;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Validators.Folder
{
    public class RenameFolderValidator : AbstractValidator<RenameFolderRequest>
    {
        public RenameFolderValidator()
        {
            RuleFor(p => p.NewFolderName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(128).WithMessage("{PropertyName} must not exceed 128 characters.");

            RuleFor(p => p.FolderID)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(-1).WithMessage("Cannot rename root !");
        }
    }
}
