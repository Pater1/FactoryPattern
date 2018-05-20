using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public class Image: Component {
        [JsonProperty]
        private string linkPath;
        [JsonProperty]
        private bool preserveAspect;

        public Image(string linkPath, bool preserveAspect = true) {
            this.LinkPath = linkPath;
            this.PreserveAspect = preserveAspect;
        }

        [JsonProperty]
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

        [JsonProperty]
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
