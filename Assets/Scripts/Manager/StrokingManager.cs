using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CumBath.Manager
{
    public class StrokingManager : MonoBehaviour
    {
        public static StrokingManager Instance { private set; get; }

        [SerializeField]
        private RectTransform _mainContainer, _fish, _cursor, _overallProgress;

        [SerializeField]
        private GameObject _strokeButton;

        [SerializeField]
        private Image _eyesImage;

        [SerializeField]
        private Sprite[] _eyes;

        [SerializeField]
        private Sprite _bonusEyes;

        [SerializeField]
        private Image[] _characters;

        [SerializeField]
        private Sprite[] _flaccidSprites;

        [SerializeField]
        private Image _cumImage;

        [SerializeField]
        private Sprite[] _cumSprites;

        [SerializeField]
        private Animator _handAnim;

        [SerializeField]
        private Image _handImage;

        [SerializeField]
        private RuntimeAnimatorController[] _handClips;

        [SerializeField]
        private Sprite[] _handSprites;

        private float _height;

        private float _max;

        private float _target;

        /// <summary>
        /// Our current timer, need to reach <see cref="_maxTimer"/> to win the minigame
        /// </summary>
        private float _timer;

        /// <summary>
        /// When reaching this duration, we win the minigame
        /// If we reach minus this duration we loose
        /// </summary>
        private float _maxTimer;

        /// <summary>
        /// Fish the cursor representing the fish is going by in the minigame
        /// </summary>
        const float _fishBaseSpeed = 300f;

        private bool _isActive;

        private int _peniesesLeft = 4;
        private int _cumLeft = 4;

        private float[] _speeds = { 1f, 1.5f, 2.5f, 4f };

        public bool IsBonusLevel { private set; get; }

        private void Awake()
        {
            Instance = this;
            _eyesImage.sprite = _eyes[0];
            _handAnim.runtimeAnimatorController = _handClips[0];
            _handImage.sprite = _handSprites[0];
        }

        public void StartStroking()
        {
            _strokeButton.SetActive(false);
            _mainContainer.gameObject.SetActive(true);
            _handAnim.SetBool("Masturbating", true);
            StartCoroutine(StartMinigame());

            _maxTimer = 3f;
            _height = _mainContainer.rect.height;
            _max = -_height + _fish.rect.height;
            _target = Random.Range(0f, _max);
            _timer = 0f;
            _overallProgress.localScale = new(1f, .5f, 1f);
            _cursor.position = new(_cursor.position.x, _height / 2f + _cursor.rect.height);
        }

        private IEnumerator StartMinigame()
        {
            yield return new WaitForSeconds(1f);
            _isActive = true;
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            var dir = Time.deltaTime * (_fish.anchoredPosition.y < _target ? 1f : -1f) * _fishBaseSpeed;
            _fish.anchoredPosition = new(_fish.anchoredPosition.x, _fish.anchoredPosition.y + dir);

            if (Mathf.Abs(_fish.anchoredPosition.y - _target) < 10f)
            {
                _target = Random.Range(0f, _max);
            }

            var pos = Mouse.current.position.ReadValue().y;
            _cursor.position = new(_cursor.position.x, pos + _cursor.rect.height / 2f);

            _timer += (_fish.anchoredPosition.y > _cursor.anchoredPosition.y || _fish.anchoredPosition.y + _cursor.rect.height - _fish.rect.height < _cursor.anchoredPosition.y ? -1f : 2f)
                * Time.deltaTime
                * _speeds[4 - _cumLeft];
            _maxTimer -= Time.deltaTime * .25f;
            _handAnim.speed = 1f + (3f - _maxTimer) / 3f;

            CumManager.Instance.IncreaseCurrent(Time.deltaTime * 10f);

            if (Mathf.Abs(_timer) >= _maxTimer)
            {
                _isActive = false;
                _mainContainer.gameObject.SetActive(false);

                StartCoroutine(CumAndProgress());
            }
            else
            {
                var size = ((_timer / _maxTimer) + 1f) / 2f;
                _overallProgress.localScale = new(1f, size, 1f);
            }
        }

        private IEnumerator CumAndProgress()
        {
            _handAnim.SetBool("Masturbating", false);
            if (_timer > 0f)
            {
                CumManager.Instance.SaveCurrent();

                if (!IsBonusLevel)
                {
                    _cumImage.gameObject.SetActive(true);
                    _cumImage.sprite = _cumSprites[4 - _peniesesLeft];
                    yield return new WaitForSeconds(1f);
                    _cumImage.gameObject.SetActive(false);
                }
            }
            else
            {
                CumManager.Instance.CancelCurrent();
            }

            _cumLeft--;
            if (_cumLeft == 0 && !IsBonusLevel)
            {
                _characters[4 - _peniesesLeft].sprite = _flaccidSprites[4 - _peniesesLeft];
                _peniesesLeft--;
                _cumLeft = 4;
            }

            if (_peniesesLeft > 0)
            {
                _handAnim.runtimeAnimatorController = _handClips[4 - _peniesesLeft];
                _handImage.sprite = _handSprites[4 - _peniesesLeft];
                _strokeButton.SetActive(true);
                _eyesImage.sprite = _eyes[4 - _peniesesLeft];
            }
            else
            {
                if (!IsBonusLevel && CumManager.Instance.IsBathFull)
                {
                    IsBonusLevel = true;
                    _handAnim.gameObject.SetActive(false);
                    _eyesImage.sprite = _bonusEyes;
                    _strokeButton.SetActive(true);
                }
                else
                {
                    CumManager.Instance.SetVictoryLevel();
                    StartCoroutine(ChangeScene());
                }
            }
        }

        public IEnumerator ChangeScene()
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Victory");
        }
    }
}