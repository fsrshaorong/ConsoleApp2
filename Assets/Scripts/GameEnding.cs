using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class GameEnding : MonoBehaviour
{
    public Image PlayerBar;
    public Image EnemyBar;
    public GameObject WinCanva;
    public GameObject LoseCanva;

    private float m_Timer;
    public float displayImageDuration=2f;
    
    /*public void LoadUIScene()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    public void UnloadUIScene()
    {
        SceneManager.UnloadSceneAsync("LobbbyUL");
    }*/

    public void GameWin()
    {
        if (EnemyBar.fillAmount == 0)
        {
            m_Timer += Time.deltaTime;
            WinCanva.SetActive(true);
            if (m_Timer>displayImageDuration)
            {
                //TODO 后面写慢慢淡出
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }

    public void GameLose()
    {
        if (PlayerBar.fillAmount == 0)
        {
            m_Timer += Time.deltaTime;
            LoseCanva.SetActive(true);
            if (m_Timer>displayImageDuration)
            {
                //LoadUIScene();
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }
    
    void Update()
    {
        GameWin();
        GameLose();
    }

}
