using UnityEngine;
using UnityEngine.UI;

public class CustomCursorUI : MonoBehaviour
{
    private MouseGroundAiming mouseGroundAiming;
    private Camera playerCamera;

    [Header("Weapon Height")]
    [SerializeField] private Transform weaponHeightReference;
    [SerializeField] private float weaponHeight = 1.5f;

    [Header("Cursor Sprite")]
    [SerializeField] private Sprite reticleCursorSprite;

    [Header("Debug")]
    [SerializeField] private bool showDebugGizmos = false;

    // Canvas y UI
    private Canvas cursorCanvas;
    private GameObject cursorUIObject;
    private Image cursorImage;
    private RectTransform cursorRectTransform;

    private void Awake()
    {
        mouseGroundAiming = GetComponent<MouseGroundAiming>();
        if (mouseGroundAiming == null)
        {
            Debug.LogError("[CustomCursorUI] MouseGroundAiming not found on same GameObject!");
        }

        playerCamera = Camera.main;

        CreateCursorCanvas();
    }

    private void CreateCursorCanvas()
    {
        // Crear Canvas
        cursorCanvas = new GameObject("CombatCursorCanvas").AddComponent<Canvas>();
        cursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        cursorCanvas.sortingOrder = 1000;

        CanvasScaler scaler = cursorCanvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        cursorCanvas.gameObject.AddComponent<GraphicRaycaster>();

        // Crear cursor Image
        cursorUIObject = new GameObject("ReticleCursor");
        cursorUIObject.transform.SetParent(cursorCanvas.transform);

        cursorImage = cursorUIObject.AddComponent<Image>();
        cursorImage.raycastTarget = false;
        cursorImage.sprite = reticleCursorSprite;
        cursorImage.enabled = false;

        cursorRectTransform = cursorUIObject.GetComponent<RectTransform>();
        cursorRectTransform.anchorMin = Vector2.zero;
        cursorRectTransform.anchorMax = Vector2.zero;
        cursorRectTransform.pivot = new Vector2(0.5f, 0.5f);
        cursorRectTransform.sizeDelta = new Vector2(64, 64);
    }

    private void OnEnable()
    {
        // No activar cursor de combate en el Hub
        bool isInHub = HubManager.Instance != null;
        if (isInHub)
        {
            if (cursorImage != null)
                cursorImage.enabled = false;
            return;
        }

        // Ocultar cursor del sistema
        Cursor.visible = false;

        // Mostrar cursor UI
        if (cursorImage != null)
            cursorImage.enabled = true;
    }

    private void OnDisable()
    {
        // Ocultar cursor UI
        if (cursorImage != null)
            cursorImage.enabled = false;

        // Restaurar cursor del sistema (si no es Hub)
        if (HubManager.Instance == null)
        {
            if (CursorManager.Instance != null)
                CursorManager.Instance.SetDefaultCursor();
            Cursor.visible = true;
        }
    }

    private void OnDestroy()
    {
        // Destruir Canvas cuando el Player se destruye
        if (cursorCanvas != null)
            Destroy(cursorCanvas.gameObject);
    }

    private void LateUpdate()
    {
        // No actualizar cursor en el Hub
        bool isInHub = HubManager.Instance != null;
        if (isInHub)
            return;

        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        if (mouseGroundAiming == null || mouseGroundAiming.aimTarget == null || playerCamera == null)
            return;

        Vector3 screenPos = CalculateElevatedScreenPosition();

        if (cursorRectTransform != null)
        {
            if (screenPos.z > 0)
            {
                cursorRectTransform.position = screenPos;
                if (cursorImage != null)
                    cursorImage.enabled = true;
            }
            else
            {
                if (cursorImage != null)
                    cursorImage.enabled = false;
            }
        }
    }

    private Vector3 CalculateElevatedScreenPosition()
    {
        // 1. Obtener aimTarget (en suelo)
        Vector3 groundTarget = mouseGroundAiming.aimTarget.position;

        // 2. Calcular punto elevado
        float heightOffset = GetWeaponHeight();
        Vector3 elevatedTarget = new Vector3(
            groundTarget.x,
            groundTarget.y + heightOffset,
            groundTarget.z
        );

        // 3. Proyectar a pantalla
        return playerCamera.WorldToScreenPoint(elevatedTarget);
    }

    private float GetWeaponHeight()
    {
        if (weaponHeightReference != null && mouseGroundAiming != null && mouseGroundAiming.playerTransform != null)
        {
            return weaponHeightReference.position.y - mouseGroundAiming.playerTransform.position.y;
        }
        return weaponHeight;
    }

    private void OnDrawGizmos()
    {
        if (!showDebugGizmos || !Application.isPlaying) return;
        if (mouseGroundAiming == null || mouseGroundAiming.aimTarget == null) return;

        Vector3 groundTarget = mouseGroundAiming.aimTarget.position;

        // Punto en el suelo (aimTarget)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundTarget, 0.3f);

        // Punto elevado (donde apunta visualmente)
        float heightOffset = GetWeaponHeight();
        Vector3 elevatedTarget = new Vector3(
            groundTarget.x,
            groundTarget.y + heightOffset,
            groundTarget.z
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(elevatedTarget, 0.3f);

        // Línea conectando ambos
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundTarget, elevatedTarget);

        // weaponHeightReference si existe
        if (weaponHeightReference != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(weaponHeightReference.position, 0.2f);
        }
    }
}
