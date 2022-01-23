using Assets.Scripts.Players;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Data
{
    public class AudioSourceManager : MonoBehaviour
    {
        private static AudioSourceManager _instance;
        public static AudioSourceManager Instance => _instance;
        [SerializeField] AudioSource music;
        [SerializeField] AudioSource lobbyMusic;
        [SerializeField] AudioSource loseSound;
        [SerializeField] AudioSource stepSound;
        [SerializeField] AudioSource jumpSound;

        private void Awake()
        {
            _instance = this;
        }
        private void Start()
        {
            float musicVolume = PlayerPrefs.HasKey("Settings.MusicVolume") ? PlayerPrefs.GetFloat("Settings.MusicVolume") : 1f;
            float soundVolume = PlayerPrefs.HasKey("Settings.SoundVolume") ? PlayerPrefs.GetFloat("Settings.SoundVolume") : 1f;

            SetMusicVolume(musicVolume);
            SetStepSoundVolume(soundVolume);
        }
        //public void UpdateStepSound(bool isWalk)
        //{
        //    if (GameHandler.Instance.IsGame && !GameHandler.Instance.IsPause)
        //    {
        //        if (character.isGrounded)
        //        {
        //            if (character.Velocity == 0f)
        //                stepSound.Stop();
        //            else if (!stepSound.isPlaying)
        //                stepSound.Play();
        //        }
        //        else
        //        {
        //            if (Input.GetButtonDown("Jump"))
        //                jumpSound.Play();
        //            stepSound.Stop();
        //        }
        //    }
        //    else
        //    {
        //        jumpSound.Stop();
        //        stepSound.Stop();
        //    }
        //}
        public void UpdateStepSound(bool isWalk)
        {
            if(!isWalk)
                stepSound.Pause();
            else if(!stepSound.isPlaying)
                stepSound.Play();
        }
        public void JumpSound()
        {
            jumpSound.Play();
        }

        public void StartGameMusic()
        {
            music.Play();
            lobbyMusic.Stop();
        }
        public void StartLobbyMusic()
        {
            lobbyMusic.Play();
            music.Stop();
        }
        public void PlayLoseSound()
        {
            loseSound.Play();
            lobbyMusic.Stop();
            music.Stop();
        }
        public void SetMusicVolume(Slider slider)
        {
            SetMusicVolume(slider.value);
        }
        public void SetMusicVolume(float value)
        {
            music.volume = value;
            lobbyMusic.volume = value;
            loseSound.volume = value;
            PlayerPrefs.SetFloat("Settings.MusicVolume", value);
        }
        public void SetStepSoundVolume(Slider slider)
        {
            SetStepSoundVolume(slider.value);
        }
        private void SetStepSoundVolume(float value)
        {
            stepSound.volume = value;
            jumpSound.volume = value;
            PlayerPrefs.SetFloat("Settings.SoundVolume", value);
        }
    }
}