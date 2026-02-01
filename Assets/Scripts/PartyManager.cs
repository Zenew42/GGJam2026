using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    [Header("UI")]
    public Transform partyContainer; // UI parent
    public GameObject partyIconPrefab; // Prefab with Image component

    private List<Sprite> partyMembers = new List<Sprite>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddToParty(Sprite portrait, GameObject npcObject)
    {
        if (portrait == null)
        {
            Debug.LogWarning("Tried to add party member with no portrait.");
            return;
        }

        partyMembers.Add(portrait);

        GameObject icon = Instantiate(partyIconPrefab, partyContainer);
        icon.GetComponent<Image>().sprite = portrait;

        npcObject.transform.position = new Vector3(9999, 9999, 0);
    }
}