using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    // 创建一个3D矢量，来表示玩家角色的移动
    Vector3 m_Movement;
    // 创建变量，获取用户输入的方向
    float horizontal;
    float vertical;

    //创建一个 刚体对象
    Rigidbody m_Rigibody;
    //创建一个Animator组件对象
    //public Animator m_Animator;
    public List<Animator> Animators;
    
    //用四元数对象 m_Rotation 来表示3D游戏中的旋转
    //初始化四元数对象，初始化为不旋转
    Quaternion m_Rotation = Quaternion.identity;

    //速度
    public float turnSpeed = 20.0f;
    public float walkForce = 1.0f;
    
    private NetworkVariable<Vector3> networkPlayerPos = new NetworkVariable<Vector3>(Vector3.zero);
    private NetworkVariable<Quaternion> networkPlayerRot = new NetworkVariable<Quaternion>(Quaternion.identity);

    private NetworkVariable<int> clientId = new NetworkVariable<int>();
    public GameObject playerCamera;
    
    
    void Start()
    {
        var netObject = GetComponent<NetworkObject>();
        //在游戏运行开始后，或取到刚体组件对象和动画管理者组件
        //m_Animator = GetComponent<Animator>();
        m_Rigibody = GetComponent<Rigidbody>();
        if (this.IsClient && this.IsOwner)
        {
            transform.position = new Vector3(Random.Range(-4, 5), 0.2f, -0.7f);
            playerCamera.SetActive(true);
        }
        /*if (netObject != null && (NetworkManager.ConnectedClients.Count-1) == clientId.Value)
        {
            // 这是本地玩家
            playerCamera.SetActive(true);
            Debug.Log(clientId.Value);
        }*/
        else
        {
            // 这不是本地玩家
            playerCamera.SetActive(false);
        }
        
    }

    public override void OnNetworkSpawn()
    {
        if (this.IsServer)
        {
            clientId.Value = (int)this.OwnerClientId;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.IsClient && this.IsOwner)
        {
            //获取用户输入
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //将用户输入组装成3d运动需要的三维矢量
            m_Movement.Set(horizontal, 0.0f, vertical);
            m_Movement.Normalize();

            //判断是否有横向或纵向移动
            bool hasHorizontal = !Mathf.Approximately(horizontal, 0.0f);
            bool hasVertical = !Mathf.Approximately(vertical, 0.0f);
            // 只要有一个方向移动，就认为玩家角色处于移动状态
            bool isWalking = hasHorizontal || hasVertical;
            //将变量传递给动画管理器
            foreach (var animator in Animators)
            {
                animator.SetBool("Iswalking", isWalking);
                animator.SetBool("Nowalking",!isWalking);
            }
            //用三维矢量来表示转向后的玩家角色的朝向
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            //设置四元数对象的值
            //m_Rotation = Quaternion.LookRotation(desiredForward);

            UpdatePosAndRotServerRpc(transform.position, transform.rotation);

            Vector3 m_MovementNew = transform.TransformDirection(m_Movement);
            //移动与转向
            m_Rigibody.AddForce(m_MovementNew * walkForce);
            //m_Rigibody.MoveRotation(m_Rotation);
        }
        else
        {
            m_Rigibody.MovePosition(networkPlayerPos.Value);
            m_Rigibody.MoveRotation(networkPlayerRot.Value);
        }
    }

    [ServerRpc]
    public void UpdatePosAndRotServerRpc(Vector3 pos,Quaternion rot)
    {
        networkPlayerPos.Value = pos;
        networkPlayerRot.Value = rot;
    }
    
    //减少耦合度，后面改 TODO
    private void Move()
    {
       
    }

    private void Turn()
    {
        
    }
}