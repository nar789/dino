using UnityEngine;

public class GameController1 : MonoBehaviour
{

    CharController charController;
    public SkillController skillController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        charController = GameObject.Find("Char").GetComponent<CharController>();
    }
    public void onSkillClick(int id)
    {
        Debug.Log("skill " + id);
        if (id == 2)
        {
            Vector3 dest = charController.transform.position + Vector3.zero;
            dest.y = 0.5f;
            SkillController.Instance.generateMagic(dest, charController.transform.rotation);
        } else
        {
            SkillController.Instance.setSkill(id);
        }
        
        
    }
}
