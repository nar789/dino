using UnityEngine;
using System.Collections;
public class MissionFloor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public GameObject building;
    public GameObject startFx;
    public TMPro.TextMeshProUGUI goldInfo;



    int leftCoin = 10;
    int capa;
    void Start()
    {
        capa = leftCoin = 10;
        goldInfo.text = "0/" + capa;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator createBuilding()
    {
        GameObject fx = Instantiate(startFx, transform.position, transform.rotation);
        yield return new WaitForSeconds(1);
        Instantiate(building, transform.position, transform.rotation);
        yield return new WaitForSeconds(2);        
        DestroyImmediate(fx);
    }

    public bool pay()
    {
        if(leftCoin > 0)
        {
            leftCoin -= 1;
            Debug.Log("left coin " + leftCoin);
            goldInfo.text = capa - leftCoin + "/" + capa;
            if(startFx != null && building != null && leftCoin == 0)
            {
                StartCoroutine(createBuilding());
            }
            return true;
        } else
        {
            return false;
        }
        
    }
}
