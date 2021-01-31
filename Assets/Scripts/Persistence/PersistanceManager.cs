using System;
using System.Collections;
using System.IO;
using Cinemachine;
using GGJ.Master.UI;
using GGJ.Traits.Combat;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
using World;

namespace GGJ.Master {
    public class PersistanceManager : Singleton<PersistanceManager> {

        public float timeUntilClose = .5F;
        public float timeUntilLoad = .5F;
        public float minWaitTime = .5F;
        private string m_persistenceFilePath;
        private Coroutine restartRoutine;
        private void Awake() {
            m_persistenceFilePath = Application.persistentDataPath + "persistence.json";
        }
        public void Restart() {
            Coroutines.ReplaceCoroutine(ref restartRoutine, this, RestartRoutine());
        }
        private IEnumerator RestartRoutine() {
            var ui = PlayerUI.Instance;
            yield return new WaitForSecondsRealtime(timeUntilClose);
            var time = Time.time;
            ui.deathCurtains.Show();
            yield return new WaitForSecondsRealtime(timeUntilLoad);
            Load();
            Time.timeScale = 1;
            var remaining = minWaitTime - (Time.time - time);
            if (remaining > 0) {
                yield return new WaitForSecondsRealtime(remaining);
            }
            var p = Player.Instance.Pawn;
            if (p != null) {
                if (p.Access(out Living living)) {
                    living.Alive = true;
                }
                var cam = Player.Instance.camera;
                var pos = p.transform.position;
                if (p.Access(out Filmed filmed)) {
                    var transposer = filmed.Camera.GetCinemachineComponent<CinemachineFramingTransposer>();
                    if (transposer != null) {
                        pos.z = -transposer.m_CameraDistance;
                    }
                }
                cam.transform.position = pos;
            }
            ui.deathCurtains.Hide();
        }

        private void Load() {
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
            File.WriteAllText(m_persistenceFilePath, json);
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