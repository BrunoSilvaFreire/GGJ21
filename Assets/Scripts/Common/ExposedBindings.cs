using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GGJ.Common {
    public class ExposedBindings : MonoBehaviour, IExposedPropertyTable {
        [Serializable]
        public class ExposedBinding {
            [SerializeField, DisplayAsString]
            private PropertyName key;

            [SerializeField]
            private Object value;

            public ExposedBinding(PropertyName key, Object value) {
                this.key = key;
                this.value = value;
            }

            public PropertyName Key => key;

            public Object Value {
                get => value;
                set => this.value = value;
            }
        }

        public List<ExposedBinding> bindings;

        public void SetReferenceValue(PropertyName id, Object value) {
            var binding = bindings.FirstOrDefault(b => b.Key == id);
            if (binding == null) {
                binding = new ExposedBinding(id, value);
                bindings.Add(binding);
            } else {
                binding.Value = value;
            }
        }

        public Object GetReferenceValue(PropertyName id, out bool idValid) {
            var found = bindings.FirstOrDefault(b => b.Key == id);
            idValid = found != null;
            return found?.Value;
        }

        public void ClearReferenceValue(PropertyName id) {
            bindings.RemoveAll(binding => binding.Key == id);
        }
    }
}