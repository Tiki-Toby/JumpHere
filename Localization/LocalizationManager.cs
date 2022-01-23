using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private static LocalizationManager _instance;
        public static LocalizationManager Instance => _instance;
        [SerializeField] LanguagePreset[] _presets;
        [SerializeField] Dictionary<string, string> _texts;

        private void Awake()
        {
            _instance = this;
            Localize();
        }

        public void Localize()
        {
            int id = (int)ProfileData.Instance.Lang;
            _texts = new Dictionary<string, string>();
            foreach(LanguagePreset preset in _presets)
                foreach(Localization translate in preset.translates)
                    _texts.Add(translate.name, translate.text[id]);
        }
        public void SetNextLanguage()
        {
            ProfileData.Instance.Lang = (Language)(((int)ProfileData.Instance.Lang + 1) % Enum.GetValues(typeof(Language)).Length);
            Localize();
            Localizator[] localizators = GameObject.FindObjectsOfType<Localizator>();   
            foreach (Localizator localizator in localizators)
                localizator.Translate();
        }
        public string GetText(string name) => _texts[name];
    }
}
