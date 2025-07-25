using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    public float speed = 15f;
      
    public Slider slider;
    int hp = 100;
    int attackPower = 0;

    Animator animator;
    int attackHash;
    int runHash;
    int dieHash;

    bool isAttack = false;


    Transform attackTarget;
    public GameObject[] attackFx;
    public Transform charTransform;

    NavMeshAgent agent;

    public GameObject coinPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("isAttack");
        runHash = Animator.StringToHash("isRun");
        dieHash = Animator.StringToHash("isDie");
        slider.maxValue = hp;
        slider.value = hp;

        charTransform = GameObject.Find("Char").transform;
        attackTarget = charTransform.GetChild(3);

        agent = GetComponent<NavMeshAgent>();
    }

    

    private void Update()
    {
        Vector3 target = charTransform.position;


        if(!isAlive() || isAttack || Vector3.Distance(transform.position, target) < 3)
        {
            agent.isStopped = true;
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.velocity = Vector3.zero;
        } else
        {
            transform.LookAt(target);

            agent.ResetPath();
            agent.isStopped = false;
            agent.updatePosition = true;
            agent.updateRotation = true;
            
            agent.SetDestination(target);
            agent.acceleration = 8;
            agent.angularSpeed = 120;
        }


        if (agent.velocity.magnitude > 0.1f && !animator.GetBool(runHash) && !isAttack)
        {
            initAnimation();
            animator.SetBool(runHash, true);
        } else if(agent.velocity.magnitude <= 0.1f)
        {
            animator.SetBool(runHash, false);
        }
      
        
    }


    public bool isAlive()
    {
        return slider.value > 0;
    }


    public void hit(int attackPower)
    {
        int skill = 0;
        if(SkillController.Instance.getSkill(1))
        {
            skill = 1;
            attackPower *= 3;
        }

        if (slider.value - attackPower <= 0)
        {
            slider.value = 0;
            StartCoroutine(startAttackFx(skill));
            StartCoroutine(dieEnemy());
            return;
        } else
        {
            slider.value -= attackPower;
            StartCoroutine(startAttackFx(skill));
        }
    }

    IEnumerator startAttackFx(int skill)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(attackFx[skill], attackTarget.position, attackTarget.rotation);
        SkillController.Instance.clearSkill(skill);
        yield return new WaitForSeconds(1);
        Destroy(attack);
    }

    IEnumerator dieEnemy()
    {
        initAnimation();
        animator.SetBool(dieHash, true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        GameObject coin = Instantiate(coinPrefab, transform.position, transform.rotation);
        coin.layer = 6;
        coin.tag = "coin1";
        coin.transform.DOJump(transform.position, 5, 1, 0.2f);
        
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
        if (!animator.GetBool(attackHash) && isAlive())
        {
            initAnimation();
            animator.SetBool(attackHash, true);
            yield return new WaitForSeconds(1);
            updater();
            animator.SetBool(attackHash, false);
            isAttack = false;
        }
    }

    private void initAnimation()
    {
        animator.SetBool(runHash, false);
        animator.SetBool(attackHash, false);
    }



}
