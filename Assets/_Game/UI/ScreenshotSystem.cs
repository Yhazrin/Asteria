using System.IO;
using UnityEngine;

namespace Asteria.UI
{
    /// <summary>
    /// Screenshot system with filters and sharing capabilities.
    /// Extends the basic photo mode with more features.
    /// </summary>
    public sealed class ScreenshotSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int superSample = 2;
        [SerializeField] string screenshotFolder = "Screenshots";

        [Header("Filters")]
        [SerializeField] bool applyVignette = true;
        [SerializeField] bool applyColorCorrection = false;
        [SerializeField] float saturation = 1f;
        [SerializeField] float contrast = 1f;
        [SerializeField] float brightness = 1f;

        // State
        bool _isCapturing;
        string _lastScreenshotPath;

        /// <summary>
        /// Take a screenshot with current settings.
        /// </summary>
        public string TakeScreenshot()
        {
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = $"Asteria_{timestamp}.png";
            string folder = Path.Combine(Application.persistentDataPath, screenshotFolder);
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, filename);

            // Capture
            ScreenCapture.CaptureScreenshot(path, superSample);
            _lastScreenshotPath = path;

            Debug.Log($"[Screenshot] Saved: {path}");
            return path;
        }

        /// <summary>
        /// Take a screenshot with custom resolution.
        /// </summary>
        public string TakeScreenshot(int width, int height)
        {
            // Create render texture
            var rt = new RenderTexture(width, height, 24);
            var cam = Camera.main;
            if (cam == null) return null;

            cam.targetTexture = rt;
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            cam.Render();

            RenderTexture.active = rt;
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            // Save
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = $"Asteria_{width}x{height}_{timestamp}.png";
            string folder = Path.Combine(Application.persistentDataPath, screenshotFolder);
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, filename);

            File.WriteAllBytes(path, texture.EncodeToPNG());
            Destroy(texture);

            _lastScreenshotPath = path;
            Debug.Log($"[Screenshot] Saved: {path}");
            return path;
        }

        /// <summary>
        /// Get the last screenshot path.
        /// </summary>
        public string GetLastScreenshotPath()
        {
            return _lastScreenshotPath;
        }

        /// <summary>
        /// Open the screenshot folder.
        /// </summary>
        public void OpenScreenshotFolder()
        {
            string folder = Path.Combine(Application.persistentDataPath, screenshotFolder);
            Directory.CreateDirectory(folder);
            Application.OpenURL(folder);
        }

        /// <summary>
        /// Get all screenshots.
        /// </summary>
        public string[] GetAllScreenshots()
        {
            string folder = Path.Combine(Application.persistentDataPath, screenshotFolder);
            if (!Directory.Exists(folder)) return new string[0];
            return Directory.GetFiles(folder, "*.png");
        }

        /// <summary>
        /// Delete a screenshot.
        /// </summary>
        public void DeleteScreenshot(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[Screenshot] Deleted: {path}");
            }
        }

        /// <summary>
        /// Apply post-processing effects to the camera.
        /// </summary>
        public void ApplyEffects(Camera cam)
        {
            if (cam == null) return;

            // This would typically use a post-processing stack
            // For now, just log the settings
            Debug.Log($"[Screenshot] Effects: vignette={applyVignette}, sat={saturation}, contrast={contrast}");
        }
    }
}
