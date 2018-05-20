using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public abstract class Component: ICollection<Component> {
        public Guid ID { get; private set; }
        public Component(){
            ID = Guid.NewGuid();
        }

        #region GridPositioning
        [JsonProperty]
        private int x, y, xSpan, ySpan;

        [JsonProperty]
        public int X {
            get {
                return x;
            }

            set {
                x = value > 0 ? value : 0;
            }
        }
        [JsonProperty]
        public int Y {
            get {
                return y;
            }

            set {
                y = value > 0 ? value : 0;
            }
        }
        [JsonProperty]
        public int XSpan {
            get {
                return xSpan;
            }

            set {
                xSpan = value > 0 ? value : 0;
            }
        }
        [JsonProperty]
        public int YSpan {
            get {
                return ySpan;
            }

            set {
                ySpan = value > 0 ? value : 0;
            }
        }
        #endregion
        
        #region ChildrenHandling
        [Flags]
        public enum ChildrenHandling {
            none = 0,
            multiple = 1,
            single = 2,
        }
        public virtual ChildrenHandling ChildrenSupported => ChildrenHandling.none;

        [JsonProperty]
        private List<Component> children = new List<Component>();
        public virtual List<Component> Children {
            get {
                if((ChildrenSupported & ChildrenHandling.multiple) == 0) {
                    throw new NotImplementedException("This component does not support multiple children");
                }
                
                return children;
            }
            set {
                if((ChildrenSupported & ChildrenHandling.multiple) == 0) {
                    throw new NotImplementedException("This component does not support multiple children");
                }

                children = value;
            }
        }


        public virtual void Add(Component child) {
            if((ChildrenSupported & ChildrenHandling.multiple) == 0) {
                throw new NotImplementedException("This component does not support multiple children");
            }

            children.Add(child);
        }
        public virtual Component Child {
            get {
                if((ChildrenSupported & ChildrenHandling.single) == 0) {
                    throw new NotImplementedException("This component does not support single children");
                }

                if(Count < 1) {
                    return null;
                } else {
                    return children[0];
                }
            }
            set {
                if((ChildrenSupported & ChildrenHandling.single) == 0) {
                    throw new NotImplementedException("This component does not support single children");
                }

                if(Count < 1) {
                    children.Add(value);
                } else {
                    children[0] = value;
                }
            }
        }

        public int Count => children.Count();

        public bool IsReadOnly => ChildrenSupported == 0;

        public IEnumerator<Component> GetEnumerator() {
            try {
                Component[] comps = new Component[] { Child };
                return comps.Select(x => x).GetEnumerator();
            } catch {
                return children.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Clear() {
            children.Clear();
        }

        public bool Contains(Component item) {
            return children.Contains(item);
        }

        public void CopyTo(Component[] array, int arrayIndex) {
            children.CopyTo(array, arrayIndex);
        }

        public bool Remove(Component item) {
            return children.Remove(item);
        }
        #endregion
    }
}