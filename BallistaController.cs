using UnityEngine;
using System.Collections;
public class BallistaController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    bool isDie = false;

    public GameObject bolt;

    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;

    bool isAttack = false;

    int isAttackHash;
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        isAttackHash = Animator.StringToHash("isAttack");
    }

    // Update is called once per frame
    void Update()
    {


        if(isAttack)
        {
            return; 
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

            isAttack = true;
            gameObject.transform.LookAt(short_enemy.transform.position);
            StartCoroutine(createBolt());

        }
    }

    IEnumerator createBolt()
    {
        animator.SetBool(isAttackHash, true);
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = -90f;
        Instantiate(bolt, pos, Quaternion.Euler(rot));
        
        yield return new WaitForSeconds(1);
        animator.SetBool(isAttackHash, false);
        isAttack = false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.pink;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
