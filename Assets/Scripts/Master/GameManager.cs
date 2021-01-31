using System;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Master {
    public class GameManager : Singleton<GameManager> {
        [SerializeField]
        private Knowledgeable.Knowledge availableKnowledge;
        public UnityEvent onAvailableKnowledgeFound;

        [ShowInInspector]
        public Knowledgeable.Knowledge AvailableKnowledge {
            get => availableKnowledge;
            set {
                if (value == availableKnowledge) {
                    return;
                }
                availableKnowledge = value;
                onAvailableKnowledgeFound.Invoke();
            }
        }

        private void Update() {
            if (Player.Instance.playerSource.GetReset()) {
                PersistanceManager.Instance.Load();
            }
        }
    }
}