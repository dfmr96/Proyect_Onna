using System;
using Player;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private DialogueUI dialogueUI;
    private DialogueNode currentNode;
    private NPCData currentNPCData;
    public InteractableBase CurrentTrigger { get; private set; }
    
    public Action CurrentTriggerActionOnEnd;

    [SerializeField] private DialogueUI dialogueUIPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(NPCData npcData, InteractableBase trigger = null)
    {
        if (npcData == null || npcData.StartingDialogue == null)
            return;

        CurrentTrigger = trigger;
        currentNPCData = npcData;
        currentNode = npcData.StartingDialogue;

        // Cambiar a cursor default para diálogos/menús
        if (CursorManager.Instance != null)
            CursorManager.Instance.SetDefaultCursor();

        CursorHelper.Show();


        if (dialogueUI == null)
            dialogueUI = Instantiate(dialogueUIPrefab);

        dialogueUI.gameObject.SetActive(true);
        dialogueUI.SetName(npcData.NpcName);
        dialogueUI.SetImage(npcData.NpcImage);
        dialogueUI.BindActions(currentNode);
        dialogueUI.DisplayNode(currentNode, OnOptionSelected);

        PlayerHelper.DisableInput();
        PlayerHelper.GetPlayer()?.GetComponent<PlayerModel>().SetInvulnerable(true);
    }

    private void OnOptionSelected(int index)
    {
        var option = currentNode.Options[index];
        option.onSelectedAction?.Invoke();

        if (option.endsDialogue)
        {
            EndDialogue();
            return;
        }

        if (option.nextNode != null)
        {
            currentNode = option.nextNode;
            dialogueUI.BindActions(currentNode);
            dialogueUI.DisplayNode(currentNode, OnOptionSelected);
        }
    }

    public void EndDialogue()
    {
        if (dialogueUI != null)
            dialogueUI.gameObject.SetActive(false);

        CurrentTrigger = null;
        currentNode = null;

        PlayerHelper.EnableInput();
        PlayerHelper.GetPlayer()?.GetComponent<PlayerModel>().SetInvulnerable(false);

        // Determinar qué cursor usar al cerrar el diálogo
        bool isInHub = HubManager.Instance != null;
        bool isStoreOpen = isInHub && HubManager.Instance.IsStoreOpen;

        if (isStoreOpen)
        {
            // En la tienda, mantener cursor visible y default
            return;
        }

        if (isInHub)
        {
            // En el Hub, ocultar cursor
            CursorHelper.Hide();
        }
        else
        {
            // En gameplay, cambiar a retícula y mostrar
            if (CursorManager.Instance != null)
            {
                CursorManager.Instance.SetReticleCursor();
                CursorHelper.Show();
            }
            else
            {
                CursorHelper.Hide();
            }
        }
    }

        
    public System.Collections.IEnumerator PreTutorialTimer(NPCData nextData, InteractableBase trigger)
    {
        EndDialogue();
        yield return new WaitForSeconds(5f);
        StartDialogue(nextData, trigger);
    }

    public void SetCurrentNPCData(NPCData newData) => currentNPCData = newData;
}
