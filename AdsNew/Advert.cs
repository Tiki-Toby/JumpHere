using Assets.Scripts.Data;
using Assets.Scripts.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts.AdsNew
{

    public class Advert : MonoBehaviour
    {
        private class AdvertReward : IUnityAdsListener
        {
            Action rewardNotify, nonrewardNotify;
            public AdvertReward(string gameID, bool isTest)
            {
                Advertisement.AddListener(this);
                Advertisement.Initialize(gameID, isTest);
            }

            public void ShowAds(Action nonrewardNotify, Action rewardNotify, string myPlacementId)
            {
                this.rewardNotify = rewardNotify;
                this.nonrewardNotify = nonrewardNotify;
                Advertisement.Show(myPlacementId);
            }
            public void OnUnityAdsReady(string placementId)
            {
            }

            public void OnUnityAdsDidError(string message)
            {

            }

            public void OnUnityAdsDidStart(string placementId)
            {
            }

            public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
            {
                switch (showResult)
                {
                    case ShowResult.Finished:
                        rewardNotify.Invoke();
                        break;
                }
            }
        }

        private const string gameId = "4433875";
        public bool isTest;

        private AdvertReward advert;

        private void Start()
        {
            advert = new AdvertReward(gameId, isTest);
        }
        public void RestartGame(PanelSpawnController panel)
        {
            advert.ShowAds(() => { }, () => { 
                GameHandler.Instance.GenerateGame(true);
                GameHandler.Instance.soundManager.StartGameMusic();
                panel.DisableOnHide();
            }, "Rewarded_Android");
        }
    }
}