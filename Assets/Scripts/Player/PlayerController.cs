using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExtractionAgent.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        //Movement Variables
        [Header("Movement Variables")]
        [SerializeField]private float moveSpeed = 5f;
        [SerializeField]private float jumpForce = 10f;
        [SerializeField]private float crouchSpeed = 2.5f;
        [SerializeField]private float rollForce = 8f;
        // [Header("Aiming Variables")]
        // [SerializeField]private Transform arm;
        // [SerializeField]private  

        [Header("Shooting Variables")]
        [SerializeField]private Transform firePoint;
        [SerializeField]private GameObject bulletPrefab;
        [SerializeField]private float bulletSpeed = 20f;
        [SerializeField]private int maxAmmo = 10;

        [Space]
        private Rigidbody rb;
        private PlayerInputActions playerinputactions;

        private Vector2 moveInput;
        private bool isJumping;

        // private bool isGrounded;
        private bool isCrouching;
        private bool isRolling;
        private int currentAmmo;
        public GameObject debugTransform;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            playerinputactions = new PlayerInputActions();

            // Binding input actions
            playerinputactions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            playerinputactions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

            playerinputactions.Player.Jump.performed += ctx => isJumping = true;
            playerinputactions.Player.Crouch.performed += ctx => ToogleCrouch();
            playerinputactions.Player.Roll.performed += ctx => PerformRoll();
            playerinputactions.Player.Shoot.performed += ctx => Shoot();

            currentAmmo = maxAmmo;
        }


        private void OnEnable()
        {
            playerinputactions.Enable();
        }
        private void OnDisable()
        {
            playerinputactions.Disable();
        }

        private void FixedUpdate()
        {
            MovePlayer();
            HandleJump();
            PlayerAnimations();
        }
        private void Update()
        {
            SlowMotion();
        }

        private void SlowMotion()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Time.timeScale = 0.4f;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                Time.timeScale = 1f;
            }
        }
        private void MovePlayer()
        {
            float speed = isCrouching ? crouchSpeed : moveSpeed;        //changes the speed when crouching 
            Vector3 movement = new Vector3(moveInput.x,0,0) * speed;
            rb.linearVelocity = new Vector3(movement.x,rb.linearVelocity.y,0);

        }
        private void HandleJump()
        {
            if(isJumping && IsGrounded() && !isCrouching)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            isJumping = false;
        }
        private void ToogleCrouch()
        {
            isCrouching = !isCrouching;
            if(isCrouching == true)
            {
                transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y/2f,transform.localScale.z);
            }
            else{
                transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y*2f,transform.localScale.z);
            }
        }


        private void PerformRoll()
        {
            if(!isRolling && IsGrounded() && !isCrouching)
            {
                Debug.Log("isnotrollingchange to true");
                isRolling = true;
                rb.AddForce(new Vector3(moveInput.x,0,0)*rollForce,ForceMode.Impulse);
                animator.SetTrigger("Sprint");
                Debug.Log("performed");
                EndRoll();
                // Invoke(nameof(EndRoll),1f);                           //Adjusts the duration of the roll
            }
        }
        private void EndRoll()
        {
            isRolling = false;
        }
        [SerializeField] private float rotSpeed = 5f;
        [SerializeField] private float animationSpeed = 1f;

        private void PlayerAnimations()
        {
            if(moveInput.x > 0)
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.x,90f,transform.rotation.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,Time.deltaTime*rotSpeed);   
            }
            else if(moveInput.x < 0)
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.x,270f,transform.rotation.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,Time.deltaTime*rotSpeed);
            }
            if(moveInput.x == 0)
            {
                animator.SetFloat("Movement",Mathf.Lerp(animator.GetFloat("Movement"),Mathf.Abs(moveInput.x),animationSpeed*Time.deltaTime));
            }
            else 
            {
                animator.SetFloat("Movement",Mathf.Lerp(animator.GetFloat("Movement"),Mathf.Abs(moveInput.x),animationSpeed*Time.deltaTime));
            }
            // if(isRolling)
            // {
                
            // }
        }
        private void Shoot()
        {
            if(currentAmmo > 0)
            {
                GameObject bullet = Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
                Rigidbody rigb = bullet.GetComponent<Rigidbody>();
                rigb.linearVelocity = firePoint.forward * bulletSpeed;

                currentAmmo--;
            }
            else{
                Debug.LogError("out of ammo!!");
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 0.6f);
        }
        public void SetDir(float dir)
        {
            if(dir > 0)
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.x,90f,transform.rotation.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,Time.deltaTime*rotSpeed);   
            }
            else if(dir < 0)
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.x,270f,transform.rotation.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,Time.deltaTime*rotSpeed);
            }
        }
    }
}