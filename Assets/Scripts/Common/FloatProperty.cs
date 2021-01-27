using System;
using System.Collections.Generic;
using UnityEngine;
namespace Common {
    public class MultiplierHandle {
        public float value;

        public MultiplierHandle() {
            value = 1;
        }
    }

    [Serializable]
    public class FloatProperty {
        [SerializeField]
        private float baseValue = 1;

        private List<MultiplierHandle> multipliers;
        public float masterMultiplier = 1;

        public float Value => baseValue * Multiplier;

        public MultiplierHandle AddMultiplier(float initialValue = 1) {
            if (multipliers == null) {
                multipliers = new List<MultiplierHandle>();
            }

            var handle = new MultiplierHandle {
                value = initialValue
            };
            multipliers.Add(handle);
            return handle;
        }

        public void RemoveMultiplier(MultiplierHandle handle) {
            multipliers?.Remove(handle);
        }

        public float BaseValue {
            get => baseValue;
            set => baseValue = value;
        }

        public float Multiplier {
            get {
                float result = masterMultiplier;
                if (multipliers != null) {
                    foreach (var multiplierHandle in multipliers) {
                        result *= multiplierHandle.value;
                    }
                }

                return result;
            }
        }


        public static implicit operator float(FloatProperty property) {
            return property.Value;
        }

        public static implicit operator FloatProperty(float value) {
            return new FloatProperty {
                baseValue = value,
                masterMultiplier = 1
            };
        }
    }
}