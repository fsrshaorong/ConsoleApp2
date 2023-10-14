using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    void Start()
    {
        LoadLobbyUIScene();
    }
    
    public void LoadLobbyUIScene()
    {
        SceneManager.LoadScene("LobbbyUL", LoadSceneMode.Additive);
    }

    public void UnloadLobbyUIScene()
    {
        SceneManager.UnloadSceneAsync("LobbbyUL");
    }

    void Update()
    {
        
    }
    
}