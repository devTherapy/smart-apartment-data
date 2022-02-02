using FluentValidation;
using Smart_Data.Application.Dtos;

namespace Smart_Data.Application.Features.Query.GlobalSearch
{
    public class GlobalSearchValidator : AbstractValidator<SearchQueryParams>
    {
        public GlobalSearchValidator()
        {
            RuleFor(x => x.Keyword).NotEmpty().WithMessage("Please enter a keyword to search").MaximumLength(256);

            RuleFor(x => x.Market).ForEach(x => x.MaximumLength(256));

            RuleFor(x => x.Size).GreaterThanOrEqualTo(10).LessThanOrEqualTo(50);

            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        }
    }
}
