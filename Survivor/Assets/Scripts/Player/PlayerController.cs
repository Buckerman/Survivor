using System.Collections;
using UnityEngine;

namespace Entities.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float climbSpeed = 3f;
        [SerializeField] private float moveTowardsDistance;
        [SerializeField] private float edgeDetectionDistance = 0.5f;

        private CharacterController controller;
        private bool isClimbing = false;
        private bool canJump = false;
        public float jumpHeight = 3f;
        public float gravity = -9.81f;
        private Vector3 velocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (isClimbing)
            {
                HandleClimbing();
            }
            else
            {
                HandleKeyboardInput();
                CheckEdgeAndJump();
            }
        }
        private void HandleClimbing()
        {
            Vector3 climbDirection = Vector3.up;
            controller.Move(climbDirection * climbSpeed * Time.deltaTime);
        }

        private void HandleKeyboardInput()
        {
            if (!controller.isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                if (velocity.y < 0)
                {
                    velocity.y = -2f;
                }
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
            moveDirection.Normalize();
            controller.Move(moveDirection * speed * Time.deltaTime);
            controller.Move(velocity * Time.deltaTime);
        }

        private void CheckEdgeAndJump()
        {
            if (controller.isGrounded)
            {
                Vector3 origin = transform.position;
                origin.y += 0.1f;
                Ray ray = new Ray(origin, Vector3.down);
                RaycastHit hit;

                if (!Physics.Raycast(ray, out hit, edgeDetectionDistance))
                {
                    CheckForPlatform();
                    if (canJump)
                    {
                        PerformJump();
                    }
                }
            }
        }
        private void CheckForPlatform()
        {
            if (currentPlatform == null || !currentPlatform.gameObject.activeInHierarchy)
            {
                canJump = false;
            }
        }
        private void PerformJump()
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }

        private Transform currentClimbable;
        private Transform currentPlatform;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building") || other.CompareTag("Platform"))
            {
                isClimbing = true;
                currentClimbable = other.transform;
                canJump = false;
            }

            if (other.CompareTag("canJump"))
            {
                currentPlatform = other.transform;
                canJump = true;
                StartCoroutine(PlatformRespawnTimer(other));
            }
        }
        private IEnumerator PlatformRespawnTimer(Collider other)
        {
            yield return new WaitForSeconds(3.5f);
            other.transform.parent.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((other.CompareTag("Building") || other.CompareTag("Platform")) && other.transform == currentClimbable)
            {
                isClimbing = false;

                Vector3 moveTowardsDirection = (currentClimbable.position - transform.position).normalized;
                moveTowardsDirection.y = 0;

                Vector3 moveAmount = moveTowardsDirection * moveTowardsDistance;
                controller.Move(moveAmount);

                currentClimbable = null;
            }
        }
    }
}
