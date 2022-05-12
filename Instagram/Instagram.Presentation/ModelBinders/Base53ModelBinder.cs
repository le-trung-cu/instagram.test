using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared;

namespace Instagram.Presentation.ModelBinders
{
    public class Base53ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }
            if(context.Metadata.ModelType != typeof(Base53))
            {
                return null;
            }
            return new Base53ModelBinder();
        }
    }
    public class Base53ModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsComplexType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            bindingContext.Model = new Base53(providedValue);
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
