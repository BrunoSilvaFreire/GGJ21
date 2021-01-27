using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using Shiroi.FX.Features;
using Sirenix.Utilities;
using UnityEngine;

namespace Shiroi.FX.Effects {
    [Serializable]
    public sealed class EffectContext {
        private readonly MonoBehaviour host;
        private readonly EffectFeature[] features;

        public EffectContext(MonoBehaviour host, params EffectFeature[] features) {
            this.features = features;
            this.host = host;
        }

        public MonoBehaviour Host {
            get { return host; }
        }

        public Coroutine StartCoroutine(IEnumerator routine) {
            if (!host.isActiveAndEnabled) {
                return null;
            }

            return host.StartCoroutine(routine);
        }

        [CanBeNull]
        public F GetOptionalFeature<F>() where F : EffectFeature {
            return features.OfType<F>().FirstOrDefault();
        }

        [NotNull]
        public F GetRequiredFeature<F>() where F : EffectFeature {
            foreach (var feature in features) {
                var f = feature as F;
                if (f != null) {
                    return f;
                }
            }

            throw new FeatureNotPresentException<F>();
        }

        [CanBeNull]
        public F GetOptinalFeatureWithTags<F>(params PropertyName[] tags) where F : EffectFeature {
            foreach (var feature in features) {
                if (!(feature is F f)) {
                    continue;
                }

                var available = f.Tags.ToHashSet();
                if (tags.All(name => available.Contains(name))) {
                    return f;
                }
            }

            return null;
        }

        [NotNull]
        public F GetRequiredFeatureWithTags<F>(params PropertyName[] tags) where F : EffectFeature {
            foreach (var feature in features) {
                var f = feature as F;
                if (f != null) {
                    return f;
                }
            }

            throw new FeatureNotPresentException<F>();
        }
    }
}