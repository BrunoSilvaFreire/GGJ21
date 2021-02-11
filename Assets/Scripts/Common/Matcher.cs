using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Common {
    public abstract class Matcher<T, TSelf> where TSelf : Matcher<T, TSelf> {

        [HideIf(nameof(IsSelf)), SerializeField]
        public List<TSelf> children;

        [ShowIf(nameof(IsSelf))]
        public T data;

        public BitMode mode;

        private bool IsSelf() {
            return mode == BitMode.Self;
        }

        public enum BitMode {
            Self,
            All,
            Any,
            None
        }

        public bool IsMet(T value) {
            switch (mode) {
                case BitMode.All:
                    return children.All(node => node.IsMet(value));
                case BitMode.Any:
                    return children.Any(node => node.IsMet(value));
                case BitMode.None:
                    return !children.Any(node => node.IsMet(value));
                case BitMode.Self:
                    return Matches(value, data);
                default:
                    return false;
            }
        }
        protected abstract bool Matches(T value, T required);
    }
}