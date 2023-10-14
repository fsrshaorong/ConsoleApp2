using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : LifeObject
{
    public Image bar;
    public float hpChange=20f;
    
    private float maxhealth=100f;
    public float health=100f;

    void Start()
    {
        
    }

    // Update is called once per frame
    
    void Update()
    {
        bar.fillAmount = health / maxhealth;
    }
    

    public void addHealth()
    {
        health += hpChange;
    }

    public void reduceHealth()
    {
        health -= hpChange;
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("bullet"))
        {
            BeAtk();
        }
    }

    public override void BeAtk()
    {
        reduceHealth();
    }
}
