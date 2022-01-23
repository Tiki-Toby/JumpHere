using Assets.Scripts.Level;
using Assets.Scripts.Players;
using Assets.Scripts.UI;
using HairyEngine.HairyCamera;
using HairyEngine.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class GameHandler : MonoBehaviour
    {
        private static GameHandler _instance;
        public static GameHandler Instance => _instance;
        [SerializeField] TrackBuilder trackBuilder;
        [SerializeField] RunnerController characterController;
        public AudioSourceManager soundManager;
        private Action onLoseAction; 
        public bool IsGame { get; private set; }
        public bool IsPause { get; private set; }
        public Vector3 CurDirection => trackBuilder.data.Direction;
        public bool IsGameProcess => IsGame && !IsPause;
        private Func<Vector3, float> DinstanceByDirection;
        private void Awake()
        {
            _instance = this;
        }
        void Start()
        {
            GenerateGame();
            SetPause(true);
        }

        public void SubscribeOnLose(Action onLoseSub)
        {
            onLoseAction += onLoseSub;
        }
        public void GenerateGame(bool isSecondChance = false)
        {
            StopAllCoroutines();
            IsGame = true;
            IsPause = false;
            if (isSecondChance)
            {
                characterController.Init(trackBuilder.data.PrevFloor.Center + Vector3.up, characterController.CurrentDirection);
                trackBuilder.StartGenerateWay(trackBuilder.data.PrevFloor.Center, new Vector3(characterController.CurrentDirection.x, 0, characterController.CurrentDirection.z));
                SessionData.isSecondChance = true;
            }
            else
            {
                SessionData.Init();
                characterController.Init(Vector3.zero, SettingsData.StartDirection);
                trackBuilder.StartGenerateWay(Vector3.zero, SettingsData.StartDirection);
                DinstanceByDirection = v => v.z;
            }
            CameraHandler.Instance.CenterOnTarget();
            StartCoroutine(Generator());
        }
        public void SetPause(bool isPause)
        {
            IsPause = isPause;
        }
        public void SwitchPause()
        {
            SetPause(!IsPause);
        }
        private void LateUpdate()
        {
            if (IsGameProcess)
            {
                characterController.MakeForceOffsetToCenterLine(trackBuilder.data.CurrentFloor.Center);
            }
        }

        void Update()
        {
            if (IsGameProcess)
            {
                SessionData.AddPath(characterController.CurrentVelocity * Time.deltaTime);
                if (characterController.PlayerBody.position.y + 10 < trackBuilder.data.CurrentFloor.floorObject?.position.y)
                    LoseAction();
            }
            if (Input.GetKeyDown(KeyCode.R))
                GenerateGame();
            if (Input.GetKeyDown(KeyCode.S))
                GenerateGame(true);
        }
        public void PlaceNewFloorForwardPlayer()
        {
            if(SessionData.extraPanelsCount > 0)
            {
                SessionData.TakeExtraPanel();
                trackBuilder.ReplaceNextFloor();
            }
        }
        public void LoseAction()
        {
            GamePanelManager.Instance.OpenLosePanel(SessionData.isSecondChance);
            ProfileData.Instance.Update((long)SessionData.timer, (long)SessionData.points);
            soundManager.PlayLoseSound();
            IsGame = false;
            onLoseAction?.Invoke();
        }
        IEnumerator Generator()
        {
            while (IsGame)
            {
                float time = Time.deltaTime + SettingsData.TimeForPlatformSelfFallen;
                yield return new WaitUntil(() => DinstanceByDirection(characterController.PlayerBody.position) > DinstanceByDirection(trackBuilder.data.NextFloor.Position));// || time < Time.deltaTime);

                trackBuilder.GeneratorNext();
                Vector3 delta = trackBuilder.data.NextFloor.Position - trackBuilder.data.CurrentFloor.Position;
                if (DinstanceByDirection(delta) == 0)
                {
                    yield return new WaitUntil(() => DinstanceByDirection(characterController.PlayerBody.position) > DinstanceByDirection(trackBuilder.data.CurrentFloor.Center));
                    characterController.TurnDirection();
                    //trackBuilder.data.TurnDirection();

                    if (delta.z == 0)
                        DinstanceByDirection = v => v.x;
                    else
                        DinstanceByDirection = v => v.z;
                }
            }
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}