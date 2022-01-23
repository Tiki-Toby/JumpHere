using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.Player
{
    abstract class BaseInputHandler : MonoBehaviour
    {
        protected bool isStop, isJump;
        protected GroundDetector _groundDetecter;
        public abstract void HandleJump();
        public abstract void HandleStop();
        private void Awake()
        {
            isStop = false;
            isJump = false;
            _groundDetecter = GetComponent<GroundDetector>();
        }
        public bool IsStop()
        {
            return isStop;
        }
        public bool IsJump()
        {
            bool isJumpVal = isJump;
            isJump = false;
            return isJumpVal;
        }
        private void Update()
        {
            HandleJump();
            HandleStop();
        }
    }
}
