using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class Soundiniter : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] Image sprite;
        [SerializeField] Sprite turnOn, turnOff;
        [SerializeField] string saveName;

        private void OnEnable()
        {
            slider.value = PlayerPrefs.GetFloat("Settings." + saveName);
        }
        public void UpdateImage()
        {
            sprite.sprite = slider.value > 0 ? turnOn : turnOff;
        }
    }
}