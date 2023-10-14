using Unity.Netcode;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform barrelEnd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void buttonFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           Fire();
        }
    }

    public void autoFire()
    {
        Fire();
    }

    public void Fire()
    {
        GameObject rocketInstance;
        rocketInstance=Instantiate(rocketPrefab,barrelEnd.position,barrelEnd.rotation);
        rocketInstance.GetComponent<NetworkObject>().Spawn();
        rocketInstance.GetComponent<Rigidbody>().AddForce(barrelEnd.forward*200);
    }
}
