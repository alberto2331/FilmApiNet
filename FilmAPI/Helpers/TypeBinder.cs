using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FilmAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nameProperty = bindingContext.ModelName;
            var valueProvider = bindingContext.ValueProvider.GetValue(nameProperty);
            if (valueProvider == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try
            {
                var decerializedValue = JsonConvert.DeserializeObject<T>(valueProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(decerializedValue);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(nameProperty, "Invalid value for type List<int>");
            }
            return Task.CompletedTask;
        }
    }
}
