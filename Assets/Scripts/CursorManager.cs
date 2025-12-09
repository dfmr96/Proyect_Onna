using System.Collections;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Header("Cursor Sprites")]
    [SerializeField] private Texture2D defaultCursorTexture;
    [SerializeField] private Texture2D clickCursorTexture;
    [SerializeField] private Texture2D reticleCursorTexture;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;
    [SerializeField] private Vector2 reticleHotspot = new Vector2(16, 16);

    [Header("Cursor Settings")]
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private float clickDurationMs = 100f;

    private Coroutine clickCoroutine;
    private bool isChangingCursor = false;
    private CursorType currentCursorType = CursorType.Default;

    private enum CursorType
    {
        Default,
        Reticle
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ValidateCursorTexture();
        SetDefaultCursor();
    }

    private void Update()
    {
        // Solo detectar clicks cuando el cursor es Default (para menús)
        if (Input.GetMouseButtonDown(0) && !isChangingCursor && currentCursorType == CursorType.Default)
        {
            OnMouseClick();
        }
    }

    private void ValidateCursorTexture()
    {
        ValidateTexture(defaultCursorTexture, "Default");
        ValidateTexture(clickCursorTexture, "Click");
        ValidateTexture(reticleCursorTexture, "Reticle");
    }

    private void ValidateTexture(Texture2D texture, string textureName)
    {
        if (texture != null)
        {
            int width = texture.width;
            int height = texture.height;

            if (width > 128 || height > 128)
            {
                Debug.LogWarning($"[CursorManager] La textura del cursor {textureName} es muy grande ({width}x{height}). " +
                    "Tamaño recomendado: 32x32 o 64x64. El cursor puede escalarse incorrectamente.");
            }
        }
    }

    private void OnMouseClick()
    {
        if (clickCursorTexture == null) return;

        if (clickCoroutine != null)
        {
            StopCoroutine(clickCoroutine);
        }

        clickCoroutine = StartCoroutine(ClickCursorRoutine());
    }

    private IEnumerator ClickCursorRoutine()
    {
        isChangingCursor = true;

        CursorType previousCursor = currentCursorType;

        SetClickCursor();

        yield return new WaitForSecondsRealtime(clickDurationMs / 1000f);

        // Volver al cursor que estaba activo antes del click
        switch (previousCursor)
        {
            case CursorType.Default:
                SetDefaultCursor();
                break;
            case CursorType.Reticle:
                SetReticleCursor();
                break;
        }

        isChangingCursor = false;
        clickCoroutine = null;
    }

    public void SetDefaultCursor()
    {
        currentCursorType = CursorType.Default;
        if (defaultCursorTexture != null)
        {
            Cursor.SetCursor(defaultCursorTexture, cursorHotspot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    private void SetClickCursor()
    {
        if (clickCursorTexture != null)
        {
            Cursor.SetCursor(clickCursorTexture, cursorHotspot, cursorMode);
        }
    }

    public void SetReticleCursor()
    {
        currentCursorType = CursorType.Reticle;
        if (reticleCursorTexture != null)
        {
            Cursor.SetCursor(reticleCursorTexture, reticleHotspot, cursorMode);
        }
        else
        {
            Debug.LogWarning("[CursorManager] Reticle cursor texture not assigned.");
        }
    }
}

