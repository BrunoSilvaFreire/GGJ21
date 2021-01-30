using System;
using System.IO;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
using World;

namespace GGJ.Master {
    public class PersistanceManager : Singleton<PersistanceManager> {

        private string m_persistenceFilePath;
        private void Awake() {
            m_persistenceFilePath = Application.persistentDataPath + "persistence.json";
        }
        
        public void Load() {
            var data = LoadLastState();
            if (data == null) {
                return;
            }
            LoadPlayer(Player.Instance, data.player);
            LoadWorld(data.world);
        }
        
        private void LoadPlayer(Player player, PlayerData data) {
            player.transform.position = data.position;
            player.Pawn.transform.position = data.position;
            player.Pawn.GetTrait<Knowledgeable>().CurrentKnowledge = data.knowledge;
        }
        
        private void LoadWorld(WorldData data) {
            MapManager.Instance.SetActiveMap(data.activeMapPosition);
        }
        
        public void Save() {
            var data = new PersistantData();
            data.player = CreatePlayerData(Player.Instance);
            data.world = CreateWorldData();
            SaveCurrentState(data);
        }

        private PlayerData CreatePlayerData(Player player) {
            var pawn = player.Pawn;
            
            PlayerData data = new PlayerData();
            data.position = pawn.transform.position;
            data.knowledge = pawn.GetTrait<Knowledgeable>().CurrentKnowledge;
            return data;
        }
        
        private WorldData CreateWorldData() {
            var data = new WorldData();
            data.activeMapPosition = MapManager.Instance.GetActiveMap().Coordinates;
            return data;
        }

        private void SaveCurrentState(PersistantData data) {
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(m_persistenceFilePath,  json);
        }

        private PersistantData LoadLastState() {
            try {
                var json = File.ReadAllText(m_persistenceFilePath);
                var data = JsonUtility.FromJson<PersistantData>(json);
                return data;
            }
            catch (Exception e) {
                Debug.Log("Failed to load last save state: " + e);
                return null;
            }
        }
    }
}