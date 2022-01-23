using Assets.Scripts.Localization;
using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    class ProfileData
    {
        private static ProfileData _instance;
        public static ProfileData Instance => _instance == null ? _instance = new ProfileData() : _instance;
        public long bestScore;
        public long time;
        private Language _language;
        public Language Lang
        {
            get => _language; 
            set
            {
                _language = value;
                PlayerPrefs.SetInt("Profile.Language", (int)value);
            }
        }

        public ProfileData()
        {
            bestScore = PlayerPrefs.HasKey("Profile.BestScore") ? long.Parse(PlayerPrefs.GetString("Profile.BestScore")) : 0;
            time = PlayerPrefs.HasKey("Profile.Time") ? long.Parse(PlayerPrefs.GetString("Profile.Time")) : 0;
            _language = (Language)(PlayerPrefs.HasKey("Settings.Language") ? PlayerPrefs.GetInt("Settings.Language") : 0);
        }
        public bool Update(long time, long result)
        {
            this.time += (DateTime.Now.Ticks - time);
            bool isNewBestScore = result > bestScore;
            if (isNewBestScore)
            {
                bestScore = result;
                PlayerPrefs.SetString("Profile.BestScore", bestScore.ToString());
            }
            PlayerPrefs.SetString("Profile.Time", time.ToString());
            return isNewBestScore;
        }
    }
}
