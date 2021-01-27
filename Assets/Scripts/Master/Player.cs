using System;
using Common;
using Input;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Master {
    public class Player : Singleton<Player> {
        [SerializeField, HideInInspector]
        private Entity pawn;

        public EntityEvent onPawnChanged;
        public InputSource playerSource;
        public int pawnCameraPriority = 10;
        private int cachedPriority;
        private InputSource cachedSource;

        [ShowInInspector]
        public Entity Pawn {
            get => pawn;
            set {
                if (value == pawn) {
                    return;
                }
                if (pawn != null || (pawn != null && pawn != value)) {
                    if (pawn.Access(out EntityInput input)) {
                        input.Reset();
                        input.source = cachedSource;
                    }

                    if (pawn.Access(out Filmed filmed)) {
                        filmed.Camera.Priority = cachedPriority;
                    }

                    TrySetMouseAnchor(playerSource, null);
                }

                pawn = value;

                Configure(value);
            }
        }

        public void SetPawn(Entity entity) {
            Pawn = entity;
        }
        private static void TrySetMouseAnchor(InputSource playerSource, Transform anchor) {
            if (playerSource == null) {
                return;
            }

            if (playerSource is NewInputSource s) {
                s.mouseAnchor = anchor;
            }

            if (playerSource is OverridableInputSource other) {
                TrySetMouseAnchor(other.delegateSource, anchor);
            }
        }

        private void Configure(Entity value) {
            onPawnChanged.Invoke(value);
            if (value == null) {
                Debug.LogWarning("No Entity");
                return;
            }

            TrySetMouseAnchor(playerSource, value.transform);


            if (value.Access(out EntityInput input)) {
                cachedSource = input.source;
                input.source = playerSource;
            }

            if (value.Access(out Filmed filmed)) {
                cachedPriority = filmed.Camera.Priority;
                filmed.Camera.Priority = pawnCameraPriority;
            }
        }


        private void Start() {
            if (Pawn != null) {
                Configure(Pawn);
            }
        }

        public void Bind<A>(UnityAction<A> onAttached) where A : Trait {
            if (pawn != null) {
                if (pawn.Access(out A trait)) {
                    onAttached(trait);
                }
            }
            onPawnChanged.AddListener(delegate(Entity arg0)
            {
                if (arg0.Access(out A trait)) {
                    onAttached(trait);
                }
            });
        }
    }

    [Serializable]
    public class IsPlayerFilter : Filter {
        public override bool Allowed(Collider2D collider) {
            return collider.GetComponentInParent<Entity>() == Player.Instance.Pawn;
        }
    }

    [Serializable]
    public class IsOnEntityFilter : Filter {
        public override bool Allowed(Collider2D collider) {
            var entity = collider.GetComponent<Entity>();

            if (entity == null) {
                return false;
            }

            return entity.gameObject == collider.gameObject;
        }
    }
}