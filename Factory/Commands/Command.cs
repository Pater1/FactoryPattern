using Factory.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Factory.Commands {
    public abstract class Command {
        [HiddenInput]
        [JsonProperty]
        public string ViewObjectKey { get; set; }
        public string ViewObjectKeyString { 
            get {
                return ViewObjectKey.ToString();
            }
            set{
                ViewObjectKey = value;
            }
        }

        public Command() {}
        public Command(string viewObjectKey) {
            ViewObjectKey = viewObjectKey;
        }

        public abstract void Do(Root root);
        public abstract void Undo(Root root);
    }
}
