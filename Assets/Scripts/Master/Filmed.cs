using Cinemachine;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Master {
#if UNITY_EDITOR
#endif


    [TraitLocation(TraitLocations.Root)]
    public class Filmed : Trait {
        private GameObject obj;

        [AssetsOnly]
        public CinemachineVirtualCamera cameraPrefab;

        public CinemachineVirtualCamera sceneCamera;
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
    }
}