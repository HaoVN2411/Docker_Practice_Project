using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace NET1717_Lab01_ProductManagement.API.Extentions
{
    public class KebabCaseQueryModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.BindingInfo.BindingSource == BindingSource.Query)
            {
                return new BinderTypeModelBinder(typeof(KebabCaseQueryModelBinder));
            }

            return null;
        }
    }
}
