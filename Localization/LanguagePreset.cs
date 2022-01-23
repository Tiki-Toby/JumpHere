using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Localization
{
    public enum Language
    {
        English = 0,
        Russian = 1
    }
    [Serializable]
    public struct Localization
    {
        public string name;
        public string[] text;
    }
    [CreateAssetMenu(fileName = "LanguagePreset", menuName = "ScriptableObjects/LanguagePreset", order = 2)]
    public class LanguagePreset : ScriptableObject
    {
        [Header("Translates")]
        public Localization[] translates;
    }
}