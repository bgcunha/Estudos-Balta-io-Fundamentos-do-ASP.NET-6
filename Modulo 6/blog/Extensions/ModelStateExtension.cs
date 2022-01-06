using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace blog.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErros(this ModelStateDictionary modelState)
        {
            List<string> result = new();

            foreach (var item in modelState.Values)
            {
                var errors = item.Errors.Select(error=>error.ErrorMessage);

                result.AddRange(errors);            
            }

            return result;

        }
    }
}
