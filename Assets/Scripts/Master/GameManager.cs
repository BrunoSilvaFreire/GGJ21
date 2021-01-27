using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine.Events;
namespace GGJ.Master {
    public class GameManager : Singleton<GameManager> {
        private Knowledgeable.Knowledge availableKnowledge;
        public UnityEvent onAvailableKnowledgeFound;

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
    }
}