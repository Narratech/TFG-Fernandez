using UnityEngine;

namespace LiquidSnake.UI
{
    public class FaceCamera : MonoBehaviour
    {
        [Tooltip("Camera to use to look at")]
        private Camera _mainCamera;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
            }

            if (_mainCamera == null)
            {
                Debug.LogError("No camera found in scene to face.");
            }
        }

        private void Update()
        {
            transform.LookAt(_mainCamera.transform, Vector3.up);
        } // Update

    } // FaceCamera

} // namespace LiquidSnake.UI
