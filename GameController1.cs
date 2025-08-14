using UnityEngine;
using UnityEngine.Video;

public class GameController1 : MonoBehaviour
{

    CharController charController;
    public SkillController skillController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SkillIconController iconController;



    public static GameController1 Instance;

    public GameObject[] panels;

    public VideoPlayer videoPlayer;
    public VideoClip[] clips;
    public GameObject godViewer;


    bool isPlaying = false;

    //Stat UI Panel
    public TMPro.TextMeshProUGUI statPointText;
    public TMPro.TextMeshProUGUI diaText;

    //

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
            godViewer.SetActive(true);
            vp.Play();
        };
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
        if (!isPlaying && videoPlayer.clip != clips[idx])
        {
            isPlaying = true;
            videoPlayer.clip = clips[idx];
            videoPlayer.Prepare();
        }
        
    }


    private void initStatUIPanel()
    {
        statPointText.text = $"{MyProfile.Instance.getSp():N0}";
        diaText.text = $"{MyProfile.Instance.getDia():N0}";
    }
}
