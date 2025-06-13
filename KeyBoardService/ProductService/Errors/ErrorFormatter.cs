using FluentValidation.Results;

namespace ProductService.API.Error
{
    public static class ErrorFormatter
    {
        public static string[] Deserialize(IEnumerable<ValidationFailure> failures) =>
            failures
                .Select(f => f.ErrorMessage)
                .ToArray();
    }
}
