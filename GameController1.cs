using UnityEngine;

public class GameController1 : MonoBehaviour
{

    CharController charController;
    public SkillController skillController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SkillIconController iconController;



    public static GameController1 Instance;


    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        charController = GameObject.Find("Char").GetComponent<CharController>();
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
}
