using FileSystemAPI.Application.Requests.File;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Validators.File
{
    public class GetFileValidator : AbstractValidator<GetFileRequest>
    {
        public GetFileValidator()
        {
            RuleFor(p => p.FileID)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
