using System;
using GGJ.Traits.Knowledge;
using UnityEngine;

namespace GGJ.Master {

    [Serializable]
    public class PlayerData {
        public Vector2 position;
        public Knowledgeable.Knowledge knowledge;
    }
    
    [Serializable]
    public class PersistantData {
        public PlayerData player;
    }
}