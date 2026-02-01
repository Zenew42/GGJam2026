using UnityEngine;


[CreateAssetMenu(fileName ="NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite[] linePortraits; 
   
    public string[] dialogueLines; 
    public bool[] autoProgressLines;
    public bool[] endDialogueLines; 
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public AudioClip typingSound;
    public bool[] addToPartyLines; 
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; //dialogue line where choices appear
    public string[] choices; //player responses
    public int[] nextDialogueIndexes; //where choice leads
}
