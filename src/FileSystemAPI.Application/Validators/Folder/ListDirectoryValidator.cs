using FileSystemAPI.Application.Requests.Folder;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Validators.Folder
{
    public class ListDirectoryValidator : AbstractValidator<ListDirectoryRequest>
    {
        public ListDirectoryValidator()
        {
            RuleFor(p => p.FolderID)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
