using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    class GamePanelManager : MonoBehaviour 
    {
        private static GamePanelManager _instance;
        public static GamePanelManager Instance => _instance;
        [SerializeField] ComponentPanelController losePanel;

        private void Awake()
        {
            _instance = _instance == null ? this : _instance;
        }
        public void OpenLosePanel(bool isSecondChance)
        {
            losePanel.OpenPanel();
            losePanel.transform.Find("Info").Find("ContinueForAdButton").gameObject.SetActive(!isSecondChance);
        }
    }
}
