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

        private PersistanceManager m_persistanceManager;

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
                if (m_persistanceManager == null) {
                    m_persistanceManager = FindObjectOfType<PersistanceManager>();
                }
                m_persistanceManager.Restart();
            }
        }
    }
}