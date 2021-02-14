
using GGJ.Master;
using GGJ.Traits;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ.Collectables {
    public class AthenaPart : Collectable {

        public int id;
        public bool follow = true;
        public bool isPresent = false;
        private static Vector2 speedRange = new Vector2(0.05f, 0.15f);
        
        private bool m_savedActive, m_savedFollow;
        private Transform m_savedParent;
        private Vector3 m_savedPosition;
        private Collector m_savedCollector, m_collector;
        private Animator m_animator;
        private float m_speed;
        
        protected override CollectionAction ProcessCollection(Entity entity) {
            if (entity.Access<Collector>(out var collector) && m_collector == null) {
                collector.Collect(this);
                transform.parent = collector.Owner.transform.parent;
                m_collector = collector;
                if (isPresent) {
                    m_animator.SetTrigger("open");
                }
                return CollectionAction.Ok;
            }
            return CollectionAction.None;
        }

        public void OnPresentAnimationCompleted() {
            follow = true;
        }

        private void Awake() {
            GameManager.Instance.RegisterAthenaPart(id);
            m_speed = Random.Range(speedRange.x, speedRange.y);
            m_animator = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate() {
            if (m_collector && follow) {
                transform.position = Vector3.Lerp(transform.position, m_collector.Owner.transform.position, m_speed);
            }
        }

        private void OnLoad() {
            follow = m_savedFollow;
            if (m_collector != m_savedCollector) {
                GameManager.Instance.RegisterAthenaPart(id);
                if (isPresent) {
                    m_animator.SetTrigger("idle");
                }
            }
            transform.position = m_savedPosition;
            transform.parent = m_savedParent;
            m_collector = m_savedCollector;
            gameObject.SetActive(m_savedActive);
        }
        
        private void OnSave() {
            m_savedFollow = follow ;
            m_savedParent = transform.parent;
            m_savedPosition = transform.position;
            m_savedCollector = m_collector;
            m_savedActive = gameObject.activeSelf;
        }
    }
}
