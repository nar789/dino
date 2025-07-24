using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.AI;

public class CharController : MonoBehaviour
{

    public float speed = 15f;
    public FloatingJoystick joystick;

    public float horizonStep = 2f;
    public float verticalStep = 2.5f;

    int attackPower = 10;

    int level = 0;

    public Slider slider;
    int hp = 100;


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
    

    void Start()
    {
        slider.maxValue = hp;
        slider.value = hp;
        animator = GetComponent<Animator>();

        isHitHash = Animator.StringToHash("isHit");
        isRunHash = Animator.StringToHash("isRun");
        isAttackHash = Animator.StringToHash("isAttack");
        isDieHash = Animator.StringToHash("isDie");

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

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
        Debug.Log("attack!");
        initAnimation();
        animator.SetBool(isAttackHash, true);
        yield return new WaitForSeconds(0.575f);
        animator.SetBool(isAttackHash, false);
        updater();
        
    }



    public int getAttackPower()
    {
        return attackPower;
    }

    public bool isAlive()
    {
        return slider.value > 0;
    }

    public void hit(int attackPower)
    {
        Debug.Log("hit " + attackPower);
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

}
