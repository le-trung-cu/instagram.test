using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Shared.DataTransferObjects;

namespace Instagram.Presentation.ModelBinders
{
    public class UserTagMediaForCreationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(UserTagMediaForCreationDto))
            {
                return null;
            }
            var subclasses = new[] { typeof(UserTagPhotoForCreationDto), typeof(UserTagVideoForCreationDto) };

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in subclasses)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new UserTagMediaForCreationModelBinder(binders);
        }
    }

    public class UserTagMediaForCreationModelBinder : IModelBinder
    {
        private readonly Dictionary<Type, (ModelMetadata, IModelBinder)> binders;

        public UserTagMediaForCreationModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelKindName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(UserTagPhotoForCreationDto.Top));

            //var modelTypeValue = bindingContext.ValueProvider.GetValue(modelKindName).FirstValue!;
            IModelBinder modelBinder;
            ModelMetadata modelMetadata;
            if (string.IsNullOrEmpty(modelKindName))
            {
                (modelMetadata, modelBinder) = binders[typeof(UserTagPhotoForCreationDto)];
            }
            else
            {
                (modelMetadata, modelBinder) = binders[typeof(UserTagVideoForCreationDto)];
            }
            var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                bindingContext.ActionContext,
                bindingContext.ValueProvider,
                modelMetadata,
                bindingInfo: null,
                bindingContext.ModelName);

            await modelBinder.BindModelAsync(newBindingContext);

            bindingContext.Result = newBindingContext.Result;

            if (newBindingContext.Result.IsModelSet)
            {
                // Setting the ValidationState ensures properties on derived types are correctly 
                bindingContext.ValidationState[newBindingContext.Result.Model!] = new ValidationStateEntry
                {
                    Metadata = modelMetadata,
                };
            }
        }
    }
}
