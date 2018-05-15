using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    public class TextBox: Component {
        private string text;

        public TextBox(string text) {
            this.Text = text;
        }

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
