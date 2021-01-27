using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common {
    [Serializable]
    public sealed class ExposedEvent {
        [ListDrawerSettings]
        public List<ExposedCall> calls;

        public void Invoke(IExposedPropertyTable table) {
            foreach (var exposedCall in calls) {
                exposedCall.Invoke(table);
            }
        }
    }

    [Serializable]
    public sealed class ExposedCall {
        public static readonly Tuple<ExposedCallType, Type>[] AllowedTypes = {
            new Tuple<ExposedCallType, Type>(ExposedCallType.Bool, typeof(bool)),
            new Tuple<ExposedCallType, Type>(ExposedCallType.Float, typeof(float)),
            new Tuple<ExposedCallType, Type>(ExposedCallType.Int, typeof(int)),
            new Tuple<ExposedCallType, Type>(ExposedCallType.String, typeof(string)),
            new Tuple<ExposedCallType, Type>(ExposedCallType.Void, typeof(void)),
            new Tuple<ExposedCallType, Type>(ExposedCallType.Object, typeof(Object)),
        };

        public enum ExposedCallType {
            Unknown,
            String,
            Int,
            Float,
            Object,
            Bool,
            Void
        }

        public static ExposedCallType FindType(MethodInfo methodInfo, out Type parameterType) {
            parameterType = null;
            if (methodInfo == null) {
                return ExposedCallType.Unknown;
            }

            var parameters = methodInfo.GetParameters();
            if (parameters.Length > 1) {
                return ExposedCallType.Unknown;
            }

            if (parameters.Length == 0) {
                return ExposedCallType.Void;
            }

            var info = parameters.Single();
            var type = info.ParameterType;
            parameterType = type;
            if (typeof(Object).IsAssignableFrom(type)) {
                return ExposedCallType.Object;
            }

            foreach (var (callType, allowedType) in AllowedTypes) {
                if (allowedType == type) {
                    return callType;
                }
            }

            return ExposedCallType.Unknown;
        }

        public ExposedCallType FindType(IExposedPropertyTable table, out Object obj, out Type parameterType) {
            return FindType(ExtractMethod(obj = target.Resolve(table)), out parameterType);
        }

        private MethodInfo ExtractMethod(Object o) {
            return o.GetType().GetMethod(method, BindingFlags.Public | BindingFlags.Instance);
        }

        public ExposedReference<Object> target;
        public string method;
        public int intParam;
        public float floatParam;
        public bool boolParam;
        public string stringParam;
        public ExposedReference<Object> objParam;

        public void Invoke(IExposedPropertyTable table) {
            var obj = target.Resolve(table);
            if (obj == null) {
                Debug.LogWarning($"Unable to find target {target} in table {table}");
                return;
            }

            var m = ExtractMethod(obj);
            object[] fp;

            switch (FindType(m, out _)) {
                case ExposedCallType.Unknown:
                    return;
                case ExposedCallType.String:
                    fp = new object[] {stringParam};
                    break;
                case ExposedCallType.Int:
                    fp = new object[] {intParam};
                    break;
                case ExposedCallType.Float:
                    fp = new object[] {floatParam};
                    break;
                case ExposedCallType.Object:
                    fp = new object[] {objParam.Resolve(table)};
                    break;
                case ExposedCallType.Void:
                    fp = new object[0];
                    break;
                case ExposedCallType.Bool:
                    fp = new object[] {boolParam};
                    break;
                default: {
                    return;
                }
            }

            m.Invoke(obj, fp);
        }
    }
}