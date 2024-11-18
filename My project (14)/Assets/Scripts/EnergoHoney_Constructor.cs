using UnityEngine;

public class HoneySpawner : MonoBehaviour
{
    public GameObject energyHoneyPrefab; 

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Honey")) 
        {

            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
            Vector3 velocity = otherRigidbody != null ? otherRigidbody.velocity : Vector3.zero;
            SpawnEnergyHoney(other.transform.position, velocity);
            Destroy(other.gameObject);
        }
    }

    private void SpawnEnergyHoney(Vector3 position, Vector3 velocity)
    {
   
        GameObject newEnergyHoney = Instantiate(energyHoneyPrefab, position, Quaternion.identity);

 
        Rigidbody newHoneyRigidbody = newEnergyHoney.GetComponent<Rigidbody>();
        if (newHoneyRigidbody != null)
        {
            newHoneyRigidbody.velocity = velocity; 
        }


    }
}