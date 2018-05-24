using Factory.Components;
using Newtonsoft.Json;
using System;
using ComMod = System.ComponentModel;
using System.Reflection;
using System.Linq;

namespace Factory.Commands {
    /// <summary>
    /// Made for instant extendability and easy [de]serialization for new view types. Do not recommend using this class directly if it can be avoided.
    /// </summary>
    public class GeneralCommand: Command {
        private static object AutoPullValue(object pullFrom, CallType callType, FieldInfo fieldInfo, PropertyInfo propInfo, MethodInfo methodInfo) {
            switch(callType) {
                case CallType.Field:
                    return fieldInfo.GetValue(pullFrom);
                case CallType.Property:
                    return propInfo.GetValue(pullFrom);
                case CallType.Method:
                    throw new NotSupportedException("Cannot automatically determine undo value for a method call.");
                default:
                    throw new InvalidOperationException("Cannot automatically determine undo value for an unknown call type.");
            }
        }
        private static Type ValidateCallType(ref CallType callType, Type type, string parameterName, out FieldInfo fieldInfo, out PropertyInfo propInfo, out MethodInfo methodInfo) {
            fieldInfo = (callType == CallType.Field || callType == CallType.auto) ? type.GetField(parameterName) : null;
            propInfo = (callType == CallType.Property || callType == CallType.auto) ? type.GetProperty(parameterName) : null;
            methodInfo = (callType == CallType.Method || callType == CallType.auto) ? type.GetMethod(parameterName) : null;

            if(propInfo == null && fieldInfo == null && methodInfo == null) {
                throw new ArgumentException($"There is no {callType.ToString()} {parameterName} on the component {type.Name}!");
            } else if(callType == CallType.auto) {
                Exception throwIfMultiple = new ArgumentException($"There is multiple parameters called {callType.ToString()} on the component {type.Name}! Please specify the call type.");
                if(propInfo != null) {
                    callType = CallType.Property;
                }
                if(fieldInfo != null) {
                    if(callType == CallType.auto) {
                        callType = CallType.Field;
                    } else {
                        throw throwIfMultiple;
                    }
                }
                if(methodInfo != null) {
                    if(callType == CallType.auto) {
                        callType = CallType.Method;
                    } else {
                        throw throwIfMultiple;
                    }
                }
            }

            switch(callType) {
                case CallType.Field:
                    return fieldInfo.FieldType;
                case CallType.Property:
                    return propInfo.PropertyType;
                case CallType.Method:
                    return typeof(object[]);
                default:
                    return null;
            }
        }
        public static GeneralCommand Build(Root root, string viewObjectKey, CallType updateCallType, string updateParameterName, object updateParameterValue, string htmlType = "text", CallType undoCallType = CallType.auto, string undoParameterName = null, object undoParameterValue = null) {
            Component comp = root[viewObjectKey];
            if(comp == null) {
                throw new ArgumentException($"There is no component at {viewObjectKey} to edit in this view!");
            }

            GeneralCommand comm = new GeneralCommand(viewObjectKey);
            comm.HtmlType = htmlType;

            Type compType = comp.GetType();

            Type updatePropType = ValidateCallType(ref updateCallType, compType, updateParameterName, out FieldInfo updateFieldInfo, out PropertyInfo updatePropInfo, out MethodInfo updateMethodInfo);
            comm.UpdatePropertyType = updatePropType.Name;
            comm.UpdateParameterName = updateParameterName;

            Type setType = updateParameterValue.GetType();
            if(!updatePropType.IsAssignableFrom(setType)) {
                throw new ArgumentException($"The value of type {setType.Name} cannot be assigned to the property {updateParameterName} of type {updatePropType.Name} on a {compType.Name}!");
            }
            comm.UpdateParameterValue = updateParameterValue;
            comm.UpdateCallType = updateCallType;

            if(undoCallType == CallType.auto) {
                undoCallType = updateCallType;
            }
            comm.UndoCallType = undoCallType;

            if(undoParameterName != null) {
                Type undoPropType = ValidateCallType(ref undoCallType, compType, undoParameterName, out FieldInfo undoFieldInfo, out PropertyInfo undoPropInfo, out MethodInfo undoMethodInfo);
                comm.UndoPropertyType = undoPropType.Name;
                comm.UndoParameterName = undoParameterName;

                Type unsetType = undoParameterValue.GetType();
                if(!undoPropType.IsAssignableFrom(unsetType)) {
                    throw new ArgumentException($"The value of type {unsetType.Name} cannot be assigned to the property {updateParameterName} of type {undoPropType.Name} on a {compType.Name}!");
                }
                comm.UpdateParameterValue = undoParameterValue;
            } else {
                comm.UndoPropertyType = comm.UpdatePropertyType;
                comm.UndoParameterName = comm.UpdateParameterName;
                comm.UndoParameterValue = AutoPullValue(comp, undoCallType, updateFieldInfo, updatePropInfo, updateMethodInfo);
            }

            return comm;
        }

