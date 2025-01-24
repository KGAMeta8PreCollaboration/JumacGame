using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigame.RGLight
{
    public class Player : MonoBehaviour
    {
        public float moveSpeed;
        public float rotateSpeed;

        public float maxHealth;
        private float _curHealth;

        private MainPage _mainPage;
        [SerializeField] private List<GameObject> _models = new List<GameObject>();

        public float HealthAmount
        {
            get
            {
                return _curHealth / maxHealth;
            }
        }
        public bool IsDead { get; private set; }

        public RGLightManager RGLightManager { get; private set; }

        public PlayerDistanceTracker PlayerDistanceTracker { get; private set; }
        public PlayerRay PlayerRay { get; private set; }
        private PlayerInputManager _playerInputManager;
        private Rigidbody _playerRigidbody;
        private Animator _playerAnimator;
        private Joystick _joystick;

        private void Update()
        {
            _mainPage.SetHealth(HealthAmount);
        }

        private void FixedUpdate()
        {
            if (RGLightManager.IsEndGame) return;

            _playerAnimator.SetBool("Walk", _joystick.dir.magnitude >= 0.1f);

            Vector3 inputDir = new Vector3(-_joystick.dir.x, 0, -_joystick.dir.y);
            Move(inputDir);
            Rotate(inputDir);
        }

        private void Move(Vector3 input)
        {
            Vector3 actualMove = new Vector3(input.x, 0, input.z).normalized * moveSpeed * Time.fixedDeltaTime;
            _playerRigidbody.MovePosition(_playerRigidbody.position + actualMove);
        }

        public void Rotate(Vector3 input)
        {
            if (input.sqrMagnitude < 0.1f) return;

            Vector3 direction = new Vector3(input.x, 0, input.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }

        public void TakeDamage(float damage)
        {
            _curHealth -= damage;
            if (_curHealth <= 0 && !IsDead) Die();
        }

        private void Die()
        {
            IsDead = true;
            _curHealth = 0;
            RGLightManager.GameResult(false);
        }

        public async void Init(RGLightManager manager)
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
            _playerRigidbody = GetComponent<Rigidbody>();
            PlayerDistanceTracker = GetComponent<PlayerDistanceTracker>();
            PlayerRay = GetComponent<PlayerRay>();
            _mainPage = GameObject.FindObjectOfType<MainPage>();
            _playerAnimator = GetComponentInChildren<Animator>();
            _joystick = GameObject.FindObjectOfType<Joystick>();

            RGLightManager = manager;

            string race = await RGLightManager.GetRace();
            if (race != null)
            {
                foreach (GameObject model in _models)
                {
                    if (model.name.Equals(race))
                    {
                        model.SetActive(true);
                    }
                    else
                    {
                        model.SetActive(false);
                    }
                }
            }

            CinemachineVirtualCamera cvc = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            cvc.Follow = transform;
            cvc.LookAt = transform;

            _curHealth = maxHealth;
        }
    }
}
