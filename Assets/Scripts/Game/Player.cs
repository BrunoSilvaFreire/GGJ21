using System;
using Common;
using GGJ.Traits;
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

                }

                pawn = value;

                Configure(value);
            }
        }

        public void SetPawn(Entity entity) {
            Pawn = entity;
        }


        private void Configure(Entity value) {
            onPawnChanged.Invoke(value);
            if (value == null) {
                Debug.LogWarning("No Entity");
                return;
            }


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

        public TraitBind<A> Bind<A>(UnityAction<TraitBind<A>, A> configuration = null) where A : Trait {
            var bind = new TraitBind<A>();
            bind.PoolFrom(onPawnChanged);
            A current = null;
            if (pawn != null) {
                current = pawn.GetTrait<A>();
            }
            configuration?.Invoke(bind, current);
            bind.Set(pawn);
            return bind;
        }
        public bool Access<T>(out T trait) where T : Trait {
            if (pawn != null) {
                return pawn.Access(out trait);
            }
            trait = null;
            return false;
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