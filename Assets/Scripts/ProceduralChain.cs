using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralChain : MonoBehaviour
{
    [SerializeField] private float shootDuration;
    //[SerializeField] private float chainOffsetFromPlayer = 1f;
    [SerializeField] private Vector3 chainOffsetFromPlayer;
    [SerializeField] private GameObject princessPrefab;
    internal bool didThrowed = false;
    public Transform player; // Player's transform
    public GameObject chainLinkPrefab; // Prefab for a single chain link (RigidBody with collider)
    public float chainLinkLength = 1f; // Length of each chain link
    private GameObject[] chainLinks; // Array to hold the chain links
    private Vector3 surfaceNormalThis;


    public void DetectHookshotCollision(Vector3 grabPoint, GameObject grabbedObject, Vector3 surfaceNormal)
    {
        // Create chain links between the player and the hit point
        surfaceNormalThis = surfaceNormal;
        CreateChain(grabPoint, surfaceNormal);

        // Attach the first chain link to the player
        SpringJoint firstLinkJoint = chainLinks[0].GetComponent<SpringJoint>();
        firstLinkJoint.connectedBody = player.GetComponent<Rigidbody>(); // Attach to player

        // Attach the last chain link to the wall
        //SpringJoint lastLinkJoint = chainLinks[chainLinks.Length - 1].GetComponent<SpringJoint>();
        //lastLinkJoint.connectedAnchor = grabPoint; // Attach the last link to the wall
        Destroy(chainLinks[chainLinks.Length - 1].GetComponent<Collider>());

        //chainLinks[chainLinks.Length - 1].GetComponent<Rigidbody>().AddForce(transform.forward * 100f , ForceMode.Impulse);
        StartCoroutine(SimulateThrow(grabPoint));
    }


    void CreateChain(Vector3 targetPoint, Vector3 surfaceNormal)
    {
        // Calculate the number of links required based on the distance between the player and the hit point
        Vector3 chainRotation = Vector3.zero;
        float distance = Vector3.Distance(player.position, targetPoint);
        int numberOfLinks = Mathf.CeilToInt(distance / chainLinkLength);

        chainLinks = new GameObject[numberOfLinks];

        // Create chain links and position them
        for (int i = 0; i < numberOfLinks; i++)
        {
            GameObject prefabToInstantiate;
            quaternion prefabRotation;

            chainRotation += new Vector3(0, 90, 0);

            Vector3 offset = player.transform.InverseTransformDirection(chainOffsetFromPlayer);
            Vector3 bottomTarget = player.position + (Vector3.down * distance);

            if(i == chainLinks.Length - 1)
            {
                prefabToInstantiate = princessPrefab;
                prefabRotation = Quaternion.LookRotation(surfaceNormal, Vector3.up);
            }else
            {
                prefabToInstantiate = chainLinkPrefab;
                prefabRotation = Quaternion.Euler(chainRotation);
            }

            Vector3 linkPosition = Vector3.Lerp(player.position + offset, bottomTarget, (float)i / (numberOfLinks - 1));
            GameObject chainLink = Instantiate(prefabToInstantiate, linkPosition, prefabRotation);

            //chainLink.transform.LookAt(targetPoint);

            // Add a hinge joint to allow bending
            SpringJoint joint = chainLink.GetComponent<SpringJoint>();   

            if (i > 0)
            {
                // Connect this link to the previous one
                joint.connectedBody = chainLinks[i - 1].GetComponent<Rigidbody>();
            }
            
            //joint.enablePreprocessing = false;

            // Store the chain link
            chainLinks[i] = chainLink;
        }

        chainLinks[chainLinks.Length - 1].GetComponent<Rigidbody>().isKinematic = true;
    }


    private IEnumerator SimulateThrow(Vector3 targetPos)
    {
        float elapsedTime = 0f;
        GameObject lastLink = chainLinks[chainLinks.Length - 1];

        while (elapsedTime < shootDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / shootDuration; // Zamana bağlı bir interpolasyon faktörü
            Vector3 newPosition;
            newPosition = Vector3.Lerp(lastLink.transform.position, targetPos, t);
            SetLastChainPosition(newPosition);

            yield return null; // Bir sonraki kareyi bekle
        }

        SetLastChainPosition(targetPos);
        didThrowed = true;
    }


    public void SetLastChainPosition(Vector3 grabPos)
    {
        GameObject lastLink = chainLinks[chainLinks.Length - 1];
        Vector3 offset = surfaceNormalThis.normalized * 2.7f;
        //offset = lastLink.transform.InverseTransformDirection(offset);
        lastLink.transform.position = grabPos + offset;
    }


    public void DestroyChain()
    {
        if(chainLinks == null) return;

        foreach (GameObject chain in chainLinks)
        {
            Destroy(chain);
        }

        StopAllCoroutines();
        didThrowed = false;
    }
}
