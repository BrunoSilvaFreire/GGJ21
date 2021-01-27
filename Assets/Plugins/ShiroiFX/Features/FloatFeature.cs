﻿using UnityEngine;

namespace Shiroi.FX.Features {
    public class FloatFeature : EffectFeature {
        private float value;

        public FloatFeature(float value, params PropertyName[] tags) : base(tags) {
            this.value = value;
        }

        public float Value {
            get {
                return value;
            }
        }
    }
}