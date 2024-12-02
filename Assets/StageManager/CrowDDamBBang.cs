using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StageManager
{
    public class CrowDDamBBang : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.SetAsBackgroundMusicInstance("Audio/Radience", true);
            SceneManager.LoadScene("CrowChacing");
        }
    }
}
