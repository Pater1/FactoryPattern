using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.CustomBindings {
    public class CommandModelBinder: ComplexTypeModelBinder {
        private Type InstantiatedType { get; set; }
        public CommandModelBinder(Type instantiatedType, IDictionary<ModelMetadata, IModelBinder> propertyBinder): base(propertyBinder) {
            InstantiatedType = instantiatedType;
        }

        protected override object CreateModel(ModelBindingContext bindingContext) {
            object ret = Activator.CreateInstance(InstantiatedType);

            return ret;// bindingContext.HttpContext.RequestServices.GetService(bindingContext.ModelType);
        }
    }
}
