using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace MyRESTServices.Helpers
{
    public static class Extensions
    {
        public static void AddToModelState(this IEnumerable<ValidationFailure> errors, ModelStateDictionary modelState)
        {
            foreach (var error in errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
