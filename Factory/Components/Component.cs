using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Factory.Commands;
using Factory.Iterator;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public abstract class Component: ICollection<Component> {
        public string BackgroundColor { get; set; } = "#FAFAFAAA";
        public virtual Root UltimateParent{ get{ return Parent.UltimateParent; } }

        public string TypeName => GetType().Name;
        [JsonProperty]
        public string id;
        public string ID { 
            get {
                return id;
            }
            set {
                id = value;
            }
        }
        protected Component(){
            id = Guid.NewGuid().ToString();
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

        [JsonIgnore]
        private Component parent;
        [JsonIgnore]
        public Component Parent {
            get {
                return parent;
            }
            internal set {
                parent = value;
            }
        }
        [JsonProperty]
        private List<Component> children = new List<Component>();
        public virtual List<Component> Children {
            get {
                if((ChildrenSupported & ChildrenHandling.multiple) == 0) {
                    throw new NotImplementedException("This component does not support multiple children");
                }

                foreach(Component child in children) {
                    child.Parent = this;
                }

                return children;
            }
            set {
                if((ChildrenSupported & ChildrenHandling.multiple) == 0) {
                    throw new NotImplementedException("This component does not support multiple children");
                }

                foreach(Component child in value){
                    child.Parent = this;
                }

                children = value;
            }
        }
        
        public virtual void Add(Component child) {
            if((ChildrenSupported & ChildrenHandling.multiple) == 0) {
                throw new NotImplementedException("This component does not support multiple children");
            }
            child.Parent = this;
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
                    children[0].Parent = this;
                    return children[0];
                }
            }
            set {
                if((ChildrenSupported & ChildrenHandling.single) == 0) {
                    throw new NotImplementedException("This component does not support single children");
                }
                value.Parent = this;

                if(Count < 1) {
                    children.Add(value);
                } else {
                    children[0] = value;
                }
            }
        }

        public int Count => children.Count();

        public bool IsReadOnly => ChildrenSupported == 0;

        public IEnumerable<Component> AllChildren(){
            return new AllChildrenIterator(this);
        }

        public IEnumerator<Component> GetEnumerator() {
            if(ChildrenSupported == ChildrenHandling.single){
                Component[] comps = new Component[] { Child };
                return comps.Where(x => x != null).GetEnumerator();
            }else if(ChildrenSupported == ChildrenHandling.multiple) {
                return children.GetEnumerator();
            }else{
                return new Component[0].Select(x => x).GetEnumerator();
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
        public bool Remove(string itemKey) {
            return children.Remove(children.Where(x => x.ID == itemKey).FirstOrDefault());
        }
        #endregion

        protected virtual bool AllowRemove { get { return true; } }
        public virtual IEnumerable<Command> Commands { get {
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "X",
                    X,
                    "number"
                );
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "Y",
                    Y,
                    "number"
                );
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "XSpan",
                    XSpan,
                    "number"
                );
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "YSpan",
                    YSpan,
                    "number"
                );
                yield return GeneralCommand.Build(
                    UltimateParent,
                    ID,
                    CallType.Property,
                    "BackgroundColor",
                    BackgroundColor,
                    "color"
                );
                if(ChildrenSupported == ChildrenHandling.single) {
                    yield return new SwapChildCommand(ID);
                } else if(ChildrenSupported == ChildrenHandling.multiple) {
                    yield return new AddChildCommand(ID);
                }

                if(AllowRemove){
                    yield return new RemoveAsChildCommand(ID);
                }
            }
        }
    }
}