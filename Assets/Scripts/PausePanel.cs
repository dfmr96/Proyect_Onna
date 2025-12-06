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
        PlayerHelper.DisableInput();
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
        PlayerHelper.EnableInput();
        CursorHelper.Hide();
        transform.parent.gameObject.SetActive(false);
    }
}
