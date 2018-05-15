using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    public class Image: Component {
        private string linkPath;
        private bool preserveAspect;

        public Image(string linkPath, bool preserveAspect = true) {
            this.LinkPath = linkPath;
            this.PreserveAspect = preserveAspect;
        }

        public string LinkPath {
            get {
                return linkPath;
            }

            set {
                if(value != null) { 
                    linkPath = value;
                }
            }
        }

        public bool PreserveAspect {
            get {
                return preserveAspect;
            }

            set {
                preserveAspect = value;
            }
        }
    }
}
