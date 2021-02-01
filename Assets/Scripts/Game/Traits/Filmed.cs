using System.Collections;
using Cinemachine;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Master {
#if UNITY_EDITOR
#endif

    [TraitLocation("Misc")]
    public class Filmed : Trait {
        private GameObject obj;

        [AssetsOnly]
        public CinemachineVirtualCamera cameraPrefab;

        public CinemachineVirtualCamera sceneCamera;
        private Coroutine routine;
        public CinemachineVirtualCamera Camera { get; private set; }

        public override void Configure(TraitDependencies dependencies) {

            if (dependencies.Initialize) {
                var entity = dependencies.Entity;

                if (sceneCamera == null) {
                    Camera = Instantiate(cameraPrefab);
                    obj = Camera.gameObject;
                    //obj.hideFlags = HideFlags.NotEditable;
                    obj.name = $"EntityCamera({entity.name})";
                } else {
                    Camera = sceneCamera;
                }

                Camera.Follow = entity.transform;
            }
        }
        [ShowInInspector]
        public void MoveToEntity() {
            var pos = Owner.transform.position;
            var pCam = Camera;
            
            var transposer = pCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (transposer != null) {
                pos.z = -transposer.m_CameraDistance;
            }
            Camera.OnTargetObjectWarped(Owner.transform, pos);
        }
    }
}