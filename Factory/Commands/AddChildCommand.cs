using System;
using System.Collections.Generic;
using System.Linq;
using Common_Core.Extentions;
using Factory.Components;
using Newtonsoft.Json;

namespace Factory.Commands {
    public class AddChildCommand: Command {
        public AddChildCommand() : base() { }
        public AddChildCommand(string viewObjectKey) : base(viewObjectKey) {
            AddedComponentType = "Grid";
        }
        public AddChildCommand(string viewObjectKey, Component toAdd) : base(viewObjectKey) {
            AddedComponent = toAdd;
        }

        [JsonProperty]
        //public string AddedComponentJson { get; set; }
        //public Component AddedComponent {
        //    get {
        //        return AddedComponentJson.AutoDeserialize<Component>(AutoSerializer.SerializationMethod.RawJson);
        //    }
        //    set {
        //        AddedComponentJson = value.AutoSerialize(AutoSerializer.SerializationMethod.RawJson);
        //    }
        //}
        public Component AddedComponent {
            get;
            set;
        }
        public string AddedComponentType { 
            get {
                return AddedComponent.GetType().Name;
            }
            set {
                Type t = AppDomain.CurrentDomain.GetAssemblies()
                                        .Select(x => x.GetTypes())
                                        .SelectMany(x => x)
                                        .Where(x => x.Name == value).FirstOrDefault();
                 AddedComponent = Activator.CreateInstance(t) as Component;
            }
        }
        public string RemoveKey{ get; set; }
        public static IEnumerable<string> ComponentTypes { 
            get {
                return AppDomain.CurrentDomain.GetAssemblies()
                                        .Select(x => x.GetTypes())
                                        .SelectMany(x => x)
                                        .Where(x => !x.IsAbstract && typeof(Component).IsAssignableFrom(x) && x != typeof(Root))
                                        .Select(x => x.Name);
            }
        }

        public override void Do(Root root) {
            RemoveKey = AddedComponent.ID;
            root[ViewObjectKey].Add(AddedComponent);
        }
        public override void Undo(Root root) {
            root[ViewObjectKey].Remove(root[RemoveKey]);
        }
    }
}
