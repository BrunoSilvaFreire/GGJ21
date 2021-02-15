using GGJ.Persistence;
using GGJ.Props.Traits;
using GGJ.Traits.Animation;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Props.Common {
    public class ElectricDoor : Trait, IPersistantLegacy {

        private bool m_savedActive;
        private Vector3 m_savedScale;

        private PersistenceManager m_manager;

        private AnimatorBinder m_binder;

        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out m_binder);
        }

        private void Open(Key key) {
            m_binder.Animator.SetTrigger("open");
            key.Consume();
        }
        private void OnCollisionEnter2D(Collision2D collision) {
            var entity = collision.collider.GetComponentInParent<Entity>();
            if (entity == null) {
                return;
            }
            if (!entity.Access(out Collector collector)) {
                return;
            }
            var key = collector.FindCollectable<Key>();
            if (key != null) {
                Open(key);
                collector.Remove(key);   
            }
        }


        public void ConfigurePersistance(PersistenceManager manager) {
            m_manager = manager;
            m_manager.onLoad.AddListener(OnLoad);
            m_manager.onSave.AddListener(OnSave);
            OnSave();
        }

        public void OnDestroy() {
            if (m_manager) {
                m_manager.onLoad.RemoveListener(OnLoad);
                m_manager.onSave.RemoveListener(OnSave);
            }
        }

        private void OnSave() {
            m_savedActive = gameObject.activeSelf;
            m_savedScale = transform.localScale;
        }

        private void OnLoad() {
            gameObject.SetActive(m_savedActive);
            transform.localScale = m_savedScale;
            m_binder.Animator.SetBool("open", !m_savedActive);
        }
    }
}