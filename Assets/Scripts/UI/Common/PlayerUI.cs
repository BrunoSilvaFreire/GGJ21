using GGJ.UI.Common.Knowledge;
using Lunari.Tsuki.Runtime.Singletons;
namespace GGJ.UI.Common {
    public class PlayerUI : Singleton<PlayerUI> {
        public KnowledgeEditor KnowledgeEditor;
        public View deathCurtains;
        private View last;
    }
}