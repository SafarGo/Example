using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MainWaspController : MonoBehaviour
{
    [SerializeField] protected Transform towardObstacle;
    [SerializeField] protected sbyte speed;
    [SerializeField] protected GameObject objectToSpawnAfterDeath;
    protected AudioSource vzrivSound;
    protected NavMeshAgent agent;
    protected float distance;

    public Transform TowardObstacle => towardObstacle;

    protected virtual void Start()
    {
        vzrivSound = GetComponent<AudioSource>();
        towardObstacle = StaticHolder.ObstaclesToAttack[Random.Range(0, StaticHolder.ObstaclesToAttack.Count)].transform;

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(towardObstacle.position);
    }

    protected virtual void FixedUpdate()
    {
        if (towardObstacle != null)
        {
            distance = Vector3.Distance(transform.position, towardObstacle.position);
        }
    }

    public virtual void WaspDeath()
    {
        if (objectToSpawnAfterDeath != null)
        {
            Instantiate(objectToSpawnAfterDeath, transform.position, Quaternion.identity);
        }

        if (agent != null)
        {
            Destroy(agent);
        }

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        if (vzrivSound != null)
        {
            vzrivSound.Play();
            Destroy(vzrivSound, vzrivSound.clip.length);
        }

        Destroy(gameObject, 1f);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Для будущего использования
        // Например:
        // if (other.CompareTag("Laser")) WaspDeath();
    }
}
