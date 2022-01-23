using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.Player
{
    class AbstractRunnerController : MonoBehaviour
    {
        [SerializeField] float velocity;
        [SerializeField] float acceleration;
        [SerializeField] float velocitySmoothness;
        [SerializeField] float rotateSmoothness;
        [SerializeField] float jumpForce;
        [SerializeField] float gravity;
        [SerializeField] float gravityForce;
        public Rigidbody PlayerBody { get; protected set; }
        public Quaternion TargetRotation { get; protected set; }
        public float CurrentVelocity { get; protected set; }
        public Vector3 Delta { get; protected set; }
        public Vector3 PrevVelocity { get; protected set; }
        public Vector3 CurrentVelocityDirection { get; protected set; }
        public Vector3 TargetDirection { get; protected set; }
        public Vector3 CurrentDirection { get; protected set; }
        public float VelocityRatio { get; protected set; }
        public float JumpVelocity { get; private set; }
        public float RunMultiplier { get; private set; }
        GroundDetector _groundDetecter;
        Animator _animator;
        CapsuleCollider _collider;
        private void Awake()
        {
            PlayerBody = GetComponent<Rigidbody>();
            _groundDetecter = GetComponent<GroundDetector>();
            _collider = GetComponent<CapsuleCollider>();
            _animator = GetComponent<Animator>();
        }
        private void Start()
        {
            RunMultiplier = 1;
            CurrentVelocity = 0;
            CurrentVelocityDirection = Vector3.zero;
            CurrentDirection = new Vector3(SettingsData.StartDirection.x, 0, SettingsData.StartDirection.z);
            TargetDirection = CurrentDirection;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (GameHandler.Instance.IsPause)
            {
                PlayerBody.velocity = Vector3.zero;
                _animator.SetBool("Run", false);
                AudioSourceManager.Instance.UpdateStepSound(false);
                return;
            }

            Vector3 direction = Vector3.zero;
            if (!Input.GetKey(KeyCode.G) && GameHandler.Instance.IsGame)
                direction = TargetDirection;
            WalkUpdate(direction);
            RotationUpdate(direction);

            UpdateAnimator();
            AudioSourceManager.Instance.UpdateStepSound(VelocityRatio > 0.5f && _groundDetecter.IsGrounded);
        }
        private void Update()
        {
            if (!GameHandler.Instance.IsGameProcess)
                return;
            velocity = SessionData.velocity;
            Jump();
        }
        public void AddSprintImpact(float add) => RunMultiplier += add;
        public void TurnDirection(Vector3 angles)
        {
            TargetDirection = new Vector3(TargetDirection.z, 0, TargetDirection.x);
            CurrentDirection = new Vector3(CurrentDirection.z, 0, CurrentDirection.x);
        }
        protected void RotationUpdate(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            if (CurrentVelocity / velocity < 0.4f)
                TargetRotation = Quaternion.LookRotation(direction);
            else
                TargetRotation = Quaternion.LookRotation(CurrentVelocityDirection);

            PlayerBody.rotation = Quaternion.Lerp(PlayerBody.rotation, TargetRotation, rotateSmoothness * Time.deltaTime);
        }
        protected void WalkUpdate(Vector3 direction)
        {
            PrevVelocity = CurrentVelocityDirection;

            float hVelocity = VelocityValue(CurrentVelocityDirection.x, acceleration * direction.x);
            float vVelocity = VelocityValue(CurrentVelocityDirection.z, acceleration * direction.z);

            CurrentVelocityDirection = new Vector3(hVelocity, 0, vVelocity);
            CurrentVelocityDirection = Vector3.ClampMagnitude(CurrentVelocityDirection, velocity * RunMultiplier);
            CurrentVelocity = CurrentVelocityDirection.magnitude;

            Vector3 jumpVelocity = _groundDetecter.IsGrounded ? Vector3.zero : Vector3.up * JumpVelocity;

            VelocityRatio = CurrentVelocity / velocity / RunMultiplier;
            PlayerBody.AddForce(Vector3.Lerp(PrevVelocity, CurrentVelocityDirection + jumpVelocity, velocitySmoothness * Time.deltaTime));
            //PlayerBody.velocity = Vector3.Lerp(PrevVelocity, CurrentVelocityDirection + jumpVelocity, velocitySmoothness * Time.deltaTime);
        }
        protected void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TargetDirection = CurrentDirection;
                if (_groundDetecter.IsGrounded)
                {
                    JumpVelocity = jumpForce;
                    AudioSourceManager.Instance.JumpSound();
                    _groundDetecter.Jump();
                }
            }
            JumpVelocity -= gravityForce * Time.deltaTime;
            JumpVelocity = Mathf.Clamp(JumpVelocity, -gravity, jumpForce);
        }
        public virtual void Init(Vector3 newPosition, Vector3 direction)
        {
            PlayerBody.MovePosition(newPosition + Vector3.up * _collider.height);
            CurrentDirection = new Vector3(direction.x, 0, direction.z);
            TargetDirection = CurrentDirection;
        }
        private void UpdateAnimator()
        {
            _animator.SetFloat("Speed", VelocityRatio);
            _animator.SetBool("Run", VelocityRatio > 0.05f);
        }
        private float VelocityValue(float value, float add)
        {
            if (add == 0)
            {
                if (value > 0.02f || value < -0.02f)
                    return Mathf.Lerp(value, 0f, velocitySmoothness * Time.deltaTime);
                else
                    return 0f;
            }
            else
            {
                value += add * Time.deltaTime;
                value = Mathf.Clamp(value, -velocity * RunMultiplier, velocity * RunMultiplier);
                return value;
            }
        }
    }
}
