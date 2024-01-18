using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Food : MonoBehaviour
{
    [SerializeField] private string foodName = "undefined";
    [SerializeField] private GameObject cutFoodPrefab;
    [SerializeField] private int cutCount = 2;
    [SerializeField] private float cutOffset = 0.1f;
    [SerializeField] private float cutForce = 3f;
    [SerializeField] public bool canBeCooked = true;
    
    [SerializeField] private float burnTemperature = 100f;
    [SerializeField] private float minTemperature = -18f;
    [SerializeField] private float frozenTemperature = -10f;
    [SerializeField] private float cookTemperature = 50f;
    [SerializeField] private float cookedPrecision = 4.5f;
    
    [SerializeField] private float heatTransfer = 0.04f;
    [SerializeField] private GameObject gaugePrefab;
    private GameObject gauge;
    [SerializeField] private float gaugeOffset = 0.5f;
    private bool isHeating = false;
    //is cooking cooldown
    private float heatingCooldown = 0.5f;
    private float heatingCooldownTimer = 0f;
    public Food bottomFood;
    public Food topFood;
    
    private SocketWithTag socketInteractor;
    [SerializeField] private Material socketMaterial;
    
    
    
    [SerializeField] private Color frozenColor = Color.blue;
    [SerializeField] private Color burnedColor = Color.black;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color cookedColor = Color.yellow;
    private float temperture = GameManager.roomTemperature;
    private bool burned;
    
    private Rigidbody rb;
    private MeshCollider col1;
    private MeshCollider col2;
    private XRGrabInteractable grabInteractable;
    private bool isCut;
    public bool justSpawned = true;
    private float spawnTimer = 3.0f;

    public void recursiveDelete()
    {
        
    }
    
    public List<GameObject> getStack()//fin de la liste = élement au fond de la pile
    {
        if (topFood == null)
        {
            var stack = new List<GameObject> { gameObject };
            return stack;
        }
        else
        {
            var stack = topFood.getStack();
            stack.Add(gameObject);
            return stack;
        }
    }
    public string GetName()
    {
        return foodName;
    }
    
    private void Cut()
    {
        for(int i = 0; i < cutCount; i++)
        {
            Vector3 offset = UnityEngine.Random.onUnitSphere;
            offset.z = 0;
            offset = offset.normalized * cutOffset;
            GameObject food = Instantiate(cutFoodPrefab, transform.position + offset, Quaternion.identity);
            if (food.GetComponent<Food>() == null)
            {
                Debug.Log("Food has no Food component");
                food.AddComponent<Food>();
            }
            food.GetComponent<Food>().temperture = temperture;
        }
        Destroy(gameObject);
    }
    
    public bool IsCooked()
    {
        return temperture >= cookTemperature - cookedPrecision;
    }
    public bool IsBurned()
    {
        return burned;
    }
    public bool IsFrozen()
    {
        return temperture <= frozenTemperature;
    }
    public bool IsNormal()
    {
        return temperture > frozenTemperature && temperture < cookTemperature - cookedPrecision;
    }
    public void FixRecursion()
    {
        if (topFood == null) return;
        var found = new List<Food> { this };
        while (found.Last().topFood != null && !found.Contains(found.Last().topFood) )
        {
            found.Add(found.Last().topFood);
        }
        
        if(found.Last().topFood == null) return;
        if(found.Last().topFood == this)
        {
            topFood = null;
        }
        else topFood.FixRecursion();
    }
    public bool HasRecursion()
    {
        if (topFood == null) return false;
        var found = new List<Food> { this };
        while (found.Last().topFood != null && !found.Contains(found.Last().topFood) )
        {
            found.Add(found.Last().topFood);
        }
        return found.Last().topFood != null;
    }
    
    public void DisableRigidbody(bool recursive, int maxrecursion = 20)
    
    {
        if (maxrecursion <= 0)
        {
            FixRecursion();
            maxrecursion = 20;
        }
        if (rb == null || justSpawned ) return;
        rb.excludeLayers = ~0 - LayerMask.GetMask("Default");
        if(recursive && topFood != null)
            topFood.DisableRigidbody(true, maxrecursion - 1);
    }
    public void EnableRigidbody(bool recursive, int maxrecursion = 20)
    {
        if (maxrecursion <= 0)
        {
            FixRecursion();
            maxrecursion = 20;
        }
        if (rb == null || justSpawned) return;
        rb.excludeLayers = 0;
        if(recursive && topFood != null)
            topFood.EnableRigidbody(true, maxrecursion - 1);
    }

    private void Start()
    {
        //Set tags and layers as Food
        gameObject.tag = "Food";
        gameObject.layer = LayerMask.NameToLayer("Food");
        Transform top = transform.Find("Top");
        //create socket interactor
        socketInteractor = top.GetComponent<SocketWithTag>();
        if (socketInteractor == null)
            socketInteractor = top.gameObject.AddComponent<SocketWithTag>();
        socketInteractor.socketTag = "Food";
        if(socketMaterial != null)
            socketInteractor.interactableHoverMeshMaterial  = socketMaterial;
        socketInteractor.interactableCantHoverMeshMaterial = null;
        socketInteractor.interactionLayers = InteractionLayerMask.GetMask("Burger");
        socketInteractor.food = this;

        //get child named top and set it as socket attach transform

        
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }
/*
        if (GetComponents<MeshCollider>().Length == 2)
        {
            col1 = GetComponents<MeshCollider>()[0];
            col2 = GetComponents<MeshCollider>()[1];
        }
        else
        {
            col1 = GetComponent<MeshCollider>();
            if (col1 == null)
            {
                col1 = gameObject.AddComponent<MeshCollider>();
                col1.convex = true;
                col1.isTrigger = true;
            }
            else
            {
                col1.convex = true;
                col1.isTrigger = true;
            }
            col2 = gameObject.AddComponent<MeshCollider>();
            col2.convex = true;
            col2.isTrigger = false;
            
        }
*/
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
            grabInteractable.movementType = XRGrabInteractable.MovementType.VelocityTracking;
            grabInteractable.throwOnDetach = false;
            grabInteractable.smoothPosition = true;
        }
        grabInteractable.interactionLayers = InteractionLayerMask.GetMask("Burger");
        grabInteractable.selectEntered.AddListener((args) =>
        {
            if(topFood != null)
                topFood.DisableRigidbody(true);
        });
        grabInteractable.selectExited.AddListener((args) =>
        {
            if(topFood != null)
                topFood.EnableRigidbody(true);
        });
        if (gaugePrefab == null)
        {
            Debug.Log("No gauge prefab");
            return;
        }
        gauge = Instantiate(gaugePrefab, transform.position, Quaternion.identity);
        gauge.GetComponent<gauge>().goalValue = (cookTemperature - frozenTemperature) / (burnTemperature - frozenTemperature);
        gauge.SetActive(false);
        
        
        
        
        
    }
    public void Heat(float value)
    {
        if (!canBeCooked)
        {
            return;
        }
        Debug.Log("Heat");
        temperture += value;
        if (!isHeating)
        {
            gauge.SetActive(true);
        }
        
        isHeating = true;
        heatingCooldownTimer = heatingCooldown;
        
    }
    public void Cool(float value)
    {
        temperture -= value;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            justSpawned = false;
        }
        if (bottomFood == null && rb.excludeLayers != 0)
        {
            rb.excludeLayers = 0;
        }
        
        //Cooking
        if (!canBeCooked) return;
        float maskStrength = 0.5f;
        if (isHeating)
        {
            if (gauge != null)
            {
                gauge.GetComponent<gauge>().gaugeValue = (temperture - frozenTemperature) / (burnTemperature - frozenTemperature);
            }
            heatingCooldownTimer -= Time.deltaTime;
            if (heatingCooldownTimer <= 0 )
            {
                isHeating = false;
                gauge.SetActive(false);

            }
        }
        if(gauge != null)
            gauge.transform.position = transform.position + Vector3.up * gaugeOffset;
        if (temperture > burnTemperature)
        {
            temperture = burnTemperature;
            burned = true;
        }
        if (temperture < minTemperature)
        {
            temperture = minTemperature;
        }
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.Log("Food has no mesh renderer");
                return;
            }
        }
        if (IsBurned())
        {
            meshRenderer.material.color = maskStrength * burnedColor + (1 - maskStrength) * normalColor;
        }
        else if (IsFrozen())
        {
            meshRenderer.material.color = maskStrength * frozenColor + (1 - maskStrength) * normalColor;
            
        }
        else if (IsCooked())
        {
            meshRenderer.material.color = maskStrength * cookedColor + (1 - maskStrength) * normalColor;
        }
        else
        {
            meshRenderer.material.color = normalColor;
        }
        //apply heat transfer
        if (temperture > GameManager.roomTemperature)
        {
            temperture -= heatTransfer * Time.deltaTime;
        }
        else if (temperture < GameManager.roomTemperature)
        {
            temperture += heatTransfer * Time.deltaTime;
        }
        
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Knife"))
        {
            //get velocity of knife and self
            Vector3 knifeVelocity = other.attachedRigidbody.velocity;
            Vector3 foodVelocity = GetComponent<Rigidbody>().velocity;
            Vector3 cutVelocity = knifeVelocity - foodVelocity;
            if (cutVelocity.magnitude < cutForce || cutFoodPrefab == null || isCut)
            {
                return;
            }
            isCut = true;
            Cut();
        }
    }


}
