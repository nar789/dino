using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCatcher : MonoBehaviour
{
    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;
    public PriorityQueue<GameObject> storage = new PriorityQueue<GameObject>();
    

    CharController charController;


    void Start()
    {
        charController = gameObject.GetComponent<CharController>();        
    }

    void Update()
    {

        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            foreach (Collider col in colliders)
            {
                if(col.tag == "coin1")
                {
                    col.tag = "coin2";
                    //charController.addLevel();
                    int level = storage.Count + 1;
                    col.gameObject.GetComponent<CoinController>().jump(level);
                    storage.Enqueue(col.gameObject, -level);
                    break;
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

 

    private void OnTriggerStay(Collider other)
    {
        if (storage.Count > 0)
        {
            if (other.tag == "area")
            {
                GameObject obj = storage.Dequeue();
                obj.GetComponent<CoinController>().pay(other.gameObject.transform.position);
            }
        }
    }


}