        private static object ConvertType(object obj, Type propertyType, string parameterName) {
            Type valueType = obj.GetType();
            if(propertyType.IsAssignableFrom(valueType)) {
                return obj;
            } else {
                object ret = null;

                try {
                    ret = Convert.ChangeType(obj, propertyType);
                } catch { }

                if(ret == null){ 
                    var converter = ComMod.TypeDescriptor.GetConverter(propertyType);
                    if(converter != null) {
                        ret = converter.ConvertFrom(obj);
                    }
                }
                
                if(ret != null) {
                    return ret;
                }else{
                    throw new ArgumentException($"Can only assign data of type {propertyType.Name} to this Command, editing {parameterName} of type {propertyType.Name}!");
                }
            }
        }

        public string HtmlType { get; set; } = "text";
        #region update
        [JsonProperty]
        private string updateParameterName;
        [JsonProperty]
        public string UpdateParameterName {
            get {
                return updateParameterName;
            }

            set {
                updateParameterName = value;
            }
        }
        
        [JsonProperty]
        private string updatePropertyType;
        [JsonProperty]
        public string UpdatePropertyType {
            get {
                return updatePropertyType;
            }

            set {
                updatePropertyType = value;
            }
        }
        [JsonIgnore]
        private Type UpdatePropType {
            get {
                return AppDomain.CurrentDomain.GetAssemblies()
                                            .Select(x => x.GetTypes())
                                            .SelectMany(x => x)
                                            .Where(x => x.Name == UpdatePropertyType)
                                            .FirstOrDefault();
            }
        }
        
        [JsonProperty]
        private object updateParameterValue;
        [JsonProperty]
        public object UpdateParameterValue {
            get {
                return updateParameterValue;
            }

            private set {
                updateParameterValue = ConvertType(value, UpdatePropType, UpdateParameterName);
            }
        }
        
        [JsonProperty]
        private CallType updateCallType;
        [JsonProperty]
        public CallType UpdateCallType {
            get {
                return updateCallType;
            }

            private set {
                updateCallType = value;
            }
        }
        #endregion
        #region undo
        [JsonProperty]
        private string undoParameterName;
        [JsonProperty]
        public string UndoParameterName {
            get {
                return undoParameterName;
            }

            set {
                undoParameterName = value;
            }
        }
        
        [JsonProperty]
        private string undoPropertyType;
        [JsonProperty]
        public string UndoPropertyType {
            get {
                return undoPropertyType;
            }

            set {
                undoPropertyType = value;
            }
        }
        [JsonIgnore]
        private Type UndoPropType {
            get {
                return AppDomain.CurrentDomain.GetAssemblies()
                                            .Select(x => x.GetTypes())
                                            .SelectMany(x => x)
                                            .Where(x => x.Name == UndoPropertyType)
                                            .FirstOrDefault();
            }
        }

        [JsonProperty]
        private object undoParameterValue;
        [JsonProperty]
        public object UndoParameterValue {
            get {
                return undoParameterValue;
            }

            private set {
                undoParameterValue = ConvertType(value, UndoPropType, UndoParameterName);
            }
        }

        [JsonProperty]
        private CallType undoCallType;
        [JsonProperty]
        public CallType UndoCallType {
            get {
                return undoCallType;
            }

            private set {
                undoCallType = value;
            }
        }
        #endregion

        private GeneralCommand(string viewObjectKey) : base(viewObjectKey) { }
        public GeneralCommand() : base() { }

        public override void Do(Root root) {
            Component comp = root[ViewObjectKey];
            switch(UpdateCallType) {
                case CallType.Field:
                    comp.GetType().GetField(UpdateParameterName).SetValue(comp, UpdateParameterValue);
                    break;
                case CallType.Property:
                    var a = comp.GetType();
                    var b = a.GetProperty(UpdateParameterName);
                    b.SetValue(comp, UpdateParameterValue);
                    break;
                case CallType.Method:
                    comp.GetType().GetMethod(UpdateParameterName).Invoke(comp, (object[])UpdateParameterValue);
                    break;
                default:
                    throw new InvalidOperationException("Cannot execute call for an unknown call type.");
            }
        }
        public override void Undo(Root root) {
            Component comp = root[ViewObjectKey];
            switch(UndoCallType) {
                case CallType.Field:
                    comp.GetType().GetField(UndoParameterName).SetValue(comp, UndoParameterValue);
                    break;
                case CallType.Property:
                    comp.GetType().GetProperty(UndoParameterName).SetValue(comp, UndoParameterValue);
                    break;
                case CallType.Method:
                    comp.GetType().GetMethod(UndoParameterName).Invoke(comp, (object[])UndoParameterValue);
                    break;
                default:
                    throw new InvalidOperationException("Cannot execute call for an unknown call type.");
            }
        }
    }
}