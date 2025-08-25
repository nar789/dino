using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;
using System.Collections;

public class GameController1 : MonoBehaviour
{

    CharController charController;
    public SkillController skillController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SkillIconController iconController;
    public BuffIconController buffIconController;
    Coroutine stopMainBuffCoroutine;


    public static GameController1 Instance;

    public GameObject[] panels;

    public VideoPlayer videoPlayer;
    public VideoClip[] clips;
    public GameObject godViewer;
    public GameObject mainGodViewer;


    bool isPlaying = false;

    //Stat UI Panel
    public TMPro.TextMeshProUGUI statPointText;
    public TMPro.TextMeshProUGUI diaText;
    public TMPro.TextMeshProUGUI[] statText;
    public TMPro.TextMeshProUGUI[] statLevelText;
    //

    //ToastUIPanel
    public CanvasGroup toastBack;
    public TMPro.TextMeshProUGUI[] toastText;
    Sequence toastSeq = null;
    //

    //Right Top UI
    public TMPro.TextMeshProUGUI rightTopDiaText;
    public TMPro.TextMeshProUGUI rightTopGoldText;
    //

    //left top UI
    public TMPro.TextMeshProUGUI missionText;
    //

    public MissionFloor[] missionFloors;

    public GameObject newMap;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        charController = GameObject.Find("Char").GetComponent<CharController>();
        //videoPlayer.Prepare();
        videoPlayer.prepareCompleted += vp =>
        {
            if(panels[1].activeSelf)
            {
                godViewer.SetActive(true);
            } else
            {
                mainGodViewer.SetActive(true);
            }
            
            vp.Play();
        };

