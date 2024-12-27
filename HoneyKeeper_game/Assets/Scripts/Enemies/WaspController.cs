using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class WaspController : MonoBehaviour
{
    [SerializeField] Transform TowardObstacle;
    [SerializeField] sbyte speed;
    AudioSource Vzriv_Sound;
    bool isCanMove = true;
    int rndi;
    NavMeshAgent agent;
    void Start()
    {

        Vzriv_Sound = gameObject.GetComponent<AudioSource>();
        TowardObstacle = StaticHolder.ObstaclesToAttack[Random.Range(0, StaticHolder.ObstaclesToAttack.Count)].gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(gameObject.transform.position, TowardObstacle.transform.position);
        if(distance >= 100)
        agent.SetDestination(TowardObstacle.position);
        else
            agent.SetDestination(gameObject.transform.position);
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    //Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);
    //
    //    if (isCanMove == true)
    //    {
    //        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, towardObstacle.position, speed * Time.deltaTime);
    //        transform.LookAt(towardObstacle);
    //    }
    //    if(transform.position.y <= 2.6f)
    //    {
    //        Invoke(nameof(MoveUp),1);
    //    }
    //}
    //
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lazer"))
        {
            Destroy(agent);
            gameObject.AddComponent<Rigidbody>();
            Vzriv_Sound.Play();
            Destroy(Vzriv_Sound,1f);
            Destroy(this);
        }

        
    }    //sbyte - от -128 до 127

    public void WaspDeath()
    {
        Destroy(agent);
        gameObject.AddComponent<Rigidbody>();
        Vzriv_Sound.Play();
        Destroy(Vzriv_Sound, 1f);
        Destroy(this);
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if(other.gameObject == towardObstacle.gameObject)
    //    {
    //        isCanMove = false;
    //    }
    //    else
    //    {
    //        gameObject.transform.Rotate(0.1f,0,0);
    //        gameObject.transform.localPosition += new Vector3(0.1f, 0, 0);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == towardObstacle.gameObject)
    //    {
    //        isCanMove = true;
    //    }
    //}
    //
    //void MoveUp()
    //{
    //    gameObject.transform.position += new Vector3(0, Mathf.MoveTowards(0, 3, speed * 5 * Time.deltaTime), 0);
    //}
}
