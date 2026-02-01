using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public int buttonType = 0;
    public Button m_Startbutton;

    void Start()
    {
        m_Startbutton.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (buttonType == 1)
        {
            SceneManager.LoadScene("Tilemapping scene");
        }
        else if (buttonType == 2)
        {
            Debug.Log("Quittin");
            Application.Quit();
        }
    }

}
