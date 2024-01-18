using System.Collections.Generic;
using UnityEngine;

public class DropOff : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Plate"))
        {
            Plate p = other.gameObject.GetComponent<Plate>();
            if (p == null)
            {
                Debug.LogError("Plate has no Plate component");
                return;
            }

            List<GameObject> recipe = p.getStack();
            GameManager.Instance.SendBurger(recipe);

            //Delete plate and burger
            
            Destroy(other.gameObject);
            foreach (GameObject g in recipe)
            {
                Destroy(g);
            }
        }
    }
}
