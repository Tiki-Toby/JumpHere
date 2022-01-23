using Assets.Scripts.Data;
using ECM.Controllers;
using HairyEngine.HairyCamera;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class MovementHandler : BaseCharacterController
    {
        [SerializeField] float toCenterSmooth = 1f;
        public Vector3 CurrentDirect { get; private set; }
        public Transform playerTransform { get; private set; }
        public Vector3 delta { get; private set; }
        public float Velocity => movement.velocity.NablaMultiply(CurrentDirect).magnitude;
        private Animator _anim;
        private Vector3 TmpDirection;

        public void AddDirection(Vector3 addDirection) 
        {
            addDirection.x *= CurrentDirect.z;
            addDirection.z *= CurrentDirect.x;
            moveDirection += addDirection;
        }

        public override void Awake()
        {
            base.Awake();
            _anim = GetComponent<Animator>();
            playerTransform = transform;
        }
        private void Start()
        {
            moveDirection = SettingsData.StartDirection;
            TmpDirection = moveDirection;
            CurrentDirect = SettingsData.StartDirection;
        }
        public void UpdateForce(Vector3 delta)
        {
            speed = SessionData.velocity;
            this.delta = delta;
            if (moveDirection == CurrentDirect)
            {
                if (delta.x == 0)
                    delta = Vector3.forward * Mathf.Lerp(0, Mathf.Abs(delta.z), toCenterSmooth * Time.deltaTime) * Mathf.Sign(delta.z);
                else
                    delta = Vector3.right * Mathf.Lerp(0, Mathf.Abs(delta.x), toCenterSmooth * Time.deltaTime) * Mathf.Sign(delta.x);
                movement.ApplyForce(-delta * 400, ForceMode.Force);
            }
        }
        protected override void HandleInput()
        {
            if (Input.GetKey(KeyCode.G) || isPaused || !GameHandler.Instance.IsGame)
            {
                if (moveDirection != Vector3.zero)
                {
                    TmpDirection = moveDirection;
                    moveDirection = Vector3.zero;
                    _anim.SetBool("Run", false);
                }
            }
            else
            {
                if (moveDirection == Vector3.zero)
                    moveDirection = TmpDirection;
                else
                {
                    if (CurrentDirect.x == 0)
                        moveDirection = new Vector3(moveDirection.x, 0, 1).normalized;
                    else
                        moveDirection = new Vector3(1, 0, moveDirection.z).normalized;
                }
                _anim.SetBool("Run", true);
            }
            jump = Input.GetKeyDown(KeyCode.Space);
            if (jump)
                moveDirection = CurrentDirect;
        }
        public void Init(Vector3 newPosition, Vector3 direction = new Vector3())
        {
            playerTransform.position = newPosition + Vector3.up * movement.capsuleCollider.height;
            if(direction == Vector3.zero)
                CurrentDirect = SettingsData.StartDirection;
            else
                CurrentDirect = direction;
            moveDirection = CurrentDirect;
        }
        public void Stop()
        {
            pause = GameHandler.Instance.IsPause;
        }
        public void TurnDirection() =>
            CurrentDirect = new Vector3(CurrentDirect.z, 0, CurrentDirect.x);
    }
}
