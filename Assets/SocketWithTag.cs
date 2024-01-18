using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithTag : XRSocketInteractor
{
    public string socketTag;
 
    public Food food;//self, parent in hierarchy
    public Plate plate;//self, parent in hierarchy
    
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        if (food != null && food.justSpawned) return false;
        Food f = interactable.transform.GetComponent<Food>();
        if(food != null && f !=null && f.getStack().Contains(food.gameObject)) // If the food is already in the stack, it can't be stacked (avoid infinite loop)
        {
            return false;
        }
        
        return interactable.transform.CompareTag(socketTag) && base.CanHover(interactable);
    }
    
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (food != null && food.justSpawned) return false; // If the food has just spawned, it can't be stacked
        Food f = interactable.transform.GetComponent<Food>();
        if(food != null && f !=null && f.getStack().Contains(food.gameObject)) // If the food is already in the stack, it can't be stacked (avoid infinite loop)
        {
            return false;
        }


        return interactable.transform.CompareTag(socketTag) && base.CanSelect(interactable);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        
        Food f = args.interactableObject.transform.GetComponent<Food>();
        
        if (food != null)
        {
            if (food.getStack().Contains(args.interactableObject.transform.gameObject))
            {
                return;
            }
            food.topFood = f;
            food.topFood.bottomFood = food;
        }
        if (plate != null)
        {
            plate.food = f;
        }
        base.OnSelectEntered(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (food != null && food.topFood != null)
        {
            food.topFood.bottomFood = null;
            food.topFood = null;
        }
        if (plate != null)
        {
            plate.food = null;
        }
        base.OnSelectExited(args);
        
    }
    
}
