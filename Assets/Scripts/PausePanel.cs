using Player;
using TMPro;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject loadCanvasPrefab;
    [SerializeField] private TextMeshProUGUI buttonText;
    private string returnToMenuText = "BACK TO MENU";
    private string quitGameText = "QUIT GAME";

    private void OnEnable()
    {
           switch (GameModeSelector.SelectedMode)
        {
            case GameMode.Hub:
                buttonText.text = quitGameText;
                break;

            default:
                buttonText.text = returnToMenuText;
                break;
        }
        //PlayerHelper.DisableInput();

        // Ocultar cursor de combate y mostrar cursor del sistema
        var customCursor = FindObjectOfType<CustomCursorUI>();
        if (customCursor != null)
            customCursor.enabled = false;

        if (CursorManager.Instance != null)
            CursorManager.Instance.SetDefaultCursor();

        CursorHelper.Show();
    }

    public void HandleButton()
    {
        switch (GameModeSelector.SelectedMode)
        {
            case GameMode.Hub:
                Application.Quit();
                break;

            default:
                SceneManagementUtils.AsyncLoadSceneByName("MainMenu", loadCanvasPrefab, this);
                break;
        }
    }

    public void ResumeGame()
    {
       // PlayerHelper.EnableInput();

        // Reactivar cursor de combate si no estamos en Hub
        bool isInHub = HubManager.Instance != null;
        if (!isInHub)
        {
            var customCursor = FindObjectOfType<CustomCursorUI>();
            if (customCursor != null)
                customCursor.enabled = true;
        }

        CursorHelper.Hide();
        transform.parent.gameObject.SetActive(false);
    }
}
