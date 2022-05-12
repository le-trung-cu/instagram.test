using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Shared.DataTransferObjects;
using Entities.Exceptions;

namespace Instagram.Presentation.ModelBinders
{
    public class PostForCreationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(PostBaseForCreationDto))
            {
                return null;
            }
            var subclasses = new[]{
                typeof(PostForCreationDto),
                typeof(PostPhotoForCreationDto),
                typeof(PostVideoForCreationDto),
                typeof(PostCarouselForCreationDto),
                typeof(PhotoItemForCreationDto),
                typeof(VideoItemForCreationDto)
            };
            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in subclasses)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new PostForCreationModelBinder(binders);
        }
    }

    public class PostForCreationModelBinder : IModelBinder
    {
        private readonly Dictionary<Type, (ModelMetadata, IModelBinder)> binders;

        public PostForCreationModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelTypeName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(PostBaseForCreationDto.PostType));
            var modelTypeValue = bindingContext.ValueProvider.GetValue(modelTypeName).FirstValue;
            if (modelTypeValue == null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            };

            IModelBinder modelBinder;
            ModelMetadata modelMetadata;

            (modelMetadata, modelBinder) = modelTypeValue switch
            {
                "Post" => binders[typeof(PostForCreationDto)],
                "PostPhoto" => binders[typeof(PostPhotoForCreationDto)],
                "PostVideo" => binders[typeof(PostVideoForCreationDto)],
                "PostCarousel" => binders[typeof(PostCarouselForCreationDto)],
                _ => throw new PostTypeUnprocessableException()
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
