using Unity.Netcode;
using UnityEngine;
using TMPro;
public class RoomPlayerItem : NetworkBehaviour
{
    [SerializeField] private TMP_Text txtNameShow;
    private TMP_InputField nameInput;

    private Transform roomPlayerContainer;
    
    public  NetworkVariable<PlayerInfo> playerInfo = 
        new NetworkVariable<PlayerInfo>(default(PlayerInfo),NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    void Start()
    {
        var roomPlayerContainer = GameObject.Find("RoomPlayerList");
        GetComponent<NetworkObject>().TrySetParent(roomPlayerContainer,false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        //roomPlayerContainer = GameObject.Find("RoomPlayerList").transform;
        if (IsOwner)
        {
            nameInput = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
            var newInfo = new PlayerInfo();
            newInfo.pName = nameInput.text;
            newInfo.pHealth = 100;
            playerInfo.Value = newInfo;
            txtNameShow.SetText(nameInput.text);
            //GetComponent<NetworkObject>().TrySetParent(roomPlayerContainer);
        }
        else
        {
            txtNameShow.SetText(playerInfo.Value.pName.ToString());
            //transform.SetParent(roomPlayerContainer);
            //GetComponent<NetworkObject>().TrySetParent(roomPlayerContainer);
        }
        //解决玩家名字同步问题
        playerInfo.OnValueChanged += (PlayerInfo oldValue, PlayerInfo newValue) =>
        {
            txtNameShow.SetText(newValue.pName.ToString());
        };
    }
}
