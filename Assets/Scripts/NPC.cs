using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;
   private int dialogueIndex;
   private bool isTyping, isDialogueActive;

   private void Start()
   {
       dialogueUI = DialogueController.Instance;
   }

   public bool CanInteract()
   {
       return !isDialogueActive;
   }
  
   public void Interact()
   { 
       Debug.Log($"Interact called. Paused={PauseController.IsGamePaused}, Active={isDialogueActive}");
       if (dialogueData == null || (PauseController.IsGamePaused && !isDialogueActive))
       {
           Debug.Log("Interact blocked.");
           return;
       }

       if (isDialogueActive)
       {
           NextLine();
       }
       else
       {
           StartDialogue();
       }
   }

   void StartDialogue()
   {
       isDialogueActive = true;
       dialogueIndex = 0;
       
       dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcSprite);
       dialogueUI.ShowDialogueUI(true);
       PauseController.SetPause(true);

       StartCoroutine(TypeLine());
   }

   void NextLine()
   {
       if (isTyping)
       {
           //skip typing anim and show full line
           StopAllCoroutines();
           dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
           isTyping = false;
       }
       else if (++dialogueIndex < dialogueData.dialogueLines.Length)
       {
           //if another line, type next line
           StartCoroutine(TypeLine());
       }
       else
       {
           EndDialogue();
       }
   }

   IEnumerator TypeLine()
   {
       isTyping = true;
       dialogueUI.SetDialogueText("");

       //typing effect
       foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
       {
           dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
           yield return new WaitForSeconds(dialogueData.typingSpeed);
       }
       isTyping = false;

       if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
       {
           yield return new WaitForSeconds(dialogueData.autoProgressDelay);
           NextLine();
       }
   }

   public void EndDialogue()
   {
       StopAllCoroutines();
       isDialogueActive = false;
       dialogueUI.SetDialogueText("");
       dialogueUI.ShowDialogueUI(false);
       PauseController.SetPause(false);
   }
}
