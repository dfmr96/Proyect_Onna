using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;

namespace Tools.AnimationCapture
{
    /// <summary>
    /// Editor Window for capturing sprites from animated GameObjects with interactive preview.
    /// Access via Window > Animation Sprite Generator
    /// </summary>
    public class AnimationSpriteGeneratorWindow : EditorWindow
    {
        // Target settings
        private GameObject targetObject;
        private Animator targetAnimator;
        private Camera renderCamera;

        // Animation selection
        private int selectedLayerIndex = 0;
        private string[] layerNames = new string[0];
        private int selectedStateIndex = 0;
        private string[] stateNames = new string[0];

        // Animation control
        private float normalizedTime = 0f;

        // Render settings
        private Vector2Int resolution = new Vector2Int(512, 512);
        private string outputFolder = "Assets/AnimationSprites";
        private Color backgroundColor = new Color(0, 0, 0, 0);

        // Preview
        private RenderTexture previewTexture;
        private Vector2 scrollPosition;
        private bool autoRefresh = true;

        // UI State
        private bool showSettings = true;
        private bool showPreview = true;

        [MenuItem("Window/Animation Sprite Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<AnimationSpriteGeneratorWindow>("Animation Sprite Generator");
            window.minSize = new Vector2(400, 600);
        }

        private void OnEnable()
        {
            // Initialize preview texture
            if (previewTexture == null)
            {
                previewTexture = new RenderTexture(resolution.x, resolution.y, 24, RenderTextureFormat.ARGB32);
            }
        }

        private void OnDisable()
        {
            // Cleanup
            if (previewTexture != null)
            {
                previewTexture.Release();
                DestroyImmediate(previewTexture);
            }
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawHeader();
            EditorGUILayout.Space(10);

            DrawTargetSelection();
            EditorGUILayout.Space(10);

            if (targetAnimator != null)
            {
                DrawAnimationSelection();
                EditorGUILayout.Space(10);

                DrawAnimationControl();
                EditorGUILayout.Space(10);

                DrawPreview();
                EditorGUILayout.Space(10);
            }

            DrawSettings();
            EditorGUILayout.Space(10);

            DrawCaptureButton();

            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Animation Sprite Generator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Capture sprites from animated GameObjects at specific animation frames.\n\n" +
                "1. Select a GameObject with Animator\n" +
                "2. Choose animation layer and state\n" +
                "3. Use slider to navigate to desired frame\n" +
                "4. Click 'Capture Current Frame' to save PNG",
                MessageType.Info);
        }

