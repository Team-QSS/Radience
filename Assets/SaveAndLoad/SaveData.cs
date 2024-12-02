using System;
using System.Collections.Generic;
using System.IO;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveAndLoad
{
    public static class SaveData
    {
        public static PlayerStatus playerStatus = new();
        private static readonly string SavePath = Application.persistentDataPath + "/PlayerStatus.json";

        public static void SaveScene()
        {
            playerStatus.stageTag = SceneManager.GetActiveScene().buildIndex;
            SaveToJson();
        }

        public static void SavePreviousScene()
        {
            playerStatus.lastLocation = new Vector2(0, 0);
            playerStatus.boneFireLocation = new Vector2(0, 0);
            playerStatus.stageTag = SceneManager.GetActiveScene().buildIndex+1;
            SaveToJson();
        }
        
        public static bool HasAbilities(string ability)
        {
            LoadFromJson();
            return playerStatus.playerAbility.Contains(ability);
        }

        public static void SetAbilities(string ability)
        {
            LoadFromJson();
            if (playerStatus.playerAbility.Contains(ability)) return;
            playerStatus.playerAbility.Add(ability);
            SaveToJson();
        }
        
        public static void LoadScene()
        {
            LoadFromJson();
            if (playerStatus.stageTag == 0) return;
            SceneManager.LoadScene(playerStatus.stageTag);
        }

        public static void LocatePosition()
        {
            LoadFromJson();
            if (playerStatus.lastLocation != new Vector2(0, 0)) return;
            switch (playerStatus.stageTag)
            {
                case 2:
                    playerStatus.lastLocation = new Vector2(-21.94f,-0.54f);
                    break;
                case 3:
                    break;
            }

        }
        public static void SaveToJson() => File.WriteAllText(SavePath,JsonUtility.ToJson(playerStatus));

        public static void LoadFromJson()
        {
            if (File.Exists(SavePath)) playerStatus = JsonUtility.FromJson<PlayerStatus>(File.ReadAllText(SavePath));
        }

        public static void DeleteInJson()
        {
            playerStatus = new PlayerStatus();
            SaveToJson();
        }
    }
    
    [Serializable]
    public class PlayerStatus
    {
        public int stageTag;
        public Vector2 lastLocation;
        public Vector2 boneFireLocation;
        public List<string> playerAbility;
    }
}