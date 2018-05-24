using Common_Core.Extentions;
using Factory.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Factory.Commands
{
    public class SwapChildCommand: Command {
        public SwapChildCommand() : base() { }
        public SwapChildCommand(string viewObjectKey) : base(viewObjectKey) {
            AddedComponentType = "Grid";
        }
        public SwapChildCommand(string viewObjectKey, Component toAdd) : base(viewObjectKey) {
            AddedComponent = toAdd;
        }

        [JsonProperty]
        public string AddedComponentJson { get; set; }
        public Component AddedComponent {
            get {
                return AddedComponentJson.AutoDeserialize<Component>(AutoSerializer.SerializationMethod.RawJson);
            }
            set {
                AddedComponentJson = value.AutoSerialize(AutoSerializer.SerializationMethod.RawJson);
            }
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
        public static IEnumerable<string> ComponentTypes {
            get {
                return AppDomain.CurrentDomain.GetAssemblies()
                                        .Select(x => x.GetTypes())
                                        .SelectMany(x => x)
                                        .Where(x => !x.IsAbstract && typeof(Component).IsAssignableFrom(x) && x != typeof(Root))
                                        .Select(x => x.Name);
            }
        }

        [JsonProperty]
        public string RemovedComponentJson { get; set; }
        public Component RemovedComponent {
            get {
                return RemovedComponentJson.AutoDeserialize<Component>(AutoSerializer.SerializationMethod.RawJson);
            }
            set {
                RemovedComponentJson = value.AutoSerialize(AutoSerializer.SerializationMethod.RawJson);
            }
        }

        public override void Do(Root root) {
            RemovedComponent = root[ViewObjectKey].Child;
            root[ViewObjectKey].Child = AddedComponent;
        }
        public override void Undo(Root root) {
            root[ViewObjectKey].Child = RemovedComponent;
        }
    }
}
