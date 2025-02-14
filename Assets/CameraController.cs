using UnityEngine;

namespace ExtractionAgent
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform player;
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(3f, 1.5f, -10f); // Added Z offset
        
        [Header("Look-Ahead Settings")]
        [SerializeField] private float lookAheadMultiplier = 2f;
        [SerializeField] private float lookAheadSmoothing = 0.1f;

        private Vector3 _currentVelocity;
        private Vector3 _lookAheadOffset;

        private void LateUpdate()
        {
            if (player == null) return;

            Vector3 targetPosition = player.position + offset; // Apply full XYZ offset
            targetPosition += _lookAheadOffset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, followSpeed * Time.deltaTime);
        }

        public void UpdateLookAhead(Vector3 aimDirection)
        {
            _lookAheadOffset = new Vector3(aimDirection.x, 0f, 0f) * lookAheadMultiplier;
        }
    }

}
