using FluentValidation;
using UserService.Entities;

namespace UserService.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Cins { get; set; }
        public Countries Countries { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CheckPassword { get; set; }
    }
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.Username).NotEmpty().WithMessage("bos olmaz");
            RuleFor(r => r.Surname).NotEmpty().WithMessage("bos olmaz");
            RuleFor(r => r.Email).NotEmpty().WithMessage("bos olmaz");
            RuleFor(r => r.Password).NotEmpty().WithMessage("bos olmaz").MinimumLength(8).MaximumLength(24);
            RuleFor(r => r.CheckPassword).NotEmpty().WithMessage("bos olmaz").MinimumLength(8).MaximumLength(24);
            RuleFor(r => r.PhoneNumber).NotEmpty().WithMessage("bos olmaz").MinimumLength(10).MaximumLength(24);
            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.Password != r.CheckPassword)
                {
                    context.AddFailure("Password", "duz deyil");
                }
            });
        }
    }
}