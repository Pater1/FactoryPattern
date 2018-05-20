using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public class Root: Component {
        public override ChildrenHandling ChildrenSupported => ChildrenHandling.single;
    }
}
