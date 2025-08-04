using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicController : MonoBehaviour
{

    public float speed = 2f;
    private Rigidbody rigidbody;

    public GameObject startExp;
    public GameObject hitExp;

    bool isHit = false;
    int attackPower = 0;

    public List<GameObject> fxObj = new List<GameObject>();

    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(BirthStartExp());
        attackPower = GameObject.Find("Char").GetComponent<CharController>().getAttackPower();
    }


    IEnumerator BirthStartExp()
    {
        GameObject start = Instantiate(startExp, transform.position, transform.rotation);
        yield return new WaitForSeconds(1);
        DestroyImmediate(start);
    }


    IEnumerator BirthHitExp(Vector3 pos)
    {
        GameObject hitObj = Instantiate(hitExp, pos, transform.rotation);
        fxObj.Add(hitObj);
        yield return new WaitForSeconds(1);
        DestroyImmediate(hitObj);
        StopAllCoroutines();
        DestroyImmediate(gameObject);

        foreach(GameObject obj in fxObj)
        {
            if(obj!= null)
            {
                DestroyImmediate(obj);
            }
        }
        
    }


    private void FixedUpdate()
    {
        if(isHit)
        {
            rigidbody.linearVelocity = Vector3.zero;
        } else
        {
            rigidbody.AddForce(transform.forward * speed);
        }
        
    }

    void Update()
    {
        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.z < -100 || transform.position.z > 100)
        {
            Destroy(gameObject);
        }

        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            Collider short_enemy = null;

            foreach (Collider col in colliders)
            {
                if (!col.gameObject.GetComponent<EnemyController>().isAlive())
                {
                    continue;
                }

                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);
                if (short_enemy == null || short_distance > short_distance2)
                {
                    short_distance = short_distance2;
                    short_enemy = col;
                }
            }

            if (short_enemy == null)
            {
                return;
            }

            gameObject.transform.LookAt(short_enemy.transform.position);


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("enemy"))
        {
            isHit = true;
            Vector3 target = other.transform.position + Vector3.zero;
            target.y = 0.5f;
            StartCoroutine(BirthHitExp(target));
            other.GetComponent<EnemyController>().hitRemote(attackPower * 5);
        }
    }
}