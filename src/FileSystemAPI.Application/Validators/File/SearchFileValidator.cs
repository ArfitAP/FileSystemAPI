using FileSystemAPI.Application.Requests.File;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Validators.File
{
    public class SearchFileValidator : AbstractValidator<SearchFileRequest>
    {
        public SearchFileValidator()
        {
            RuleFor(p => p.FolderID)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .When(p => p.AllFolders == false);
        }
    }
}
