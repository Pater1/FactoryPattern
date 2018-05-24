using Factory.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public partial class TextBox: Component {
        [JsonProperty]
        private string text;

        public TextBox(string text) {
            this.Text = text;
        }
        public TextBox() {
            this.Text = "defalut text here!";
        }

        [JsonProperty]
        public string Text {
            get {
                return text;
            }

            set {
                if(value != null) { 
                    text = value;
                }
            }
        }

        public override IEnumerable<Command> Commands {
            get {
                foreach(Command cmb in base.Commands) {
                    yield return cmb;
                }
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "Text",
                    Text
                );
            }
        }
    }
}
