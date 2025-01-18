using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] AudioClip clipAfterDeath;
    public static PlayerLifeController instance { get; private set; }
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
        //InvokeRepeating(nameof(UpdateLife), 15, 15);
        if(instance == null)
        { instance = this; }
        rb = gameObject.GetComponent<Rigidbody>();
        currentLife = maxLife;
        playerLifeStroke.fillAmount = currentLife;
       predcurrentLife = currentLife;
    }

    private void FixedUpdate()
    {
            velocityY = rb.velocity.y;
        if (velocityY < -25)
        {
            if(velocityY < -75)
            {
            follingDamage = -100;
            }
            else { follingDamage = (int)velocityY - 15; }
        }
        else
        {
            follingDamage = 0;
        }
        
    }
    public void UpdateLife(int lifedelta)
    {
        currentLife += lifedelta;
        playerLifeStroke.fillAmount = currentLife / 100;
        if(currentLife <= 0)
        {
            PlayerDeath();
            return;
        }
        if(currentLife < 50 && currentLife > 20)
        {
            StaticController.instance.SetMoveingSpeed(gameObject, 3,4);
        }
        if (currentLife >= 50)
        {
            StaticController.instance.SetMoveingSpeed(gameObject, 8,8);
        }
        if (currentLife <= 20 && currentLife > 0)
        {
            StaticController.instance.SetMoveingSpeed(gameObject, 2,2);
        }
        if(currentLife > maxLife)
        {
            currentLife = maxLife;
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
        //int rndpredmet = Random.Range(0, 16);
       //for(int i = 0; i < ObjectInHandController.instance.objectsInHand.Count; i++)      
       //{
       //
       //}
        //StaticController.instance.MuteAllAudioSources();
        deathMenu.SetActive(true);
        StaticController.instance.TurnCursorInState(false);
        StaticController.instance.SetMoveingSpeed(gameObject,0,0);
        ObjectInHandController.instance.SetObject(1);
        ObjectInHandController.instance.DropObjectInhand(1);
        gameObject.transform.position = new Vector3(0, 3000, 0);
        rb.velocity = Vector3.zero;
        Invoke(nameof(PlayerAfterdeath), 4);
        Debug.Log("Umer");
    }
    void PlayerAfterdeath()
    {
        rb.velocity = Vector3.zero;
        StaticController.instance.RestoreAudioSources();
        currentLife = 15;
        playerLifeStroke.fillAmount = currentLife;
        deathMenu.SetActive(false);
        StaticController.instance.TurnCursorInState(true);
        StaticController.instance.SetMoveingSpeed(gameObject, 5,8);
        gameObject.transform.position = playerStart.position;
        StaticController.instance.RestoreAudioSources();
        StaticClipsController.instance.ActivateClip(clipAfterDeath);
    }
}
