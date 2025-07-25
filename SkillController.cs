using UnityEngine;

public class SkillController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static SkillController Instance;

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

}
