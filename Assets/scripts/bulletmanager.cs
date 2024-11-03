using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmanager : MonoBehaviour
{

    public float BulletDamage, Bullettime; //merminin hasarýný ve süresini verdim önce

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Bullettime); //yok etmek için gereken süreyi vercez zaten bu da fonksiyon.

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
