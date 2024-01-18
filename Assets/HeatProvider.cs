using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HeatProvider : MonoBehaviour
{
    //Enum type for heat type, cube, cylinder, sphere
    public enum HeatType
    {
        Rectangle,
        Cylinder,
        Sphere
    }
    [SerializeField] private float heat = 1f;
    [SerializeField] private HeatType heatType = HeatType.Cylinder;
    private bool m_Active;
    [SerializeField] private float m_HeatRadius = 0.5f;
    private Collider m_HeatCollider;
    private List<GameObject> foodIn = new List<GameObject>();

    public bool IsActive()
    {
        return m_Active;
    }
    private float heatRadius
    {
        get => m_HeatRadius;
        set
        {
            m_HeatRadius = value;
            if (heatRadius < 0)
            {
                heatRadius = 0;
            }
        }
    }
    [SerializeField] private float m_HeatHeight = 1f;
    private float heatHeight
    {
        get => m_HeatHeight;
        set
        {
            m_HeatHeight = value;
            if (heatHeight < 0)
            {
                heatHeight = 0;
            }
        }
    }
    [SerializeField] private Vector3 m_HeatCenter;
    private Vector3 heatCenter
    {
        get => m_HeatCenter;
        set
        {
            m_HeatCenter = value;
            if (heatCenter == null)
            {
                heatCenter = Vector3.zero;
            }
        }
    }
    public void Activate()
    {
        Debug.Log("Activate");
        foodIn.Clear();
        
        
        m_Active = !m_Active;
        m_HeatCollider.enabled = m_Active;
        
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (heatType == HeatType.Rectangle)
        {
            BoxCollider col = gameObject.AddComponent<BoxCollider>();
            col.center = heatCenter + new Vector3(0, heatHeight / 2, 0);
            col.size = new Vector3(heatRadius, heatHeight, heatRadius);
            m_HeatCollider = col;
            
        }
        else if (heatType == HeatType.Cylinder)
        {
            CapsuleCollider col = gameObject.AddComponent<CapsuleCollider>();
            col.center = heatCenter + new Vector3(0, 0, heatHeight / 2);
            col.radius = heatRadius;
            col.height = heatHeight;
            // z axis
            col.direction = 2;
            m_HeatCollider = col;
        }
        else if (heatType == HeatType.Sphere)
        {
            SphereCollider col = gameObject.AddComponent<SphereCollider>();
            col.center = heatCenter;
            col.radius = heatRadius;
            m_HeatCollider = col;
        }
        m_HeatCollider.isTrigger = true;
        m_HeatCollider.enabled = m_Active;
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            foodIn.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            foodIn.Remove(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Active)
        {
            foreach (GameObject food in foodIn)
            {
                food.GetComponent<Food>().Heat(heat*Time.deltaTime);
            }
        }
        
    }
}
