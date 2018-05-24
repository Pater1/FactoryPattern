using Factory.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public partial class Button: Component {
        [JsonProperty]
        private string text;
        [JsonProperty]
        private string funcLink;

        public override ChildrenHandling ChildrenSupported => ChildrenHandling.single;
        public override Component Child {
            get {
                if(base.Child == null){
                    return new TextBox(Text);
                }else{
                    return base.Child;
                }
            }

            set {
                base.Child = value;
            }
        }

        public Button(string text, string funcLink) {
            this.Text = text;
            this.FuncLink = funcLink;
        }
        public Button() {
            this.Text = "default text goes here!";
            this.FuncLink = "http://google.com";
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

        [JsonProperty]
        public string FuncLink {
            get {
                return funcLink;
            }

            set {
                if(value != null) {
                    funcLink = value;
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
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "FuncLink",
                    FuncLink,
                    "url"
                );
            }
        }
    }
}
