using Assets.Scripts.Data;
using Assets.Scripts.Localization;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LanguageChanger : Localizator
    {
        [SerializeField] Image flag;
        [SerializeField] Sprite[] flagSprites = new Sprite[Enum.GetValues(typeof(Language)).Length];
        public override void Translate()
        {
            base.Translate();
            flag.sprite = flagSprites[(int)ProfileData.Instance.Lang];
        }
    }
}