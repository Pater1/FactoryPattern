﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Factory.Components {
    public abstract class Component: ICollection<Component> {
        #region GridPositioning
        private int x, y, xSpan, ySpan;

        public int X {
            get {
                return x;
            }

            set {
                x = value > 0 ? value : 0;
            }
        }
        public int Y {
            get {
                return y;
            }

            set {
                y = value > 0 ? value : 0;
            }
        }
        public int XSpan {
            get {
                return xSpan;
            }

            set {
                xSpan = value > 0 ? value : 0;
            }
        }
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

        private List<Component> children = new List<Component>();


        public virtual void Add(Component child) {
            if((ChildrenSupported & ChildrenHandling.multiple) == 0) {
                throw new NotImplementedException("This component does not support multiple children");
            }

            children.Add(child);
        }
        public virtual Component Child {
            get {
                if((ChildrenSupported & ChildrenHandling.single) == 0) {
                    throw new NotImplementedException("This component does not support multiple children");
                }

                if(Count < 1) {
                    return null;
                } else {
                    return children[0];
                }
            }
            set {
                if((ChildrenSupported & ChildrenHandling.single) == 0) {
                    throw new NotImplementedException("This component does not support multiple children");
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