using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatIndicator : MonoBehaviour
{
    private HeatProvider heatProvider;
    [SerializeField] private GameObject indicator;
    //material of the indicator
    [SerializeField] private Material indicatorMaterialOn;
    [SerializeField] private Material indicatorMaterialOff;
    [SerializeField] private ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        heatProvider = GetComponent<HeatProvider>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (heatProvider == null)
        {
            Debug.Log("HeatIndicator has no HeatProvider");
            enabled = false;
            return;
        }
        if (indicator == null || indicatorMaterialOn == null || indicatorMaterialOff == null)
        {
            Debug.Log("HeatIndicator has no indicator or no material");
            enabled = false;
            return;
        }
        
        indicator.GetComponent<MeshRenderer>().material = indicatorMaterialOff;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (heatProvider.IsActive())
        {
            indicator.GetComponent<MeshRenderer>().material = indicatorMaterialOn;
            if (particleSystem!= null && !particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        else
        {
            indicator.GetComponent<MeshRenderer>().material = indicatorMaterialOff;
            if (particleSystem!= null && particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }
        
    }
}
