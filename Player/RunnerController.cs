using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;

namespace HairyEngine.Player
{
    class RunnerController : MonoBehaviour
    {
        [SerializeField] float velocity;
        [SerializeField] float acceleration;
        [SerializeField] float velocitySmoothness;
        [SerializeField] float rotateSmoothness;
        [SerializeField] float jumpForce;
        [SerializeField] float gravity;
        [SerializeField] float gravityForce;
        [SerializeField] float toCenterSmooth;
        public Rigidbody PlayerBody { get; protected set; }
        public Quaternion TargetRotation { get; protected set; }
        public float CurrentVelocity { get; protected set; }
        public Vector3 Delta { get; protected set; }
        public Vector3 PrevVelocity { get; protected set; }
        public Vector3 CurrentVelocityDirection { get; protected set; }
        public Vector3 TargetDirection { get; protected set; }
        public Vector3 CurrentDirection { get; protected set; }
        public float VelocityRatio { get; protected set; }
        public float ExternalRotation { get; protected set; }
        public float RunMultiplier { get; private set; }
        GroundDetector _groundDetecter;
        BaseInputHandler _inputHandler;
        Animator _animator;
        CapsuleCollider _collider;
        private void Awake()
        {
            PlayerBody = GetComponent<Rigidbody>();
            _groundDetecter = GetComponent<GroundDetector>();
            _collider = GetComponent<CapsuleCollider>();
            _animator = GetComponent<Animator>();
            _inputHandler = GetComponent<BaseInputHandler>();
        }
        private void Start()
        {
            RunMultiplier = 1;
            CurrentVelocity = 0;
            ExternalRotation = 0;
            CurrentVelocityDirection = Vector3.zero;
            CurrentDirection = new Vector3(SettingsData.StartDirection.x, 0, SettingsData.StartDirection.z);
            TargetDirection = CurrentDirection;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (GameHandler.Instance.IsPause)
            {
                PlayerBody.Sleep();
                _animator.SetBool("Run", false);
                AudioSourceManager.Instance.UpdateStepSound(false);
                return;
            }

            Vector3 direction = Vector3.zero;
            if (!_inputHandler.IsStop() && GameHandler.Instance.IsGame)
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
            Jump();
        }
        public void AddSprintImpact(float add) => RunMultiplier += add;
        public virtual void Init(Vector3 newPosition, Vector3 direction)
        {
            PlayerBody.MovePosition(newPosition + Vector3.up * _collider.height);
            CurrentDirection = new Vector3(direction.x, 0, direction.z);
            TargetDirection = CurrentDirection;
        }
        public void TurnDirection()
        {
            TargetDirection = new Vector3(TargetDirection.z, 0, TargetDirection.x);
            CurrentDirection = new Vector3(CurrentDirection.z, 0, CurrentDirection.x);
        }
        public void OffsetDirectionByAngle(float angle)
        {
            ExternalRotation += angle * Mathf.PI / 180;
            float maxAngle = Mathf.PI / 2f;
            ExternalRotation = Mathf.Clamp(ExternalRotation, -maxAngle, maxAngle);
            ExternalRotationUpdate();
        }
        public void MakeForceOffsetToCenterLine(Vector3 tilePosition)
        {
            tilePosition -= PlayerBody.position;
            tilePosition.x *= CurrentVelocityDirection.z;
            tilePosition.y = 0;
            tilePosition.z *= CurrentVelocityDirection.x;
            Delta = tilePosition;
            if (TargetDirection == CurrentDirection)
            {
                Delta = Vector3.Lerp(Vector3.zero, Delta, toCenterSmooth * Time.deltaTime) * 10;
                PlayerBody.AddForce(Delta);
            }
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

            VelocityRatio = CurrentVelocity / velocity / RunMultiplier;
            PlayerBody.AddForce(Vector3.Lerp(PrevVelocity * 20, CurrentVelocityDirection * 20, velocitySmoothness * Time.deltaTime), ForceMode.Impulse);
        }
        protected void Jump()
        {
            if (_inputHandler.IsJump())
            {
                TargetDirection = CurrentDirection;
                ExternalRotation = 0;
                if (_groundDetecter.IsGrounded)
                {
                    AudioSourceManager.Instance.JumpSound();
                    PlayerBody.AddForce(jumpForce * Vector3.up, ForceMode.Acceleration);
                }
            }
        }
        private void UpdateAnimator()
        {
            _animator.SetFloat("Speed", VelocityRatio);
            _animator.SetBool("Run", VelocityRatio > 0.05f);
        }
        private void ExternalRotationUpdate()
        {
            float sin = Mathf.Sin(ExternalRotation);
            float cos = Mathf.Cos(ExternalRotation);
            Vector3 newDirection = CurrentDirection;
            newDirection.z = newDirection.x * cos - newDirection.z * sin;
            newDirection.x = newDirection.x * sin + newDirection.z * cos;
            TargetDirection = newDirection;
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
