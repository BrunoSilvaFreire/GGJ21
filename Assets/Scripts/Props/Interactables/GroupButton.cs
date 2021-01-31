using GGJ.Master;
using GGJ.Props;
using GGJ.Traits;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using UnityEngine;
using World;

namespace Props.Interactables {

    public class GroupButton : Interactable, ITiledObject, IButtonGroup, IPersistant {

        [SerializeField] private int m_buttonGroupId;

        private bool m_pressed, m_savedPressed;
        private ButtonGroupManager m_manager;
        private PersistanceManager m_persistenceManager;
        private SpriteRenderer renderer;
        private AnimatorBinder binder;
        
        public void Setup(ObjectData data) {
            m_buttonGroupId = PropertyData.GetInt(data.properties, "id");
        }
        public override void Configure(TraitDependencies dependencies) {
            renderer = dependencies.RequiresComponent<SpriteRenderer>("View");
            dependencies.Access(out binder);
        }
        public override void Interact(Entity entity) {
            if (entity != Player.Instance.Pawn) {
                return;
            }
            m_manager.RemoveFromButtonGroup(m_buttonGroupId);
            m_pressed = true;
            binder.Animator.SetTrigger("press");
        }

        public void ConfigureGroup(ButtonGroupManager manager) {
            m_manager = manager;
            var group = m_manager.AddToButtonGroup(m_buttonGroupId);
            renderer.color = group.color;
        }

        public void ConfigurePersistance(PersistanceManager manager) {
            m_persistenceManager = manager;
            m_persistenceManager.onLoad.AddListener(OnLoad);
            m_persistenceManager.onSave.AddListener(OnSave);
            OnSave();
        }

        public void OnDestroy() {
            if (m_persistenceManager) {
                m_persistenceManager.onLoad.RemoveListener(OnLoad);
                m_persistenceManager.onSave.RemoveListener(OnSave);
            }
        }

        private void OnSave() {
            m_savedPressed = m_pressed;
        }

        private void OnLoad() {
            if (m_pressed != m_savedPressed) {
                m_manager.AddToButtonGroup(m_buttonGroupId);
                binder.Animator.SetTrigger("idle");
            }
            m_pressed = m_savedPressed;
        }
    }
}