using UnityEngine;

public class EnemyCatcher : MonoBehaviour
{

    CharController charController;

    public float radius = 0f;
    public LayerMask layer;

    public Collider[] colliders;

    bool isPlaying = false;
    public delegate void StateUpdater();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charController = gameObject.GetComponent<CharController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!charController.isAlive())
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
                if(!col.gameObject.GetComponent<EnemyController>().isAlive())
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

            if(short_enemy == null)
            {
                return;
            }

            gameObject.transform.LookAt(short_enemy.transform.position);
            EnemyController enemyController = short_enemy.gameObject.GetComponent<EnemyController>();
            if(enemyController.isAlive() && !isPlaying)
            {
                isPlaying = true;
                attackAndHit(enemyController, charController.getAttackPower());
            } else if(!enemyController.isAlive())
            {
                stopPlaying();
            }
        }
    }


    public void attackAndHit(EnemyController enemy, int attackPower)
    {
        charController.attack(stopPlaying);
        enemy.hit(attackPower);
    }

    public void stopPlaying()
    {
        isPlaying = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
