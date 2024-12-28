using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainWaspController : MonoBehaviour
{
    [SerializeField] Transform towardObstacle;
    [SerializeField] sbyte speed;
    AudioSource Vzriv_Sound;
    bool isCanMove = true;
    protected int rndi;
   protected NavMeshAgent agent { get; private set; }
    protected float distance { get; private set; }
    public Transform TowardObstacle { get => towardObstacle;}

    void Start()
    {
        Vzriv_Sound = gameObject.GetComponent<AudioSource>();
        towardObstacle = StaticHolder.ObstaclesToAttack[Random.Range(0, StaticHolder.ObstaclesToAttack.Count)].gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(towardObstacle.position);
    }
    protected virtual void FixedUpdate()
    {
        distance = Vector3.Distance(gameObject.transform.position, towardObstacle.transform.position);
            //agent.SetDestination(towardObstacle.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lazer"))
        {
            Destroy(agent);
            gameObject.AddComponent<Rigidbody>();
            Vzriv_Sound.Play();
            Destroy(Vzriv_Sound, 1f);
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
}
