using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmanager : MonoBehaviour
{

    public float BulletDamage, Bullettime; //merminin hasar�n� ve s�resini verdim �nce

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Bullettime); //yok etmek i�in gereken s�reyi vercez zaten bu da fonksiyon.

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
