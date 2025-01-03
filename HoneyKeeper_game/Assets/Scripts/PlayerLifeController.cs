using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] Image playerLifeStroke;
    [SerializeField] int maxLife;
    [SerializeField] float currentLife;
    [SerializeField] Transform playerStart;
    float predcurrentLife;
    float t;
    int follingDamage;

    Rigidbody rb;
   [SerializeField] float velocityY;

    [Header("מבתוךעû הכÿ סלונעט")]
    [SerializeField]GameObject deathMenu;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        currentLife = maxLife;
        playerLifeStroke.fillAmount = currentLife;
       predcurrentLife = currentLife;
    }

    private void FixedUpdate()
    {
            velocityY = rb.velocity.y;
        if (velocityY < -32)
        {
            if(velocityY < -75)
            {
            follingDamage = -100;
            }
            else { follingDamage = (int)velocityY; }
        }
        
    }
    public void UpdateLife(int lifedelta)
    {
        currentLife += lifedelta;
        playerLifeStroke.fillAmount = currentLife / 100;
        if(currentLife <= 0)
        {
            PlayerDeath();
        }
        if(currentLife < 50 && currentLife > 20)
        {
            StaticController.instance.SetMoveingSpeed(gameObject, 3,4);
        }
        if (currentLife >= 50)
        {
            StaticController.instance.SetMoveingSpeed(gameObject, 5,8);
        }
        if (currentLife <= 20 && currentLife > 0)
        {
            StaticController.instance.SetMoveingSpeed(gameObject, 2,2);
        }
        //predcurrentLife = currentLife;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("FireMelter"))
        {
            t += Time.deltaTime;
            if(t > 0.5f)
            {
                t = 0;
                UpdateLife(-1);
            }
            //UpdateLife(-50);
            //UpdateLifeCorutine(1,-50);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        UpdateLife(follingDamage);
        follingDamage = 0;
    }
    
   void PlayerDeath()
    {
        deathMenu.SetActive(true);
        StaticController.instance.TurnCursorInState(false);
        StaticController.instance.SetMoveingSpeed(gameObject,0, 0);
        Invoke(nameof(PlayerAfterdeath), 4);
    }
    void PlayerAfterdeath()
    {
        currentLife = 15;
        playerLifeStroke.fillAmount = currentLife;
        deathMenu.SetActive(false);
        StaticController.instance.TurnCursorInState(true);
        StaticController.instance.SetMoveingSpeed(gameObject, 5,8);
        gameObject.transform.position = playerStart.position;
    }
}
