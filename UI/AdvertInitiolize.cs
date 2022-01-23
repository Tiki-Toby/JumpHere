using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    class AdvertInitiolize : MonoBehaviour
    {
        private void OnEnable()
        {
            if (!Advertisement.IsReady())
            {
                GetComponent<Button>().interactable = false;
                StartCoroutine(WaitForAd());
            }
        }
        IEnumerator WaitForAd()
        {
            yield return new WaitUntil(() => Advertisement.IsReady());
            GetComponent<Button>().interactable = true;
        }
    }
}
