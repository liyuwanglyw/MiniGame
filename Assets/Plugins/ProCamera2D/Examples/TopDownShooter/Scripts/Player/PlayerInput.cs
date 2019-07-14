using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerInput : MonoBehaviour
    {
        public float RunSpeed = 12;
        public float Acceleration = 30;

        float _currentSpeedH;
        float _currentSpeedV;
        Vector3 _amountToMove;
        int _totalJumps;

        CharacterController _characterController;

        bool _movementAllowed = true;
        private Animator _animator;
        public string Openlevel;
        public GameObject opendoor;

        CharacterController _characterController;

        public bool _movementAllowed = true;

        void Start()
        {
            _characterController = GetComponent<CharacterController>();

            var cinematics = FindObjectsOfType<ProCamera2DCinematics>();
            for (int i = 0; i < cinematics.Length; i++)
            {
                cinematics[i].OnCinematicStarted.AddListener(() =>
                    {
                        _movementAllowed = false; 
                        _currentSpeedH = 0;
                        _currentSpeedV = 0;
                    });

                cinematics[i].OnCinematicFinished.AddListener(() =>
                    {
                        _movementAllowed = true; 
                    });
            }
        }

        void Update()
        {
            if (!_movementAllowed)
                return;

            if (Input.GetKey(KeyCode.A))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, -90, 0);
                _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 90, 0);
                _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 0, 0);
                _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 180, 0);
                _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                if (!Openlevel.Equals(""))
                {
                    //MapControl.getInstance().StartLevel(Openlevel, Over);
                }
            }
            else
            {
                _animator.SetBool("Walk", false);
            }

            var targetSpeedH = Input.GetAxis("Horizontal") * RunSpeed;
            _currentSpeedH = IncrementTowards(_currentSpeedH, targetSpeedH, Acceleration);

            var targetSpeedV = Input.GetAxis("Vertical") * RunSpeed;
            _currentSpeedV = IncrementTowards(_currentSpeedV, targetSpeedV, Acceleration);

            _amountToMove.x = _currentSpeedH;
            _amountToMove.z = _currentSpeedV;

            _characterController.Move(_amountToMove * Time.deltaTime);
        }

        // Increase n towards target by speed
        private float IncrementTowards(float n, float target, float a)
        {
            if (n == target)
            {
                return n;   
            }
            else
            {
                float dir = Mathf.Sign(target - n); 
                n += a * Time.deltaTime * dir;
                return (dir == Mathf.Sign(target - n)) ? n : target;
            }
        }
    }
}