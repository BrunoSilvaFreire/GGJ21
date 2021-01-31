using DG.Tweening;
using GGJ.Props;
using Lunari.Tsuki.Entities;
using UnityEngine;
using World;

namespace Props.Interactables {
    public class GroupDoor : Trait, ITiledObject, IButtonGroup {

        private int m_buttonGroupId;
        public void Setup(ObjectData data) {
            m_buttonGroupId = PropertyData.GetInt(data.properties, "id");
        }

        public void ConfigureGroup() {
            ButtonGroupManager.Instance.AddListenerToButtonGroup(m_buttonGroupId, OnGroupCleared);
        }

        public void OnGroupCleared() {
            transform.DOScale(Vector3.zero, 1);
        }
    }
}