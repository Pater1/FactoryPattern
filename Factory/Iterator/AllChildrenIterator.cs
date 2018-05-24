using Factory.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static Factory.Components.Component;

namespace Factory.Iterator {
    public struct AllChildrenIterator: IEnumerable<Component> {
        private Component root;

        public AllChildrenIterator(Component root){
            this.root = root;
        }

        public IEnumerator<Component> GetEnumerator() {
            yield return root;
            if(root.ChildrenSupported == ChildrenHandling.multiple) {
                foreach(Component child in root.Children) {
                    foreach(Component grandChild in new AllChildrenIterator(child)) {
                        yield return grandChild;
                    }
                }
            } else if(root.ChildrenSupported == ChildrenHandling.single) {
                if(root.Child != null) {
                    foreach(Component child in new AllChildrenIterator(root.Child)) {
                        yield return child;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
