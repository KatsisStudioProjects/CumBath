using UnityEngine;
using UnityEngine.UI;

namespace CumBath.Menu
{
    public class Victory : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Sprite[] _sprites;

        private void Awake()
        {
            _image.sprite = _sprites[Mathf.Clamp(StaticData.VictoryIndex, 0, _sprites.Length - 1)];
        }
    }
}