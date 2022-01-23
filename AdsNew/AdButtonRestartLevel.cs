using Assets.Scripts.Data;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrypts.AdsNew
{
    [RequireComponent(typeof(Button))]
    class AdButtonRestartLevel : MonoBehaviour
    {
        [SerializeField] PanelSpawnController panel;
        private void Start()
        {
            gameObject.SetActive(false);
            AdManager.Instance.LoadRewardedVideo(gameObject);
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Action finished = () =>
                {
                    GameHandler.Instance.GenerateGame(true);
                    panel.DisableOnHide();
                };
                Action skipped = () => { };
                AdManager.Instance.ShowAd(finished, skipped);
            });
        }
    }
}
