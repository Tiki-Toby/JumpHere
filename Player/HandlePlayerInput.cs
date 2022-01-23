using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.Player
{
    class HandlePlayerInput : BaseInputHandler
    {
        [SerializeField] float timeForWaitingGrouded;
        public override void HandleJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopAllCoroutines();
                StartCoroutine(TryToCompleteJump());
            }
        }
        public override void HandleStop()
        {
            if (Input.GetKey(KeyCode.G))
                isStop = true;
            else
                isStop = false;
        }
        private IEnumerator TryToCompleteJump()
        {
            float timer = timeForWaitingGrouded;
            while (timer > 0)
            {
                if(_groundDetecter.IsGrounded)
                {
                    Debug.Log("AAAAAAAAAa");
                    isJump = true;
                    timer = 0;
                }
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