        rightTopDiaText.text = $"{MyProfile.Instance.getDia():N0}";
        rightTopGoldText.text = $"{MyProfile.Instance.getGold():N0}";
        missionText.text = MyProfile.Instance.getMissionName() + " " +
            MyProfile.Instance.getBuildingCount() + " / " +
            MyProfile.Instance.getTotalBuildingCount();
    }
    public void onSkillClick(int id)
    {
       
        if (iconController.coolSkill(id - 1))
        {
            //Debug.Log("skill " + id);
            if (id == 3)
            {
                SkillController.Instance.generateExplosion(charController.transform.position, charController.transform.rotation);

            }
            else if (id == 2)
            {
                Vector3 dest = charController.transform.position + Vector3.zero;
                dest.y = 0.5f;
                SkillController.Instance.generateMagic(dest, charController.transform.rotation);
            }
            else
            {
                SkillController.Instance.setSkill(id);
            }
        }
    }

    public void onBuffClick(int id)
    {
        if(buffIconController.coolBuff(id))
        {
            //update my profile stat.
            MyProfile.Instance.setBuff(id, true);
            if(id == 2)
            {
                MyProfile.Instance.updateHpCapa();
            }
        }
        //play animation
        videoPlayer.Pause();
        videoPlayer.frame = 0;
        isPlaying = false;
        videoPlayer.clip = null;
        buff(id);
        if(stopMainBuffCoroutine != null)
        {
            StopCoroutine(stopMainBuffCoroutine);
        }
        stopMainBuffCoroutine = StartCoroutine(stopMainBuff());
    }

    IEnumerator stopMainBuff()
    {
        yield return new WaitForSeconds(5);
        videoPlayer.Pause();
        videoPlayer.frame = 0;
        isPlaying = false;
        videoPlayer.clip = null;
        mainGodViewer.SetActive(false);
    }

    public void onAutoButtonClick()
    {
        SkillController.Instance.setAuto(!SkillController.Instance.getIsAuto());
    }

    public void openStat(int idx)
    {
        Time.timeScale = 0f;
        for(int i=0;i<panels.Length;i++)
        {
            panels[i].SetActive(false);
        }
        panels[idx].SetActive(true);
        godViewer.SetActive(false);
        videoPlayer.Pause();
        videoPlayer.frame = 0;
        isPlaying = false;
        videoPlayer.clip = null;

        if(idx == 0)
        {
            initStatUIPanel();
        }
    }
    public void closeStat()
    {
        Time.timeScale = 1f;
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        godViewer.SetActive(false);
        videoPlayer.Pause();
        videoPlayer.frame = 0;
        isPlaying = false;
        videoPlayer.clip = null;
    }


    public void buff(int idx)
    {
        string[] typeStr = {"근력", "민첩", "건강", "지능" };
        if (!isPlaying && videoPlayer.clip != clips[idx])
        {
            isPlaying = true;
            videoPlayer.clip = clips[idx];
            videoPlayer.Prepare();
            toast(typeStr[idx] + "이 10% 상승했습니다.", 1);
        }
        
    }


    private void initStatUIPanel()
    {
        statPointText.text = $"{MyProfile.Instance.getSp():N0}";
        diaText.text = $"{MyProfile.Instance.getDia():N0}";
        for (int i = 0; i < statText.Length; i++)
        {
            statText[i].text = $"{MyProfile.Instance.getStat(i):N0}";
            statLevelText[i].text = $"Lv.{MyProfile.Instance.getStatLevel(i):N0}";
        }


        
    }

    public void onLevelUpStat(int idx)
    {
        if (MyProfile.Instance.getStatLevel(idx) + 1 > 99)
        {
            toast("최대 능력치에 도달했습니다!", 2);
            return;
        }


        //if (!MyProfile.Instance.useStatPoint())
        {
            if (!MyProfile.Instance.useDia(5))
            {
                toast("다이아가 모자랍니다. 상점에서 구매하세요.", 2);
                return;
            }
        }
        statPointText.text = $"{MyProfile.Instance.getSp():N0}";
        diaText.text = $"{MyProfile.Instance.getDia():N0}";
        rightTopDiaText.text = $"{MyProfile.Instance.getDia():N0}";

        MyProfile.Instance.levelUpStat(idx);

        statText[idx].text = $"{MyProfile.Instance.getStat(idx):N0}";
        statLevelText[idx].text = $"Lv.{MyProfile.Instance.getStatLevel(idx):N0}";

        toast("능력치 레벨업!", 0);



        int stat = MyProfile.Instance.getStat(idx);
        int level = MyProfile.Instance.getStatLevel(idx);
        

        
    }

    //idx : yello, blue, red
    public void toast(string text, int idx)
    {
        for(int i=0;i<toastText.Length;i++)
        {
            toastText[i].gameObject.SetActive(false);
        }
        toastText[idx].gameObject.SetActive(true);
        toastText[idx].text = text;
        toastBack.gameObject.SetActive(true);
        toastBack.alpha = 0;
        if(toastSeq != null)
        {
            toastSeq.Kill();
        }
        toastSeq = DOTween.Sequence();
        toastSeq.Append(toastBack.DOFade(1, 1f).SetUpdate(true))
            .AppendInterval(2f)
            .Append(toastBack.DOFade(0, 1f).SetUpdate(true))
            .OnComplete(() =>
            {
                toastBack.gameObject.SetActive(false);
            }).SetUpdate(true);


    }

    public void addGold()
    {
        MyProfile.Instance.addGold();
        rightTopGoldText.text = $"{MyProfile.Instance.getGold():N0}";
    }

    public void useGold()
    {
        MyProfile.Instance.useGold();
        rightTopGoldText.text = $"{MyProfile.Instance.getGold():N0}";
    }

    public void addBuildingCount()
    {
        MyProfile.Instance.addBuildingCount();
        missionText.text = MyProfile.Instance.getMissionName() + " " +
            MyProfile.Instance.getBuildingCount() + " / " +
            MyProfile.Instance.getTotalBuildingCount();


        if(MyProfile.Instance.getBuildingCount() <= MyProfile.Instance.getTotalBuildingCount())
        {
            if(MyProfile.Instance.addMissionLevelAndClearBuildingCount())
            {
                toast("미션 클리어! 다이아 10개 지급!!", 1);
                MyProfile.Instance.addDia(10);
                rightTopDiaText.text = $"{MyProfile.Instance.getDia():N0}";

                missionText.text = MyProfile.Instance.getMissionName() + " " +
                MyProfile.Instance.getBuildingCount() + " / " +
                MyProfile.Instance.getTotalBuildingCount();

                allClearMissionFloor();
                if(MyProfile.Instance.getMissionLevel() == 1)
                {
                    Debug.Log("mission level 2");
                    DinoFactory.Instance.startBorn2();
                }
            } else
            {
                toast("미션 모두 클리어! 승리! 다이아 100개 지급", 1);
                MyProfile.Instance.addDia(100);

                rightTopDiaText.text = $"{MyProfile.Instance.getDia():N0}";

                missionText.text = MyProfile.Instance.getMissionName() + " " +
                MyProfile.Instance.getBuildingCount() + " / " +
                MyProfile.Instance.getTotalBuildingCount();

                allClearMissionFloor();
            }
        }
    }

    void allClearMissionFloor()
    {
        for(int i=0;i<missionFloors.Length;i++)
        {
            missionFloors[i].clear();
        }
    }
}
