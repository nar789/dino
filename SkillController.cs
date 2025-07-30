using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static SkillController Instance;

    public GameObject magic;
    public GameObject explosion;

    bool isAuto = true;
    public Image autoButtonImage;

    bool[] skillStatus = { false, false, false, false };
    private void Awake()
    {
        Instance = this;
    }

    public bool getSkill(int idx)
    {
        return skillStatus[idx];
    }

    public void clearSkill(int idx)
    {
        skillStatus[idx] = false;
    }


    public void setSkill(int idx)
    {
        skillStatus[idx] = true;
    }

    public void generateMagic(Vector3 pos, Quaternion rot)
    {
        Instantiate(magic, pos, rot);
    }

    public void generateExplosion(Vector3 pos, Quaternion rot)
    {
        Instantiate(explosion, pos, rot);
    }

    public void setAuto(bool auto)
    {
        isAuto = auto;

        if(isAuto)
        {
            autoButtonImage.color = new Color32(255, 255, 255, 255);
        } else
        {
            autoButtonImage.color = new Color32(0, 0, 0, 200);
        }

    }

    public bool getIsAuto()
    {
        return isAuto;
    }



}
