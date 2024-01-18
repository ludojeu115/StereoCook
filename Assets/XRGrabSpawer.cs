using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabSpawer : XRBaseInteractable
{
    [SerializeField] private GameObject prefab;
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (prefab!= null)
        {
            // Instantiate object
            GameObject newObject = Instantiate(prefab, args.interactorObject.transform.position, Quaternion.identity);

            // Get grab interactable from prefab
            XRGrabInteractable objectInteractable = newObject.GetComponent<XRGrabInteractable>();

            // Select object into same interactor
            interactionManager.SelectEnter(args.interactorObject, objectInteractable);
        }
        
        base.OnSelectEntered(args);
    }
}
