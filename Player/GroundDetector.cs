using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.Player
{
    class GroundDetector : MonoBehaviour
    {
        [SerializeField] int layer;
        public bool IsGrounded { get; private set; }
        private void Awake()
        {
            IsGrounded = true;
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == layer)
                IsGrounded = true;
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == layer)
                IsGrounded = false;
        }
        public void Jump() => IsGrounded = false;
    }
}
