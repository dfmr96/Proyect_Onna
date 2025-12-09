using System.Collections;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Header("Cursor Sprites")]
    [SerializeField] private Texture2D defaultCursorTexture;
    [SerializeField] private Texture2D clickCursorTexture;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    [Header("Cursor Settings")]
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private float clickDurationMs = 100f;

    private Coroutine clickCoroutine;
    private bool isChangingCursor = false;

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
        if (Input.GetMouseButtonDown(0) && !isChangingCursor)
        {
            OnMouseClick();
        }
    }

    private void ValidateCursorTexture()
    {
        if (defaultCursorTexture != null)
        {
            int width = defaultCursorTexture.width;
            int height = defaultCursorTexture.height;

            if (width > 128 || height > 128)
            {
                Debug.LogWarning($"[CursorManager] La textura del cursor es muy grande ({width}x{height}). " +
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

        SetClickCursor();

        yield return new WaitForSecondsRealtime(clickDurationMs / 1000f);

        SetDefaultCursor();

        isChangingCursor = false;
        clickCoroutine = null;
    }

    public void SetDefaultCursor()
    {
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
}

