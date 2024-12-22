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
    private void Update()
    {
        agent.SetDestination(TowardObstacle.position);
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    //Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);
    //
    //    if (isCanMove == true)
    //    {
    //        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, TowardObstacle.position, speed * Time.deltaTime);
    //        transform.LookAt(TowardObstacle);
    //    }
    //    if(transform.position.y <= 2.6f)
    //    {
    //        Invoke(nameof(MoveUp),1);
    //    }
    //}
    //
    private void OnTriggerEnter(Collider other)
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
    //private void OnTriggerStay(Collider other)
    //{
    //    if(other.gameObject == TowardObstacle.gameObject)
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
    //    if (other.gameObject == TowardObstacle.gameObject)
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
