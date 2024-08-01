using UnityEngine;

namespace Entities.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float climbSpeed = 3f;
        [SerializeField] private float moveTowardsDistance;

        private CharacterController controller;
        private bool isClimbing = false;
        private bool canJump = false;
        public float jumpHeight = 2f;
        public float gravity = -9.81f;
        private Vector3 velocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            HandleKeyboardInput();
            if (isClimbing)
            {
                HandleClimbing();
            }
            if (velocity.y < 0)
            {
                velocity.y = 0f;
            }
            if (canJump)
            {
                HandleJump();
            }
            controller.Move(velocity * Time.deltaTime);
        }
        private void HandleJump()
        {
            Debug.Log("jumped");
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }
        private void HandleKeyboardInput()
        {
            if (isClimbing)
            {
                return;
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
            moveDirection.Normalize();
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        private void HandleClimbing()
        {
            Vector3 climbDirection = Vector3.up;
            controller.Move(climbDirection * climbSpeed * Time.deltaTime);
        }

        private Transform currentClimbable;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                isClimbing = true;
                currentClimbable = other.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Building") && other.transform == currentClimbable)
            {
                isClimbing = false;

                Vector3 moveTowardsDirection = (currentClimbable.position - transform.position).normalized;
                moveTowardsDirection.y = 0;

                Vector3 moveAmount = moveTowardsDirection * moveTowardsDistance;
                controller.Move(moveAmount);

                currentClimbable = null;
            }

            if (other.CompareTag("canJump"))
            {
                canJump = true;
            }
        }
    }
}
