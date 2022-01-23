using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ComponentPanelController : PanelSpawnController
    {
        PanelSpawnController[] panels;
        private void OnEnable()
        {
            if(panels == null)
                panels = transform.GetComponentsInChildren<PanelSpawnController>();
        }
        private void Start()
        {
            foreach (PanelSpawnController panel in panels)
                if(!panel.ignoreController)
                    panel.SetTime(time);
            gameObject.SetActive(isOpen);
        }
        public override void HidePanel()
        {
            if (!isOpen)
                return;
            isOpen = false;

            foreach (PanelSpawnController panel in panels)
                panel.HidePanel();
        }

        public override void OpenPanel()
        {
            if (isOpen)
                return;
            else
                gameObject.SetActive(true);
            isOpen = true;
            foreach (PanelSpawnController panel in panels)
                panel.OpenPanel();
        }
    }
}