using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    public class Root: Component {
        public override ChildrenHandling ChildrenSupported => ChildrenHandling.single;
    }
}
