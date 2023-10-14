using System.Runtime.CompilerServices;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwdInput;
    //动态生成
    [SerializeField] 
    private GameObject playerPrefab;
    [SerializeField] 
    private GameObject heroPrefab;

    [SerializeField] private TMP_Text roomPlayerDisplay;

    private int roomPlayers = 0;
    
    private string passwdStored;
    void Start()
    {
        CreatePlayer();
        //回调 lamada注册函数
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Debug.Log("A new client connected,id=" + id);
        };
        
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            Debug.Log("A client disconnected,id=" + id);
        };
        
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            Debug.Log("Server started");
            CreateHero();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        foreach (var info in NetworkManager.Singleton.ConnectedClientsList)
        {
            var obj = Instantiate(playerPrefab);
            info.PlayerObject = obj.GetComponent<NetworkObject>();
            //info.PlayerObject.Spawn();
            info.PlayerObject.SpawnWithOwnership(info.ClientId);
        }
    }
    
    private void CreatePlayer()
    {
        //随机生成
        //Instantiate(playerPrefab, new Vector3(3.1f, 0.2f, -0.7f), Quaternion.identity);
    }

    private void CreateHero()
    {
        GameObject ob = Instantiate(heroPrefab, new Vector3(12f, 0.46f, 6.0f), Quaternion.identity);
        ob.GetComponent<NetworkObject>().Spawn();
    }

    public void OnStartServerBtnClick()
    {
        if (NetworkManager.Singleton.StartServer())
        {
            Debug.Log("Start server suc.");
        }
        else
        {
            Debug.Log("Start server fail.");
        }
    }

    public void OnStartClientBtnClick()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += connectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += disconnectCallback;
        
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwdInput.text);
        
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Start client suc.");
        }
        else
        {
            Debug.Log("Start client fail.");
        }
    }
    
    public void OnStartHostBtnClick()
    {
        passwdStored = passwdInput.text;
        //回调
        NetworkManager.Singleton.ConnectionApprovalCallback = approvalCallback;
        
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Start host suc.");
        }
        else
        {
            Debug.Log("Start host fail.");
        }
    }
    
    public void OnShutdownNetworkBtnClick()
    {
      NetworkManager.Singleton.Shutdown();
      Debug.Log("shutdown network");
    }

    void approvalCallback(NetworkManager.ConnectionApprovalRequest req,NetworkManager.ConnectionApprovalResponse res)
    {
        if (req.ClientNetworkId.Equals(NetworkManager.Singleton.LocalClientId))
        {
            res.Approved = true;
            res.CreatePlayerObject = false;
            return;
        }
        var cId=req.ClientNetworkId;
        var payload = req.Payload;  //字符串密码
        //解析出来
        string passwd = Encoding.ASCII.GetString(payload);
        Debug.Log("new approval client="+cId+";passwd="+passwd);
        
        if (passwd.Equals(passwdStored))
        {
            res.Approved = true;
            //是否创建3d对象 
            res.CreatePlayerObject = false;
        }
        else
        {
            res.Approved = false;
            res.Reason = "passwd failed";
        }
    }

    void connectedCallback(ulong id)
    {
        Debug.Log("connected suc:"+id);
        ++roomPlayers;
        roomPlayerDisplay.SetText(roomPlayers.ToString());
    }
    
    void disconnectCallback(ulong id)
    {
        Debug.Log("connected fail:"+id);
        --roomPlayers;
        roomPlayerDisplay.SetText(roomPlayers.ToString());
    }
}

