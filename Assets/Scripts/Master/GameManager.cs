using System;
using System.Collections.Generic;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Master {
    public class GameManager : Singleton<GameManager> {
        [SerializeField]
        private Knowledgeable.Knowledge availableKnowledge;
        public UnityEvent onAvailableKnowledgeFound;
        public UnityEvent onAthenaPartsCollected;

        private PersistanceManager m_persistanceManager;
        private HashSet<int> m_athenaParts = new HashSet<int>();

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

        public bool AllAthenaPartsCollected() {
            return m_athenaParts.IsEmpty();
        }

        public void CollectAthenaPart(int id) {
            m_athenaParts.Remove(id);
            if (m_athenaParts.IsEmpty()) {
                onAthenaPartsCollected.Invoke();
            }
        }
        
        public void RegisterAthenaPart(int id) {
            m_athenaParts.Add(id);
        }
    }
}