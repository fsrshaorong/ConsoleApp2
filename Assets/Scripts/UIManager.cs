using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : NetworkBehaviour
{
    //TODO 将btnPanel移到UIScene，然后将LobbyUIScene设为 isServer 才开，还要同步
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