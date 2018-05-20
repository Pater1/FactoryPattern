using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public class TextBox: Component {
        [JsonProperty]
        private string text;

        public TextBox(string text) {
            this.Text = text;
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
    }
}
