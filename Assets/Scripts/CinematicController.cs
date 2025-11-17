using System.Collections.Generic;
using Player;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class CinematicController : MonoBehaviour
{
    [SerializeField] GameObject loadCanvasPrefab;
    [SerializeField] private string nextScene;
    
    [Header("Sprites de la cinemática")]
    [SerializeField] private List<Sprite> cinematicSprites;

    [Header("UI de pantalla")]
    [SerializeField] private Image cinematicImage;

    [Header("Texto de instrucción")]
    [SerializeField] private bool isOn = true;
    [SerializeField] private CanvasGroup pressTextCanvasGroup;
    [SerializeField] private float waitTimeToShowText = 2f;
    [SerializeField] private float textPulseSpeed = 2f;
    [SerializeField] private float advanceCooldown = 0.2f;


    private int currentIndex = 0;
    private float pulseStartTime;


    private float nextAdvanceTime = 0f;

    //Timer para mostrar el texto
    private float idleTimer = 0f;
    private bool isShowingText = false;

    private void Start()
    {
        if (cinematicSprites == null || cinematicSprites.Count == 0 || cinematicImage == null)
        {
            Debug.LogError("Faltan referencias o sprites en la cinemática.");
            return;
        }

        pressTextCanvasGroup.alpha = 0f;

        ShowCurrentSprite();
    }

    //private void Update()
    //{
    //    idleTimer += Time.deltaTime;

    //    //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
    //    //{
    //    //    AdvanceCinematic();
    //    //}
    //}

    private void Update()
    {
        idleTimer += Time.deltaTime;

        // Mostrar texto si pasan X segundos sin tocar nada
        if (isOn && !isShowingText && idleTimer >= waitTimeToShowText)
        {
            isShowingText = true;
            pulseStartTime = Time.time; //reset del pulso
        }

        // Efecto pulsante
        if (isShowingText)
        {
            pressTextCanvasGroup.alpha = (Mathf.Sin((Time.time - pulseStartTime) * textPulseSpeed) + 1f) / 2f;
        }

        // INPUT + cooldown
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            && Time.time >= nextAdvanceTime)
        {
            nextAdvanceTime = Time.time + advanceCooldown;
            idleTimer = 0f; // resetea el timer
            HidePressText();
            AdvanceCinematic();
        }
    }

    private void HidePressText()
    {
        isShowingText = false;
        pressTextCanvasGroup.alpha = 0f;
    }

    private void AdvanceCinematic()
    {
        currentIndex++;
        if (currentIndex > cinematicSprites.Count) return;

        if (currentIndex >= cinematicSprites.Count)
        {
            //Save intro status
            SaveSystem.MarkIntroSeen();

            GameModeSelector.SelectedMode = GameMode.Hub;
            SceneManagementUtils.AsyncLoadSceneByName(nextScene, loadCanvasPrefab, this);
            return;
        }

        ShowCurrentSprite();
    }

    private void ShowCurrentSprite()
    {
        cinematicImage.sprite = cinematicSprites[currentIndex];
    }
}