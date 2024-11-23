using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FlowersFresh : MonoBehaviour
{
    [SerializeField] private float freshment;
    [SerializeField] private Slider freshmentSlider;
    [SerializeField] private ParticleSystem water;

    private void Start()
    {
        freshment = 100.0f;
        if (freshmentSlider != null)
        {
            freshmentSlider.maxValue = 100.0f; 
            freshmentSlider.value = freshment;
        }
    }

    private void Update()
    {
        freshment -= Time.deltaTime;
        freshment = Mathf.Clamp(freshment, 0, 100);

        if (freshmentSlider != null)
        {
            freshmentSlider.value = freshment;
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            Refresher();
        }
    }

    public void Refresher()
    {
        water.Play();
        freshment += 10;
    }

}
