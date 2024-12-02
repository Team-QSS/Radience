using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimelinePerStage.title
{
    public class NextSceneTitle : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.SetAsBackgroundMusicInstance("Audio/The_Moonless_Forest", true);
            SceneManager.LoadScene("Stage1");
        }
    }
}

