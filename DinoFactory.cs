using UnityEngine;
using System.Collections;


public class DinoFactory : MonoBehaviour
{
    public static DinoFactory Instance;

    public GameObject dinoPrefab;

    public GameObject origin;
    public GameObject origin2;

    int maxCount = 10;
    int currentCount = 0;
    int currentCount2 = 0;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(birth());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startBorn2()
    {
        StartCoroutine(birth2());
    }

    IEnumerator birth2()
    {
        yield return new WaitForSeconds(1);
        EnemyController dino = Instantiate(dinoPrefab, origin2.transform.position, dinoPrefab.transform.rotation).GetComponent<EnemyController>();
        currentCount2 += 1;
        if (currentCount2 + 1 <= maxCount)
        {
            StartCoroutine(birth2());
        }
        else
        {
            dino.setOnDieEnemy(() => StartCoroutine(reborn2()));
        }
    }

    IEnumerator birth()
    {
        yield return new WaitForSeconds(1);
        EnemyController dino = Instantiate(dinoPrefab, origin.transform.position, dinoPrefab.transform.rotation).GetComponent<EnemyController>();
        currentCount += 1; 
        if(currentCount + 1 <= maxCount)
        {
            StartCoroutine(birth());
        } else
        {
            dino.setOnDieEnemy(() => StartCoroutine(reborn()));
        }
    }

    IEnumerator reborn()
    {
        currentCount = 0;
        //Debug.Log("reborn() after 10 sec.");
        yield return new WaitForSeconds(9);
        StartCoroutine(birth());
    }

    IEnumerator reborn2()
    {
        currentCount2 = 0;
        //Debug.Log("reborn() after 10 sec.");
        yield return new WaitForSeconds(9);
        StartCoroutine(birth2());
    }



}
