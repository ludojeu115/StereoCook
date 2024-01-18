using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Plate : MonoBehaviour
{
    public Food food;
    private SocketWithTag socketInteractor;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private void DisableRigidbody()
    {
        if(food != null)
            food.DisableRigidbody(true);
    }
    private void EnableRigidbody()
    {
        if(food != null)
            food.EnableRigidbody(true);
    }

    public List<GameObject> getStack()
    {
        if (food == null)
            return new List<GameObject>();
        return food.getStack();
    }
    
    
    void Start()
    {
        gameObject.tag = "Plate";
        gameObject.layer = LayerMask.NameToLayer("Plate");
        Transform top = transform.Find("Top");
        socketInteractor = top.GetComponent<SocketWithTag>();
        if (socketInteractor == null)
            socketInteractor = top.gameObject.AddComponent<SocketWithTag>();
        socketInteractor.socketTag = "Food";
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }
        socketInteractor.interactionLayers = InteractionLayerMask.GetMask("Burger");
        socketInteractor.interactableCantHoverMeshMaterial = null;
        socketInteractor.plate = this;
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
            if (rb == null) return;
            rb.excludeLayers = ~0 - LayerMask.GetMask("Default");
            if(food != null)
                food.DisableRigidbody(true);
        });
        grabInteractable.selectExited.AddListener((args) =>
        {
            if (rb == null) return;
            rb.excludeLayers = 0;
            if(food != null)
                food.EnableRigidbody(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
