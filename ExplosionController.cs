using UnityEngine;
using System.Collections;
public class ExplosionController : MonoBehaviour
{



    int attackPower = 0;

    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;


    bool once = false;




    void Start()
    {
        attackPower = GameObject.Find("Char").GetComponent<CharController>().getAttackPower();
    }

    // Update is called once per frame
    void Update()
    {
        if(!once)
        {
            once = true;
            colliders = Physics.OverlapSphere(transform.position, radius, layer);

            if (colliders.Length > 0)
            {

                foreach (Collider col in colliders)
                {
                    if (!col.gameObject.GetComponent<EnemyController>().isAlive())
                    {
                        continue;
                    }

                    EnemyController controller = col.GetComponent<EnemyController>();
                    float S = MyProfile.Instance.getStat(3) / 1000;
                    controller.hitRemote(attackPower * 4 * S);
                }

            }

            StartCoroutine(gone());
        }
    }

    IEnumerator gone()
    {
        yield return new WaitForSeconds(1);
        DestroyImmediate(gameObject);
    }
}
