using Common_Core.Extentions;
using Factory.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Factory.Commands {
    public class RemoveAsChildCommand: Command {
        public RemoveAsChildCommand() : base() { }
        public RemoveAsChildCommand(string viewObjectKey) : base(viewObjectKey) {}
        public RemoveAsChildCommand(string viewObjectKey, Component toAdd) : base(viewObjectKey) {
            RemovedComponent = toAdd;
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
        public string ParentComponentKey { get; set; }

        public override void Do(Root root) {
            ParentComponentKey = root[ViewObjectKey].Parent.ID;
            RemovedComponent = root[ViewObjectKey];
            root[ParentComponentKey].Remove(ViewObjectKey);
        }
        public override void Undo(Root root) {
            root[ParentComponentKey].Add(RemovedComponent);
            ViewObjectKey = RemovedComponent.ID;
        }
    }
}
