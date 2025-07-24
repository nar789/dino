using UnityEngine;
using System.Collections;



public class SoldierController : MonoBehaviour
{

    GameObject dinoPaths;
    int pathIdx = 0;
    public float speed = 15f;

    //public Slider slider;
    int hp = 100;
    int attackPower = 10;

    bool isAttack = false;

    public float checkWallDist = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dinoPaths = GameObject.Find("DinoPath");
        pathIdx = dinoPaths.transform.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive() || isAttack)
        {
            return;
        }



        Vector3 target = Vector3.zero;
        
        Vector3 dinoPosition = dinoPaths.transform.GetChild(pathIdx).position + Vector3.zero;
        if (Vector3.Distance(dinoPosition, transform.position) > 0.1f)
        {
            target = dinoPosition + Vector3.zero;
        }
        else if (pathIdx - 1 >= 0)
        {
            pathIdx -= 1;
            target = dinoPaths.transform.GetChild(pathIdx).position + Vector3.zero;
        }
        else
        {
            Debug.Log("goal!");
            Destroy(gameObject);
        }

        transform.LookAt(target);
        
        if (checkWall(target))
        {
            /*
            if (animator.GetBool(runHash))
            {
                animator.SetBool(runHash, false);
            }*/
        }
        else
        {
            //initAnimation();
            //animator.SetBool(runHash, true);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }


    }


    private bool checkWall(Vector3 dest)
    {
        Vector3 direction;
        direction = transform.forward;
        Debug.DrawRay(transform.position, direction * checkWallDist, Color.cyan);
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, checkWallDist))
        {
            if (hit.collider.tag == "enemy")
            {
                if (hit.collider.gameObject.GetComponent<EnemyController>().isAlive())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }



    public bool isAlive()
    {
        //return slider.value > 0;
        return true;
    }



    public int getAttackPower()
    {
        return attackPower;
    }


    public void attack(PlayerCatcher.StateUpdater updater)
    {
        isAttack = true;
        StartCoroutine(startAttackAnimation(updater));
    }

    IEnumerator startAttackAnimation(PlayerCatcher.StateUpdater updater)
    {
        //if (!animator.GetBool(attackHash) && isAlive())
        {
            initAnimation();
            //animator.SetBool(attackHash, true);
            yield return new WaitForSeconds(1);
            updater();
            //animator.SetBool(attackHash, false);
            isAttack = false;
        }
    }

    private void initAnimation()
    {
        //animator.SetBool(runHash, false);
        //animator.SetBool(attackHash, false);
    }

}