        private void DrawTargetSelection()
        {
            EditorGUILayout.LabelField("Target Object", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            targetObject = (GameObject)EditorGUILayout.ObjectField("GameObject", targetObject, typeof(GameObject), true);

            if (EditorGUI.EndChangeCheck())
            {
                OnTargetChanged();
            }

            // Display Animator status
            if (targetObject != null)
            {
                if (targetAnimator != null)
                {
                    EditorGUILayout.HelpBox($"Animator found: {targetAnimator.runtimeAnimatorController?.name ?? "No controller"}", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("No Animator component found on this GameObject!", MessageType.Warning);
                }
            }

            // Camera selection
            EditorGUI.BeginChangeCheck();
            renderCamera = (Camera)EditorGUILayout.ObjectField("Render Camera", renderCamera, typeof(Camera), true);
            if (EditorGUI.EndChangeCheck())
            {
                AnimationSpriteGenerator.renderCamera = renderCamera;
            }

            if (renderCamera == null)
            {
                EditorGUILayout.HelpBox("Please assign a camera to render the preview and capture sprites.", MessageType.Warning);
            }
        }

        private void DrawAnimationSelection()
        {
            EditorGUILayout.LabelField("Animation Selection", EditorStyles.boldLabel);

            // Layer selection
            if (layerNames.Length > 0)
            {
                EditorGUI.BeginChangeCheck();
                selectedLayerIndex = EditorGUILayout.Popup("Layer", selectedLayerIndex, layerNames);
                if (EditorGUI.EndChangeCheck())
                {
                    OnLayerChanged();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No animation layers found in Animator Controller.", MessageType.Warning);
            }

            // State selection
            if (stateNames.Length > 0)
            {
                EditorGUI.BeginChangeCheck();
                selectedStateIndex = EditorGUILayout.Popup("Animation State", selectedStateIndex, stateNames);
                if (EditorGUI.EndChangeCheck())
                {
                    UpdatePreview();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No animation states found in selected layer.", MessageType.Warning);
            }
        }

        private void DrawAnimationControl()
        {
            EditorGUILayout.LabelField("Animation Control", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            normalizedTime = EditorGUILayout.Slider("Frame (Normalized)", normalizedTime, 0f, 1f);

            if (EditorGUI.EndChangeCheck() && autoRefresh)
            {
                UpdatePreview();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Start (0%)"))
            {
                normalizedTime = 0f;
                UpdatePreview();
            }
            if (GUILayout.Button("Mid (50%)"))
            {
                normalizedTime = 0.5f;
                UpdatePreview();
            }
            if (GUILayout.Button("End (100%)"))
            {
                normalizedTime = 1f;
                UpdatePreview();
            }
            EditorGUILayout.EndHorizontal();

            autoRefresh = EditorGUILayout.Toggle("Auto Refresh Preview", autoRefresh);

            if (!autoRefresh && GUILayout.Button("Refresh Preview"))
            {
                UpdatePreview();
            }
        }

        private void DrawPreview()
        {
            showPreview = EditorGUILayout.Foldout(showPreview, "Preview", true);
            if (!showPreview) return;

            if (renderCamera == null || targetAnimator == null)
            {
                EditorGUILayout.HelpBox("Preview requires both a target object and render camera.", MessageType.Info);
                return;
            }

            // Calculate preview size to maintain aspect ratio
            float windowWidth = EditorGUIUtility.currentViewWidth - 40;
            float aspectRatio = (float)resolution.x / resolution.y;
            float previewHeight = windowWidth / aspectRatio;

            Rect previewRect = GUILayoutUtility.GetRect(windowWidth, previewHeight);

            // Draw preview texture
            if (previewTexture != null)
            {
                EditorGUI.DrawPreviewTexture(previewRect, previewTexture, null, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUI.DrawRect(previewRect, Color.gray);
            }

            EditorGUILayout.LabelField($"Resolution: {resolution.x}x{resolution.y}", EditorStyles.miniLabel);
        }

        private void DrawSettings()
        {
            showSettings = EditorGUILayout.Foldout(showSettings, "Settings", true);
            if (!showSettings) return;

            EditorGUI.BeginChangeCheck();
            resolution = EditorGUILayout.Vector2IntField("Resolution", resolution);
            if (EditorGUI.EndChangeCheck())
            {
                // Recreate preview texture with new resolution
                if (previewTexture != null)
                {
                    previewTexture.Release();
                    DestroyImmediate(previewTexture);
                }
                previewTexture = new RenderTexture(resolution.x, resolution.y, 24, RenderTextureFormat.ARGB32);
                AnimationSpriteGenerator.resolution = resolution;
                UpdatePreview();
            }

            outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
            AnimationSpriteGenerator.outputFolder = outputFolder;

            backgroundColor = EditorGUILayout.ColorField("Background Color", backgroundColor);
            AnimationSpriteGenerator.backgroundColor = backgroundColor;

            if (GUILayout.Button("Open Output Folder"))
            {
                EditorUtility.RevealInFinder(outputFolder);
            }
        }

        private void DrawCaptureButton()
        {
            EditorGUI.BeginDisabledGroup(targetObject == null || targetAnimator == null || renderCamera == null || stateNames.Length == 0);

            if (GUILayout.Button("Capture Current Frame", GUILayout.Height(40)))
            {
                CaptureFrame();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void OnTargetChanged()
        {
            if (targetObject != null)
            {
                targetAnimator = targetObject.GetComponent<Animator>();

                if (targetAnimator != null && targetAnimator.runtimeAnimatorController != null)
                {
                    layerNames = AnimationSpriteGenerator.GetAnimatorLayers(targetAnimator);
                    selectedLayerIndex = 0;
                    OnLayerChanged();
                }
                else
                {
                    layerNames = new string[0];
                    stateNames = new string[0];
                }
            }
            else
            {
                targetAnimator = null;
                layerNames = new string[0];
                stateNames = new string[0];
            }

            UpdatePreview();
        }

        private void OnLayerChanged()
        {
            if (targetAnimator != null)
            {
                stateNames = AnimationSpriteGenerator.GetAnimationStates(targetAnimator, selectedLayerIndex);
                selectedStateIndex = 0;
                UpdatePreview();
            }
        }

        private void UpdatePreview()
        {
            if (targetAnimator == null || renderCamera == null || stateNames.Length == 0 || previewTexture == null)
                return;

            // Setup render texture
            renderCamera.targetTexture = previewTexture;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = backgroundColor;

            // Sample animation
            string currentState = stateNames[selectedStateIndex];
            AnimationSpriteGenerator.SampleAnimation(targetAnimator, currentState, normalizedTime, selectedLayerIndex);

            // Auto-frame the object in camera view
            AnimationSpriteGenerator.AutoFrameObject(targetObject, renderCamera);

            // Render
            renderCamera.Render();

            // Cleanup
            renderCamera.targetTexture = null;

            // Force repaint
            Repaint();
        }

        private void CaptureFrame()
        {
            if (stateNames.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "No animation state selected.", "OK");
                return;
            }

            string currentState = stateNames[selectedStateIndex];
            string path = AnimationSpriteGenerator.CaptureAnimationFrame(
                targetObject,
                currentState,
                normalizedTime,
                selectedLayerIndex
            );

            if (!string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog(
                    "Success",
                    $"Animation sprite captured successfully!\n\nFile: {path}",
                    "OK"
                );

                // Ping the asset in the project window
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (asset != null)
                {
                    EditorGUIUtility.PingObject(asset);
                }
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Error",
                    "Failed to capture animation sprite. Check the console for details.",
                    "OK"
                );
            }
        }
    }
}
