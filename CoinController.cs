using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CoinController : MonoBehaviour
{

    Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("Char").GetComponent<CharController>().getCoinTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void jump(int level)
    {
        //StartCoroutine(startJump(level));
        Vector3 dest = target.position + Vector3.zero;
        dest.y = level * 0.2f;
        transform.DOJump(dest, 5, 1, 0.2f).OnComplete(() =>
        {
            transform.parent = target.parent;
            transform.localPosition = new Vector3(target.localPosition.x, level * 0.2f, target.localPosition.z);
        });
    }

    public void pay(Vector3 dest)
    {
        transform.DOJump(dest, 5, 1, 0.2f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }




}
