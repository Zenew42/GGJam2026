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
   private bool shouldJoinParty = false;
   private Sprite joinPortrait = null;

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

       dialogueUI.SetNPCInfo(dialogueData.npcName, null); 
       dialogueUI.ShowDialogueUI(true);
       PauseController.SetPause(true);

       DisplayCurrentLine();
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
           
       dialogueUI.ClearChoices();

       if (dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
       {
           EndDialogue();
           return;
       }

       foreach (DialogueChoice dialogueChoice in dialogueData.choices)
       {
           if (dialogueChoice.dialogueIndex == dialogueIndex)
           {
               DisplayChoices(dialogueChoice);
               return;
           }
       }
           
       if (++dialogueIndex < dialogueData.dialogueLines.Length)
       {
           //if another line, type next line
           DisplayCurrentLine();
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

   void DisplayChoices(DialogueChoice choice)
   {
       for (int i = 0; i < choice.choices.Length; i++)
       {
           int nextIndex = choice.nextDialogueIndexes[i];
           dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
       }
   }

   void ChooseOption(int nextIndex)
   {
       dialogueIndex = nextIndex;
       dialogueUI.ClearChoices();
       DisplayCurrentLine();
   }

   void DisplayCurrentLine()
   {
       StopAllCoroutines();

       if (dialogueData.linePortraits != null &&
           dialogueIndex < dialogueData.linePortraits.Length)
       {
           dialogueUI.characterImage.sprite = dialogueData.linePortraits[dialogueIndex];
       }

       if (dialogueData.addToPartyLines != null &&
           dialogueIndex < dialogueData.addToPartyLines.Length &&
           dialogueData.addToPartyLines[dialogueIndex])
       {
           shouldJoinParty = true;
           joinPortrait = dialogueData.linePortraits[dialogueIndex];
       }

       StartCoroutine(TypeLine());
   }
   
   public void EndDialogue()
   {
       StopAllCoroutines();
       isDialogueActive = false;
       dialogueUI.SetDialogueText("");
       dialogueUI.ShowDialogueUI(false);
       PauseController.SetPause(false);

       // Perform party join after dialogue ends
       if (shouldJoinParty && joinPortrait != null)
       {
           PartyManager.Instance.AddToParty(joinPortrait, this.gameObject);
       }
   }
}
