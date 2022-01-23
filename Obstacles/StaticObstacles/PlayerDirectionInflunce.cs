using Assets.Scripts.Data;
using Assets.Scripts.Level;
using HairyEngine.Player;
using UnityEngine;

namespace JumpHere.Obstacles
{
    public class PlayerDirectionInflunce : Obstacles
    {
        [SerializeField] float obstacleInflunce;
        [SerializeField] bool isRigidbody;
        Rigidbody _rigidbody;
        private void Awake()
        {
            if (isRigidbody)
                _rigidbody = GetComponent<Rigidbody>();
        }
        private void CollisionHandle(Collision collision)
        {
            RunnerController controller = collision.transform.GetComponent<RunnerController>();
            if (controller && !isRigidbody)
            {
                float angle;
                if (controller.ExternalRotation == 0)
                    angle = UnityEngine.Random.Range(-1f, 1f);
                else if (controller.Delta.x == 0)
                    angle = UnityEngine.Random.value * Mathf.Sign(controller.ExternalRotation);
                else
                    angle = UnityEngine.Random.value * Mathf.Sign(controller.ExternalRotation);
                angle *= obstacleInflunce;
                //dir = controller.CurrentVelocityDirection * Mathf.Tan(angle);
                //if (controller.Delta.magnitude < 0.02f)
                //    dir *= UnityEngine.Random.Range(-1f, 1f);
                //else if (controller.Delta.x == 0)
                //    dir *= UnityEngine.Random.value * Mathf.Sign(controller.Delta.z);
                //else
                //    dir *= UnityEngine.Random.value * Mathf.Sign(controller.Delta.x);
                //dir *= obstacleInflunce;
                controller.OffsetDirectionByAngle(angle);
            }
        }
        public override void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                CollisionHandle(collision);
                base.OnCollisionEnter(collision);
            }
        }
    }
}