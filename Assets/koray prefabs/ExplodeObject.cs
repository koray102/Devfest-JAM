using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeObject : MonoBehaviour
{
    [SerializeField] private float collisionMultp;
    [SerializeField] private float collisionDistance;
    [SerializeField] private GameObject triggerObject;
    [SerializeField] internal bool canExplode;
    private GameObject childObj;
    private bool didExploded = false;

    private void Start()
    {
        
    }
    public void Explode(GameObject throwedObject)
    {
        if(!didExploded && canExplode)
        {
            //Debug.Log("Explode object");

            if(GetComponent<Rigidbody>() != null)
            {
                Destroy(GetComponent<Rigidbody>());
            }

            if(GetComponent<Collider>() != null)
            {
                Destroy(GetComponent<Collider>());
            }
            
            List<Transform> allChildren = GetAllChildren(transform);
            foreach (var childCollider in allChildren)
            {
                childObj = childCollider.gameObject;
                if(childObj != gameObject)
                {
                    childObj.AddComponent<Rigidbody>();
                    childObj.GetComponent<Rigidbody>().isKinematic = false;

                    childObj.GetComponent<Rigidbody>().AddExplosionForce(collisionMultp, throwedObject.transform.position, collisionDistance);
                }
            }
            didExploded = true;
        }
    }

    private List<Transform> GetAllChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent)
        {
            children.Add(child);
            children.AddRange(GetAllChildren(child)); // Rekürsif çağrı
        }

        return children;
    }
}
