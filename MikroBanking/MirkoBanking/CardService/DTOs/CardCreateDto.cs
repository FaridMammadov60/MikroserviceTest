using FluentValidation;

namespace CardService.DTOs
{
    public class CardCreateDto
    {
        public string Name { get; set; }
    }
    public class CardDtoValidator : AbstractValidator<CardCreateDto>
    {
        public CardDtoValidator()
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage("bos olmaz");
          
        }
    }
}
