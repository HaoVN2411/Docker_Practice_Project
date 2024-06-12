using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

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
                var valueProviderResult = bindingContext.ValueProvider.GetValue(kebabCaseName);

                if (valueProviderResult == ValueProviderResult.None)
                {
                    continue;
                }

                var value = valueProviderResult.FirstValue;

                if (value != null)
                {
                    try
                    {
                        var convertedValue = ConvertValue(value, property.PropertyType);
                        property.SetValue(model, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        bindingContext.ModelState.TryAddModelError(property.Name, ex.Message);
                    }
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private string ConvertToKebabCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
        }

        private object ConvertValue(string value, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return value;
            }

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }

                targetType = Nullable.GetUnderlyingType(targetType);
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, value);
            }

            return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
        //public Task BindModelAsync(ModelBindingContext bindingContext)
        //{
        //    if (bindingContext == null)
        //    {
        //        throw new ArgumentNullException(nameof(bindingContext));
        //    }

        //    var modelType = bindingContext.ModelMetadata.ModelType;
        //    var model = Activator.CreateInstance(modelType);

        //    var properties = modelType.GetProperties();
        //    foreach (var property in properties)
        //    {
        //        var kebabCaseName = ConvertToKebabCase(property.Name);
        //        var value = bindingContext.ValueProvider.GetValue(kebabCaseName).FirstValue;
        //        if (value != null)
        //        {
        //            property.SetValue(model, Convert.ChangeType(value, property.PropertyType));
        //        }
        //    }

        //    bindingContext.Result = ModelBindingResult.Success(model);
        //    return Task.CompletedTask;
        //}

        //private string ConvertToKebabCase(string str)
        //{
        //    return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
        //}
    }
}
