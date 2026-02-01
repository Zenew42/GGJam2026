using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public int buttonType = 0;
    private Button button;

    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }

}
