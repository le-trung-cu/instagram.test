using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Shared.DataTransferObjects;

namespace Instagram.Presentation.ModelBinders
{
    public class MediaItemForCreationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType != typeof(MediaItemForCreationDto))
            {
                return null;
            }
            var subclasses = new[] { typeof(PhotoItemForCreationDto), typeof(VideoItemForCreationDto) };

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in subclasses)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new MediaItemForCreationModelBinder(binders);
        }
    }

    public class MediaItemForCreationModelBinder : IModelBinder
    {
        private readonly Dictionary<Type, (ModelMetadata, IModelBinder)> binders;

        public MediaItemForCreationModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelTypeName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(MediaItemForCreationDto.ModelType));
            var modelTypeValue = bindingContext.ValueProvider.GetValue(modelTypeName).FirstValue;
            if (modelTypeValue == null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            };

            IModelBinder modelBinder;
            ModelMetadata modelMetadata;
            (modelMetadata, modelBinder) = modelTypeValue.ToLower() switch
            {
                "photo" => binders[typeof(PhotoItemForCreationDto)],
                "video" => binders[typeof(VideoItemForCreationDto)],
                _ => throw new MediaTypeUnprocessableException(),
            };

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
