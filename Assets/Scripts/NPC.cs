using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image characterImage;
    
   private int dialogueIndex;
   private bool isTyping, isDialogueActive;
   
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

       nameText.SetText(dialogueData.npcName);
       characterImage.sprite = dialogueData.npcSprite;
       
       dialoguePanel.SetActive(true);
       PauseController.SetPause(true);

       StartCoroutine(TypeLine());
   }

   void NextLine()
   {
       if (isTyping)
       {
           //skip typing anim and show full line
           StopAllCoroutines();
           dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
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
       dialogueText.SetText("");

       //typing effect
       foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
       {
           dialogueText.text += letter;
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
       dialogueText.SetText("");
       dialoguePanel.SetActive(false);
       PauseController.SetPause(false);
   }
}
