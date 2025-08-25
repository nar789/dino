using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.AI;

public class CharController : MonoBehaviour
{

    //float speed = 7f;
    public FloatingJoystick joystick;

    public float horizonStep = 2f;
    public float verticalStep = 2.5f;

    int attackPower = 10;

    int level = 0;

    public Slider slider;


    Animator animator;
    int isRunHash;
    int isAttackHash;
    int isDieHash;
    int isHitHash;


    Vector3 direction = Vector3.zero;
    public float checkWallDist = 6f;

    NavMeshAgent agent;
    Rigidbody rb;

    public Transform coinTarget;

    public GameObject levelUpFx;
    public GameObject levelUpFx2;


    void Start()
    {
        
        animator = GetComponent<Animator>();

        isHitHash = Animator.StringToHash("isHit");
        isRunHash = Animator.StringToHash("isRun");
        isAttackHash = Animator.StringToHash("isAttack");
        isDieHash = Animator.StringToHash("isDie");

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void initHp(int capa, int cur)
    {
        slider.maxValue = capa;
        slider.value = cur;
    }



    private void FixedUpdate()
    {
        if(!isAlive())
        {
            return;
        }

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {

            Vector3 moveDirection = new Vector3(-joystick.Horizontal, 0, -joystick.Vertical).normalized;
            float speed = MyProfile.Instance.getStat(1) / 100;
            MoveAgent(moveDirection, speed);
            transform.LookAt(transform.position + moveDirection);
                        
        }

    }

    void Update()
    {
        
        if(!isAlive())
        {
            return;
        }
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {            
            initAnimation();
            animator.SetBool(isRunHash, true);
        }
        else
        {
            animator.SetBool(isRunHash, false);
        }

    }

    void MoveAgent(Vector3 moveDirection, float moveSpeed)
    {
        if (moveDirection.magnitude > 0.1f) // Add a small deadzone
        {
            //Option 1: Using Move (simple movement)
            agent.Move(moveDirection * moveSpeed * Time.deltaTime);

            //Option 2: Using SetDestination (pathfinding)
            //Vector3 targetPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
            //NavMeshHit hit;
            //if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            //{
            //    agent.SetDestination(hit.position);
            //}
        }
        else
        {
            // Optionally stop the agent if no input
            //agent.SetDestination(transform.position); // Or set velocity to zero.
            agent.velocity = Vector3.zero;
        }

    }

    public int getLevel()
    {
        return level;
    }

    public void addLevel()
    {
        level += 1;
    }


    public void attack(EnemyCatcher.StateUpdater updater)
    {
        StartCoroutine(startAttackAnimation(updater));
    }

    IEnumerator startAttackAnimation(EnemyCatcher.StateUpdater updater)
    {
        //Debug.Log("attack!");
        initAnimation();
        animator.SetBool(isAttackHash, true);
        yield return new WaitForSeconds(0.575f);
        animator.SetBool(isAttackHash, false);
        updater();
        
    }



    public int getAttackPower()
    {
        int str = MyProfile.Instance.getStat(0) / 100;
        return str;
    }

    public bool isAlive()
    {
        return slider.value > 0;
    }

    public void hit(int attackPower)
    {
        //Debug.Log("hit " + attackPower);
        MyProfile.Instance.updateHp(attackPower);
        if (slider.value - attackPower <= 0)
        {
            slider.value = 0;
            StartCoroutine(diePlayer());
            return;
        }
        else
        {
            slider.value -= attackPower;
        }
    }



    IEnumerator diePlayer()
    {
        Debug.Log("die!");
        initAnimation();
        animator.SetBool(isDieHash, true);
        yield return new WaitForSeconds(1);
        //Destroy(gameObject);

    }


    private void initAnimation()
    {
        animator.SetBool(isRunHash, false);
        animator.SetBool(isAttackHash, false);
        animator.SetBool(isHitHash, false);
        animator.SetBool(isDieHash, false);
    }

    public Transform getCoinTarget()
    {
        return coinTarget;
    }

    public void startLevelUpFx()
    {
        StartCoroutine(playLevelUpFx());
    }
    IEnumerator playLevelUpFx()
    {
        GameObject fx = Instantiate(levelUpFx, transform.position, transform.rotation);
        GameObject fx2 = Instantiate(levelUpFx2, transform.position, transform.rotation);
        yield return new WaitForSeconds(2);
        DestroyImmediate(fx);
        DestroyImmediate(fx2);
    }

}
