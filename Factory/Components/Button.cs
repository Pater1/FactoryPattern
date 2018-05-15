using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    public class Button: Component {
        private string text, funcLink;

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
