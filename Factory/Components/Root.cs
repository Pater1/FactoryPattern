using Factory.Commands;
using Factory.Iterator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Components {
    [JsonObject(MemberSerialization.Fields)]
    public partial class Root: Component, IList<Command>, IDictionary<string, Component> {
        public override Root UltimateParent { get { return this; } }
        public override ChildrenHandling ChildrenSupported => ChildrenHandling.single;

        protected override bool AllowRemove { get { return false; } }

        //Drop head when add if iteration < Count
        [JsonProperty]
        private int iteration = 0;
        public void Up(){
            if(iteration < Commands.Count) {
                Commands[iteration].Do(this);
                iteration++;
            }
        }
        public void Down(){
            if(iteration > 0) {
                iteration--;
                Commands[iteration].Undo(this);
            }
        }

        #region IList<Command>
        private IList<Command> commands = new List<Command>();
        [JsonProperty]
        public IList<Command> Commands {
            get {
                return commands;
            }

            set {
                commands = value;
            }
        }
        
        public Command this[int index] {
            get {
                return Commands[index];
            }

            set {
                Commands[index] = value;
            }
        }

        public void Add(Command item) => Commands.Add(item);

        public bool Contains(Command item) => Commands.Contains(item);

        public void CopyTo(Command[] array, int arrayIndex) => Commands.CopyTo(array, arrayIndex);

        public bool Remove(Command item) => Commands.Remove(item);

        IEnumerator<Command> IEnumerable<Command>.GetEnumerator() => Commands.GetEnumerator();

        public int IndexOf(Command item) => Commands.IndexOf(item);

        public void Insert(int index, Command item) => Commands.Insert(index, item);

        public void RemoveAt(int index) => Commands.RemoveAt(index);
        #endregion

        public ICollection<string> Keys => (this as ICollection<Component>).Select(x => x.ID).ToList();

        public ICollection<Component> Values => (this as ICollection<Component>);

        public Component this[string key] {
            get {
                return new AllChildrenIterator(this).Where(x => x.ID == key).FirstOrDefault();
            }

            set {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Adds value as child to Component at key
        /// </summary>
        /// <param name="key">the key to the component to which to add child as a child component</param>
        /// <param name="child">the child component to add to the component at key</param>
        public void Add(string key, Component child) {
            if(TryGetValue(key, out Component parent)) {
                parent.Add(child);
            }
        }

        public bool ContainsKey(string key) => Keys.Contains(key);

        public bool Remove(string key) {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out Component value) {
            if(ContainsKey(key)){
                value = this[key];
                return true;
            }else{
                value = null;
                return false;
            }
        }

        public void Add(KeyValuePair<string, Component> item) => Add(item.Key, item.Value);

        private IEnumerable<KeyValuePair<string, Component>> KVList =>
            (this as ICollection<Component>).Select(x => new KeyValuePair<string, Component>(x.ID, x));

        public bool Contains(KeyValuePair<string, Component> item) => KVList.Contains(item);

        public void CopyTo(KeyValuePair<string, Component>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, Component> item) => Remove(item.Key);

        IEnumerator<KeyValuePair<string, Component>> IEnumerable<KeyValuePair<string, Component>>.GetEnumerator() =>
            KVList.GetEnumerator();
    }
}
