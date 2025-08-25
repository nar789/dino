using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconController : MonoBehaviour
{
    public GameObject[] hide;
    public TMPro.TextMeshProUGUI[] timeText;
    public Image[] hideImg;
    private bool[] isHide = { false, false, false, false };
    private float[] skillTimes = { 600, 600, 600, 600 };
    private float[] getSkillTimes = { 0, 0, 0, 0 };
    void Start()
    {
        for (int i = 0; i < hide.Length; i++)
        {
            hide[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hideSkillChk();
    }

    public bool coolBuff(int idx)
    {
        if (!hide[idx].activeSelf)
        {
            hide[idx].SetActive(true);
            getSkillTimes[idx] = skillTimes[idx];
            isHide[idx] = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void hideSkillChk()
    {
        if (isHide[0])
        {
            StartCoroutine(skillTimeChk(0));
        }

        if (isHide[1])
        {
            StartCoroutine(skillTimeChk(1));
        }

        if (isHide[2])
        {
            StartCoroutine(skillTimeChk(2));
        }

        if (isHide[3])
        {
            StartCoroutine(skillTimeChk(3));
        }
    }

    IEnumerator skillTimeChk(int idx)
    {
        yield return null;

        if (getSkillTimes[idx] > 0)
        {
            getSkillTimes[idx] -= Time.deltaTime;

            if (getSkillTimes[idx] < 0)
            {
                getSkillTimes[idx] = 0;
                isHide[idx] = false;
                hide[idx].SetActive(false);
                MyProfile.Instance.setBuff(idx, false);
                if(idx == 2)
                {
                    MyProfile.Instance.updateHpCapa();
                }
            }

            timeText[idx].text = $"{getSkillTimes[idx]:N0}";

            float time = getSkillTimes[idx] / skillTimes[idx];
            hideImg[idx].fillAmount = time;
        }
    }


}
