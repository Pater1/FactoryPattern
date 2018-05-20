using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public class Button: Component {
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
    }
}
