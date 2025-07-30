using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SkillIconController : MonoBehaviour
{
    public GameObject[] hide;
    public GameObject[] text;
    public TMPro.TextMeshProUGUI[] timeText;
    public Image[] hideImg;
    private bool[] isHide = { false, false, false };
    private float[] skillTimes = { 3, 6, 9 };
    private float[] getSkillTimes = { 0, 0, 0 };
    void Start()
    {
        for(int i=0;i<text.Length;i++)
        {
            timeText[i] = text[i].GetComponent<TMPro.TextMeshProUGUI>();
            hide[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hideSkillChk();   
    }

    public bool coolSkill(int idx)
    {
        if (!hide[idx].activeSelf)
        {
            hide[idx].SetActive(true);
            getSkillTimes[idx] = skillTimes[idx];
            isHide[idx] = true;
            return true;
        } else
        {
            return false;
        }
    }

    private void hideSkillChk()
    {
        if(isHide[0])
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
    }

    IEnumerator skillTimeChk(int idx)
    {
        yield return null;

        if(getSkillTimes[idx] > 0)
        {
            getSkillTimes[idx] -= Time.deltaTime;

            if(getSkillTimes[idx] < 0)
            {
                getSkillTimes[idx] = 0;
                isHide[idx] = false;
                hide[idx].SetActive(false);
            }

            timeText[idx].text = getSkillTimes[idx].ToString("00");

            float time = getSkillTimes[idx] / skillTimes[idx];
            hideImg[idx].fillAmount = time;
        }
    }
}
