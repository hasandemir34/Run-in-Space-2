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
    public float JumpSpeed = 1f, jumpfrequency=1f, nexjumptime;//arka arkaya hýzlý sýçramalarý engellemek için yapýyorum.
    
    public bool isground = false;  //zemine deyerek sadece bir kere atlamayý ayarlamaya çalýþýyorum. ulaþabilmek için public yaptýk
    public Transform groundcheckPosition; //sceen ekranýnda kimi kontrol ettiðimizi buraya sürüklüyorum.
    public float groundcheckradius;   //bunlarýn hepsi zeminden zýplama için ama pek anlamadým! buraya 0.1 deðerini girdik.
    public LayerMask groundchecklayer; //burada ise layer olarak ground seçtik ve böylece sadece oralarda zýplayacak.

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

        if (playerRB.velocity.x > 0 && facingright) //yön deðiþtirme için
        {
            FlipFace();
        }
        else if (playerRB.velocity.x < 0 && !facingright)
        {
            FlipFace();
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isground) //burada ve yazdým çünkü yere temasýna bakýyorum zýplamak için.
        {
            nexjumptime = Time.timeSinceLevelLoad + jumpfrequency; //burada hep ayný jumpspeed deðerinde atlamasýný ayarladýk.
            Jump();

        }
        if(!kumandaCheck) transitionSr.color = Color.Lerp(transitionSr.color, new Color(0, 0, 0, 0), Time.deltaTime * 7);//yusufla yapýlan screen için.

        if (kumandaCheck)
        {
            transitionSr.color = Color.Lerp(transitionSr.color, new Color(0, 0, 0, 1), Time.deltaTime * 7);
            if(transitionSr.color.a >.99f   ) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //ekran deðiþimi için ilkbuildindex sonra +1
        }

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        
        
        if (Input.GetMouseButtonDown(0))//sol týka basýldýðýný kontrol ediyor.
        {
            shootBullet();//týkladýktan sonra bu çalýþacak.

        }




    }
    void HorizontalMove()
    {
        playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * movespeed, playerRB.velocity.y);
        playerAnimator.SetFloat("playerspeed", Mathf.Abs(playerRB.velocity.x)); //burda mutlak deðerle x deðerini aldýk ve tanýmladýk.


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
        //sceen ekranýndan isgroundun tikli olup olmamasýndan çalýþýp çalýþmadýðýnýn anlýyorum.
        playerAnimator.SetBool("isgroundedanim",isground);//animasyonun çalýþmasý için yapýldý. bool deðiþkeni animatorde.

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
