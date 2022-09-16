using UnityEngine;
using DG.Tweening;
using ClubBusiness;
using ZestCore.Utility;

namespace ZestGames
{
    public class GameManager : MonoBehaviour
    {
        [Header("-- TESTING --")]
        [SerializeField, Range(0f, 5f)] private float gameSpeed = 1f;

        public static Enums.GameState GameState { get; private set; }
        public static Enums.GameEnd GameEnd { get; private set; }

        [Header("-- REFERENCES --")]
        private UiManager _uiManager;
        private LevelManager _levelManager;
        private SettingsManager _settingsManager;
        private DataManager _dataManager;
        private QueueManager _queueManager;
        private CustomerManager _customerManager;
        private AreaManager _areaManager;
        private ClubManager _clubManager;

        private void Init()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1250, 50);
            Application.targetFrameRate = 240;

            GameState = Enums.GameState.WaitingToStart;
            GameEnd = Enums.GameEnd.None;

            _levelManager = GetComponent<LevelManager>();
            _levelManager.Init(this);
            _dataManager = GetComponent<DataManager>();
            _dataManager.Init(this);
            _settingsManager = GetComponent<SettingsManager>();
            _settingsManager.Init(this);
            _uiManager = GetComponent<UiManager>();
            _uiManager.Init(this);
            _queueManager = GetComponent<QueueManager>();
            _queueManager.Init(this);
            _clubManager = GetComponent<ClubManager>();
            _clubManager.Init(this);
            _customerManager = GetComponent<CustomerManager>();
            Delayer.DoActionAfterDelay(this, 0.2f, () => _customerManager.Init(this));
            _areaManager = GetComponent<AreaManager>();
            _areaManager.Init(this);

            UiEvents.OnUpdateCollectableText?.Invoke(DataManager.TotalMoney);
            UiEvents.OnUpdateLevelText?.Invoke(LevelHandler.Level);
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnGameEnd += HandleGameEnd;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnGameEnd -= HandleGameEnd;

            DOTween.KillAll();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Time.timeScale = gameSpeed;
#endif
        }

        private void HandleGameStart()
        {
            GameState = Enums.GameState.Started;
        }

        private void HandleGameEnd(Enums.GameEnd gameEnd)
        {
            GameEnd = gameEnd;

            if (gameEnd == Enums.GameEnd.Success)
                GameEvents.OnLevelSuccess?.Invoke();
            else if (gameEnd == Enums.GameEnd.Fail)
                GameEvents.OnLevelFail?.Invoke();
        }
    }
}
