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
        SceneManager.LoadScene("Tilemapping scene");
    }

}
