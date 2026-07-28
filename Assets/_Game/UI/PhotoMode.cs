using UnityEngine;

namespace Asteria.UI
{
    /// <summary>
    /// Photo mode for taking screenshots.
    /// Hides UI, allows free camera, and captures images.
    /// </summary>
    public sealed class PhotoMode : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] KeyCode toggleKey = KeyCode.P;
        [SerializeField] float cameraSensitivity = 2f;
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float fastMoveSpeed = 30f;

        bool _isActive;
        Camera _camera;
        Vector3 _originalPosition;
        Quaternion _originalRotation;
        float _rotationX;
        float _rotationY;
        GameUIRoot _uiRoot;

        public bool IsActive => _isActive;

        void Start()
        {
            _camera = Camera.main;
            _uiRoot = GameUIRoot.Instance;
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                TogglePhotoMode();
            }

            if (_isActive)
            {
                HandleCameraMovement();
                HandleCapture();
            }
        }

        void TogglePhotoMode()
        {
            _isActive = !_isActive;

            if (_isActive)
            {
                EnterPhotoMode();
            }
            else
            {
                ExitPhotoMode();
            }
        }

        void EnterPhotoMode()
        {
            if (_camera == null) return;

            // Save original camera state
            _originalPosition = _camera.transform.position;
            _originalRotation = _camera.transform.rotation;

            // Hide UI
            if (_uiRoot != null)
            {
                _uiRoot.gameObject.SetActive(false);
            }

            // Unlock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Initialize rotation
            var euler = _camera.transform.eulerAngles;
            _rotationX = euler.y;
            _rotationY = euler.x;

            Debug.Log("[Asteria] Photo mode ON. Press P to exit. Press Space to capture.");
        }

        void ExitPhotoMode()
        {
            if (_camera == null) return;

            // Restore camera
            _camera.transform.position = _originalPosition;
            _camera.transform.rotation = _originalRotation;

            // Show UI
            if (_uiRoot != null)
            {
                _uiRoot.gameObject.SetActive(true);
            }

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("[Asteria] Photo mode OFF.");
        }

        void HandleCameraMovement()
        {
            if (_camera == null) return;

            // Rotation
            _rotationX += Input.GetAxis("Mouse X") * cameraSensitivity;
            _rotationY -= Input.GetAxis("Mouse Y") * cameraSensitivity;
            _rotationY = Mathf.Clamp(_rotationY, -89f, 89f);

            _camera.transform.rotation = Quaternion.Euler(_rotationY, _rotationX, 0);

            // Movement
            float speed = Input.GetKey(KeyCode.LeftShift) ? fastMoveSpeed : moveSpeed;
            Vector3 move = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                Input.GetKey(KeyCode.Space) ? 1 : Input.GetKey(KeyCode.LeftControl) ? -1 : 0,
                Input.GetAxisRaw("Vertical")
            );

            _camera.transform.Translate(move * speed * Time.deltaTime, Space.Self);
        }

        void HandleCapture()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CaptureScreenshot();
            }
        }

        void CaptureScreenshot()
        {
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = $"Asteria_Photo_{timestamp}.png";
            string path = System.IO.Path.Combine(Application.persistentDataPath, "Photos", filename);

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));

            ScreenCapture.CaptureScreenshot(path, 2); // 2x resolution
            Debug.Log($"[Asteria] Photo saved: {path}");
        }
    }
}
