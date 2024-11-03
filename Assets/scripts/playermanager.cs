using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scripts : MonoBehaviour
{
    Rigidbody2D playerRB;
    public float movespeed = 1f;
    bool facingright = true;
    Animator playerAnimator;
    public float JumpSpeed = 1f, jumpfrequency=1f, nexjumptime;//arka arkaya h�zl� s��ramalar� engellemek i�in yap�yorum.
    
    public bool isground = false;  //zemine deyerek sadece bir kere atlamay� ayarlamaya �al���yorum. ula�abilmek i�in public yapt�k
    public Transform groundcheckPosition; //sceen ekran�nda kimi kontrol etti�imizi buraya s�r�kl�yorum.
    public float groundcheckradius;   //bunlar�n hepsi zeminden z�plama i�in ama pek anlamad�m! buraya 0.1 de�erini girdik.
    public LayerMask groundchecklayer; //burada ise layer olarak ground se�tik ve b�ylece sadece oralarda z�playacak.

    public SpriteRenderer transitionSr;

    public bool kumandaCheck;

    Transform muzzle;
    public Transform bullet;
    public float bulletspeed;


    void Start()
    {

        muzzle = transform.GetChild(1);
        
        
        playerRB = this.GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        transitionSr.color = Color.black;


    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove();
        ongroundcheck();

        if (playerRB.velocity.x > 0 && facingright) //y�n de�i�tirme i�in
        {
            FlipFace();
        }
        else if (playerRB.velocity.x < 0 && !facingright)
        {
            FlipFace();
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isground) //burada ve yazd�m ��nk� yere temas�na bak�yorum z�plamak i�in.
        {
            nexjumptime = Time.timeSinceLevelLoad + jumpfrequency; //burada hep ayn� jumpspeed de�erinde atlamas�n� ayarlad�k.
            Jump();

        }
        if(!kumandaCheck) transitionSr.color = Color.Lerp(transitionSr.color, new Color(0, 0, 0, 0), Time.deltaTime * 7);//yusufla yap�lan screen i�in.

        if (kumandaCheck)
        {
            transitionSr.color = Color.Lerp(transitionSr.color, new Color(0, 0, 0, 1), Time.deltaTime * 7);
            if(transitionSr.color.a >.99f   ) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //ekran de�i�imi i�in ilkbuildindex sonra +1
        }

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        
        
        if (Input.GetMouseButtonDown(0))//sol t�ka bas�ld���n� kontrol ediyor.
        {
            shootBullet();//t�klad�ktan sonra bu �al��acak.

        }




    }
    void HorizontalMove()
    {
        playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * movespeed, playerRB.velocity.y);
        playerAnimator.SetFloat("playerspeed", Mathf.Abs(playerRB.velocity.x)); //burda mutlak de�erle x de�erini ald�k ve tan�mlad�k.


    }
    void FlipFace()
    {
        facingright = !facingright;
        Vector3 tempLocalScale = transform.localScale;
        tempLocalScale.x *= -1;
        transform.localScale = tempLocalScale;
    }
    void Jump()
    {
        playerRB.AddForce(new Vector2(0f, JumpSpeed));
    }
    void ongroundcheck()
    {
        isground = Physics2D.OverlapCircle(groundcheckPosition.position, groundcheckradius, groundchecklayer);
        //sceen ekran�ndan isgroundun tikli olup olmamas�ndan �al���p �al��mad���n�n anl�yorum.
        playerAnimator.SetBool("isgroundedanim",isground);//animasyonun �al��mas� i�in yap�ld�. bool de�i�keni animatorde.

    }

 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("kumanda")) kumandaCheck = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("kumanda")) kumandaCheck = false;

    }
    void shootBullet()
    {
        Transform tempbullet;
        tempbullet = Instantiate(bullet, muzzle.position, Quaternion.identity);
        tempbullet.GetComponent<Rigidbody2D>().AddForce(muzzle.forward * bulletspeed);

    }


}
