using UnityEngine;
using System.Collections;
public class MissionFloor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public GameObject building;
    public GameObject startFx;
    public TMPro.TextMeshProUGUI goldInfo;
    GameObject buildingInstance;



    int leftCoin = 10;
    int capa;
    int[] capaList = {10, 10, 10, 500, 1000, 2000, 3000, 5000, 7000, 10000};
    void Start()
    {
        capa = leftCoin = capaList[MyProfile.Instance.getMissionLevel()];
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
        buildingInstance = Instantiate(building, transform.position, transform.rotation);
        yield return new WaitForSeconds(2);        
        DestroyImmediate(fx);
    }

    public bool pay()
    {
        if(leftCoin > 0)
        {
            leftCoin -= 1;
            goldInfo.text = capa - leftCoin + "/" + capa;
            if(startFx != null && building != null && leftCoin == 0)
            {
                StartCoroutine(createBuilding());
                GameController1.Instance.addBuildingCount();
            }
            return true;
        } else
        {
            return false;
        }
        
    }

    IEnumerator clearBuilding()
    {
        GameObject fx = Instantiate(startFx, transform.position, transform.rotation);
        yield return new WaitForSeconds(1);
        DestroyImmediate(buildingInstance);
        yield return new WaitForSeconds(2);
        DestroyImmediate(fx);
    }


    public void clear()
    {
        StopAllCoroutines();
        StartCoroutine(clearBuilding());
        capa = leftCoin = capaList[MyProfile.Instance.getMissionLevel()];
        goldInfo.text = capa - leftCoin + "/" + capa;
    }
}
