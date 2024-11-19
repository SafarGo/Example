using System.Collections;
using UnityEngine;

public class HoneySpawner : MonoBehaviour
{
    public GameObject energyHoneyPrefab;
    [SerializeField] int HoneyDelSpeed;
    int medCaunter;
    [SerializeField] int cartridgeCount;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Honey")) 
        {

            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
            Vector3 velocity = otherRigidbody != null ? otherRigidbody.velocity : Vector3.zero;
            Destroy(other.gameObject);
            medCaunter++;
                if (medCaunter == 2)
            {
                medCaunter = 0;
                StartCoroutine(SpawnEnergyHoney(other.transform.position, velocity));
            }
        }
    }

    private IEnumerator SpawnEnergyHoney(Vector3 position, Vector3 velocity)
    {

        yield return new WaitForSeconds(HoneyDelSpeed / cartridgeCount);
        GameObject newEnergyHoney = Instantiate(energyHoneyPrefab, position, Quaternion.identity);

 
        Rigidbody newHoneyRigidbody = newEnergyHoney.GetComponent<Rigidbody>();
        if (newHoneyRigidbody != null)
        {
            newHoneyRigidbody.velocity = velocity; 
        }


    }
}