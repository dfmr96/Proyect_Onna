using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Tools.AnimationCapture
{
    /// <summary>
    /// Core utility for capturing sprites from animated GameObjects at specific animation frames.
    /// Use AnimationSpriteGeneratorWindow for interactive usage.
    /// </summary>
    public static class AnimationSpriteGenerator
    {
        public static Camera renderCamera;
        public static Vector2Int resolution = new Vector2Int(512, 512);
        public static string outputFolder = "Assets/AnimationSprites";
        public static Color backgroundColor = new Color(0, 0, 0, 0); // Transparent

        /// <summary>
        /// Captures a sprite of the target GameObject at a specific animation state and normalized time.
        /// </summary>
        /// <param name="target">GameObject with Animator component</param>
        /// <param name="stateName">Name of the animation state (e.g., "Reload", "Idle")</param>
        /// <param name="normalizedTime">Time in the animation (0.0 to 1.0)</param>
        /// <param name="layerIndex">Animator layer index (default: 0)</param>
        /// <returns>Path to the saved PNG file, or null if failed</returns>
        public static string CaptureAnimationFrame(GameObject target, string stateName, float normalizedTime, int layerIndex = 0)
        {
            if (target == null)
            {
                Debug.LogError("Target GameObject is null");
                return null;
            }

            Animator animator = target.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"GameObject '{target.name}' does not have an Animator component");
                return null;
            }

            if (renderCamera == null)
            {
                Debug.LogError("Render camera is not set. Please assign a camera in the AnimationSpriteGeneratorWindow.");
                return null;
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Setup render texture
            RenderTexture rt = new RenderTexture(resolution.x, resolution.y, 24, RenderTextureFormat.ARGB32);
            renderCamera.targetTexture = rt;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = backgroundColor;

            // Sample the animation at the specified frame
            SampleAnimation(animator, stateName, normalizedTime, layerIndex);

            // Auto-frame the object in camera view
            AutoFrameObject(target, renderCamera);

            // Render the camera
            renderCamera.Render();

            // Read pixels from render texture
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(resolution.x, resolution.y, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, resolution.x, resolution.y), 0, 0);
            tex.Apply();

            // Encode to PNG
            byte[] png = tex.EncodeToPNG();

            // Generate filename: ObjectName_StateName_Frame000.png
            int frameNumber = Mathf.RoundToInt(normalizedTime * 100);
            string safeStateName = SanitizeFileName(stateName);
            string fileName = $"{target.name}_{safeStateName}_F{frameNumber:000}.png";
            string path = Path.Combine(outputFolder, fileName);

            File.WriteAllBytes(path, png);

            // Cleanup
            renderCamera.targetTexture = null;
            RenderTexture.active = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);

            AssetDatabase.Refresh();

            Debug.Log($"Animation sprite captured: {path}");
            return path;
        }

        /// <summary>
        /// Samples the animator at a specific state and normalized time without playing the animation.
        /// </summary>
        public static void SampleAnimation(Animator animator, string stateName, float normalizedTime, int layerIndex = 0)
        {
            if (animator == null || string.IsNullOrEmpty(stateName))
                return;

            // Get the state hash
            int stateHash = Animator.StringToHash(stateName);

            // Update mode to allow sampling in edit mode
            AnimatorUpdateMode previousUpdateMode = animator.updateMode;
            animator.updateMode = AnimatorUpdateMode.Normal;

            // Play the state
            animator.Play(stateHash, layerIndex, normalizedTime);

            // Force update to apply the animation immediately
            animator.Update(0f);

            // Restore previous update mode
            animator.updateMode = previousUpdateMode;
        }

        /// <summary>
        /// Auto-frames the target object in the camera view.
        /// </summary>
        public static void AutoFrameObject(GameObject target, Camera camera)
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
                return;

            // Calculate combined bounds of all renderers
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            // Position camera to look at the center of the object
            Vector3 targetPosition = bounds.center;
            Vector3 cameraOffset = camera.transform.position - target.transform.position;
            camera.transform.position = targetPosition + cameraOffset;

            // Adjust orthographic size to fit the object
            if (camera.orthographic)
            {
                float maxExtent = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
                camera.orthographicSize = maxExtent * 1.5f; // 1.5x for padding
            }
            else
            {
                // For perspective camera, adjust distance
                float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
                float distance = maxSize / (2f * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
                Vector3 direction = camera.transform.forward;
                camera.transform.position = targetPosition - direction * distance * 1.5f;
            }
        }

        /// <summary>
        /// Sanitizes filename by removing invalid characters.
        /// </summary>
        private static string SanitizeFileName(string fileName)
        {
            char[] invalids = Path.GetInvalidFileNameChars();
            foreach (char c in invalids)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        /// <summary>
        /// Gets all animation state names from an Animator Controller.
        /// </summary>
        public static string[] GetAnimationStates(Animator animator, int layerIndex = 0)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return new string[0];

            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null)
            {
                // Try to get it from the editor
                controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(animator.runtimeAnimatorController));
            }

            if (controller == null || layerIndex >= controller.layers.Length)
                return new string[0];

            var states = controller.layers[layerIndex].stateMachine.states;
            string[] stateNames = new string[states.Length];

            for (int i = 0; i < states.Length; i++)
            {
                stateNames[i] = states[i].state.name;
            }

            return stateNames;
        }

        /// <summary>
        /// Gets all layer names from an Animator Controller.
        /// </summary>
        public static string[] GetAnimatorLayers(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return new string[0];

            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null)
            {
                controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(animator.runtimeAnimatorController));
            }

            if (controller == null)
                return new string[0];

            string[] layerNames = new string[controller.layers.Length];
            for (int i = 0; i < controller.layers.Length; i++)
            {
                layerNames[i] = controller.layers[i].name;
            }

            return layerNames;
        }
    }
}
