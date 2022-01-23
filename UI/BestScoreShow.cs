using Assets.Scripts.Data;
using Assets.Scripts.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    class BestScoreShow : Localizator
    {
        string bestScore;
        public void OnEnable()
        {
            textField.text = bestScore + ": " +ProfileData.Instance.bestScore;
        }
        public override void Translate()
        {
            bestScore = LocalizationManager.Instance.GetText(translateName);
            textField.text = bestScore + ": " + ProfileData.Instance.bestScore;
        }
    }
}
