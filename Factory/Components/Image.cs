using Factory.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public partial class Image: Component {
        [JsonProperty]
        private string linkPath;
        [JsonProperty]
        private bool preserveAspect;

        public Image(string linkPath, bool preserveAspect = true) {
            this.LinkPath = linkPath;
            this.PreserveAspect = preserveAspect;
        }
        public Image() {
            this.LinkPath = "https://www.w3schools.com/w3css/img_lights.jpg";
            this.PreserveAspect = true;
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

        public override IEnumerable<Command> Commands {
            get {
                foreach(Command cmb in base.Commands) {
                    yield return cmb;
                }
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "LinkPath",
                    LinkPath,
                    "url"
                );
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "PreserveAspect",
                    PreserveAspect,
                    "checkbox"
                );
            }
        }
    }
}
