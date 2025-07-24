using UnityEngine;
using System.Collections;


public class DinoFactory : MonoBehaviour
{


    public GameObject dinoPrefab;

    public GameObject origin;

    int maxCount = 10;
    int currentCount = 0;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(birth());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator birth()
    {
        yield return new WaitForSeconds(1);
        Instantiate(dinoPrefab, origin.transform.position, dinoPrefab.transform.rotation);
        currentCount += 1; 
        if(currentCount + 1 <= maxCount)
        {
            StartCoroutine(birth());
        }
    }


    
}
