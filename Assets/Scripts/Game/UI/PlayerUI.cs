using GGJ.Master.UI.Knowledge;
using Lunari.Tsuki.Runtime.Misc;
using Lunari.Tsuki.Runtime.Singletons;
using UI;

namespace GGJ.Master.UI {
    public class PlayerUI : Singleton<PlayerUI> {
        public Clock clock = 0.25F;
        public KnowledgeEditor KnowledgeEditor;
        public View deathCurtains;
        private View last;
    }
}