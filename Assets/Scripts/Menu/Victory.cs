using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CumBath.Menu
{
    public class Victory : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Sprite[] _sprites;

        [SerializeField]
        private TMP_Text _infoText;

        private void Awake()
        {
            _image.sprite = _sprites[Mathf.Clamp(StaticData.VictoryIndex, 0, _sprites.Length - 1)];
            _infoText.text = $"Total cum: {(int)StaticData.CumAmount} cL";
        }

        public void Replay()
        {
            SceneManager.LoadScene("Main");
        }
    }
}