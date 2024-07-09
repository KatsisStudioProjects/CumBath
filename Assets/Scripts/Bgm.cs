using UnityEngine;

namespace CumBath
{
    public class Bgm : MonoBehaviour
    {
        public static Bgm Instance { private set; get; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}