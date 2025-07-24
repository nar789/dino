using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;
    
    EnemyController enemyController;
    public Collider short_player;

    public delegate void StateUpdater();
    bool isAttack = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyController = gameObject.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyController.isAlive())
        {
            return;
        }


        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            short_player = colliders[0];
            foreach (Collider col in colliders)
            {
                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);
                if (short_distance > short_distance2)
                {
                    short_distance = short_distance2;
                    short_player = col;
                }
            }
            
            gameObject.transform.LookAt(short_player.transform.position);
            CharController charController = short_player.gameObject.GetComponent<CharController>();
           
            if (charController.isAlive() && !isAttack)
            {
                isAttack = true;
                attackAndHit(charController, enemyController.getAttackPower());
            }
            else if (!charController.isAlive())
            {
                stopPlaying();
            }
            
        }
    }

    public void attackAndHit(CharController player, int attackPower)
    {
        enemyController.attack(stopPlaying);
        player.hit(attackPower);
    }

    public void stopPlaying()
    {
        isAttack = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
