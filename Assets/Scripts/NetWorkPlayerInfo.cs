using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;

public class NetWorkPlayerInfo : NetworkBehaviour
{
    private TMP_InputField nameInput;
    
    //弄个文字预制体接受它 后面再生成
    [SerializeField] private TMP_Text nameDisplay;

    public  NetworkVariable<PlayerInfo> playerInfo = 
        new NetworkVariable<PlayerInfo>(default(PlayerInfo),NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            nameInput = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
            Debug.Log(nameInput.text);
            nameDisplay.SetText(nameInput.text);
            var newInfo = playerInfo.Value;
            newInfo.pHealth = 100;
            playerInfo.Value = newInfo;
            nameInput.onEndEdit.AddListener((str) =>
            {
                var newInfo = playerInfo.Value;
                newInfo.pName = str;
                playerInfo.Value = newInfo;
            });
        }
        else
        {
            //nameDisplay.text = netName.Value.ToString();
            nameDisplay.SetText(playerInfo.Value.pName.ToString());
        }

        //netName.OnValueChanged += updateNameValue;
        playerInfo.OnValueChanged += updateInfo;
    }
    
    void updateNameValue(FixedString128Bytes oldVal,FixedString128Bytes newVal)
    {
        nameDisplay.text = newVal.ToString();
    }

    void updateInfo(PlayerInfo oldInfo,PlayerInfo newInfo)
    {
        nameDisplay.SetText(playerInfo.Value.pName.ToString());
    }

}
public struct PlayerInfo: INetworkSerializable,System.IEquatable<PlayerInfo>
{
    public FixedString128Bytes pName;
        
    public float pHealth;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out pName);
            reader.ReadValueSafe(out pHealth);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(pName);
            writer.WriteValueSafe(pHealth);
        }
    }

    public bool Equals(PlayerInfo other)
    {
        return pName == other.pName && pHealth == other.pHealth;
    }
}
