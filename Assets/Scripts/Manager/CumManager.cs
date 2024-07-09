using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CumBath.Manager
{
    public class CumManager : MonoBehaviour
    {
        public static CumManager Instance { private set; get; }

        [SerializeField]
        private TMP_Text _mlText;

        [SerializeField]
        private Image _bath;

        [SerializeField]
        private Sprite[] _cumLayers;

        private float _totalMl;
        private float _currentMl;

        private int CumIndex => Mathf.Clamp(Mathf.FloorToInt(_totalMl / 100f) - 1, 0, _cumLayers.Length - 1);

        public bool IsBathFull => CumIndex == _cumLayers.Length - 1;

        private void Awake()
        {
            Instance = this;
        }

        private void UpdateUI()
        {
            _mlText.text = $"{(int)(_totalMl + _currentMl)} mL";
        }

        public void IncreaseCurrent(float amount)
        {
            _currentMl += amount;
            UpdateUI();
        }

        public void SaveCurrent()
        {
            _totalMl += _currentMl;
            _currentMl = 0f;

            if (_totalMl >= 100f)
            {
                _bath.color = Color.white;
                _bath.sprite = _cumLayers[CumIndex];
            }
        }

        public void CancelCurrent()
        {
            _currentMl = 0f;
            UpdateUI();
        }
    }
}