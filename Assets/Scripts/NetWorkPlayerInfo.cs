using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;

public class NetWorkPlayerInfo : NetworkBehaviour
{
    private InputField nameInput;

    //弄个文字预制体接受它 后面再生成
    [SerializeField] private Text nameDisplay;

    private NetworkVariable<FixedString128Bytes> netName = 
        new NetworkVariable<FixedString128Bytes>("player",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
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
            nameInput = GameObject.Find("NameInput").GetComponent<InputField>();
            nameDisplay.text = nameInput.text;
            netName.Value = nameInput.text;
        }
        else
        {
            nameDisplay.text = netName.Value.ToString();
        }

        netName.OnValueChanged += updateNameValue;
    }
    
    void updateNameValue(FixedString128Bytes oldVal,FixedString128Bytes newVal)
    {
        nameDisplay.text = newVal.ToString();
    }
}
