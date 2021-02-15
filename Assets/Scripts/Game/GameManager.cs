using System.Collections.Generic;
using GGJ.Persistence;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Game {
    public class GameManager : Singleton<GameManager> {
        [SerializeField]
        private Knowledge availableKnowledge;
        public UnityEvent onAvailableKnowledgeFound;
        public UnityEvent onAthenaPartsCollected;

        private HashSet<int> m_athenaParts = new HashSet<int>();

        [ShowInInspector]
        public Knowledge AvailableKnowledge {
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
                PersistenceManager.Instance.Restart();
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