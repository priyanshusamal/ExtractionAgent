using UnityEngine;
using ExtractionAgent.Player;

namespace ExtractionAgent
{
    public class AimController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform gunPivot;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerController playerController;
        // [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private CameraController cameraController;
        public GameObject aimObject;

        private bool isFacingRight = true;

        private void Update()
        {
            AimAtMouse();
        }

        private void AimAtMouse()
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.y = 0f;
            mousePosition.z = 0f; // Keep in 2D plane
            aimObject.transform.position = mousePosition;
            Vector3 direction = (mousePosition - gunPivot.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Flip player if aiming direction changes
            if ((direction.x < 0 && isFacingRight) || (direction.x > 0 && !isFacingRight))
            {
                // FlipPlayer(direction.x);
                playerController.SetDir(direction.x);
            }

            // Adjust gun rotation based on facing direction
            gunPivot.rotation = Quaternion.Euler(0f, 0f, angle);
            
            // Update camera look-ahead
            cameraController.UpdateLookAhead(direction);
        }

        private void FlipPlayer(float aimDirectionX)
        {
            isFacingRight = aimDirectionX > 0;

            // if()
            // Flip player scale
            // Vector3 newScale = playerTransform.localScale;
            // newScale.x = isFacingRight ? 1 : -1;
            // playerTransform.localScale = newScale;

            // Flip gunPivot
            Vector3 gunScale = gunPivot.localScale;
            gunScale.y = isFacingRight ? 1 : -1;
            gunPivot.localScale = gunScale;

            // Flip sprite render direction
            // playerSprite.flipX = !isFacingRight;
        }

    }
}
