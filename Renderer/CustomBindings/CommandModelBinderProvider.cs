using Factory.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.CustomBindings
{
    public class CommandModelBinderProvider: IModelBinderProvider {
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            
        if(context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            if(!context.Metadata.ModelType.IsAbstract &&
                typeof(Command).IsAssignableFrom(context.Metadata.ModelType) &&
                (context.BindingInfo.BindingSource == null || !context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Services))
            ) {
                var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();
                for(var i = 0; i < context.Metadata.Properties.Count; i++) {
                    var property = context.Metadata.Properties[i];
                    propertyBinders.Add(property, context.CreateBinder(property));
                }
                return new CommandModelBinder(context.Metadata.ModelType, propertyBinders);
            }

            return null;
        }
    }
}
