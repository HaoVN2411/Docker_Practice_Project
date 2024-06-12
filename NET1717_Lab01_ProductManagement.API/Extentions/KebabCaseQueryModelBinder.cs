using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NET1717_Lab01_ProductManagement.API.Extentions
{
    public class KebabCaseQueryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelType = bindingContext.ModelMetadata.ModelType;
            var model = Activator.CreateInstance(modelType);

            var properties = modelType.GetProperties();
            foreach (var property in properties)
            {
                var kebabCaseName = ConvertToKebabCase(property.Name);
                var value = bindingContext.ValueProvider.GetValue(kebabCaseName).FirstValue;
                if (value != null)
                {
                    property.SetValue(model, Convert.ChangeType(value, property.PropertyType));
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private string ConvertToKebabCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
        }   
    }
}
