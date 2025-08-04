using UnityEngine;
using UnityEngine.UI;

public class MyProfile : MonoBehaviour
{

    public static MyProfile Instance;


    int hpCapa;
    int hp = 1000; //db
    float exp = 0f; //db 
    int level = 1; //db
    int dia; //db
    int sp = 0;  //db
    float str; //��  //db
    float dex; //�̼�, ��ø��  //db
    float con; //hp�� ũ��, ������  //db
    float inte; //���� ���ݷ�, ����  //db


    //local saved
    int gold;
    string missionStr = "0,0,0,0,0,0,0";
    int gpm; //�̼Ǵ� ��� gold per mission
    int[] mission = { 0, 0, 0, 0, 0, 0, 0};
    //�̼� ��ü������������,����������ȣ,�̼ǰ���,�̼Ǽ��࿩��


    //fixed constant
    float expWeight = 0.89f;
    float spWeight = 1.08f;


    public TMPro.TextMeshProUGUI levelText;

    public Slider expSlider;
    public TMPro.TextMeshProUGUI expText;

    public Slider hpSlider;
    public TMPro.TextMeshProUGUI hpText;

    CharController charController;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        charController = GameObject.Find("Char").GetComponent<CharController>();

        expSlider.maxValue = 100;
        expSlider.value = exp;
        expText.text = exp.ToString("F2") + "%";

        hpCapa = hp;
        hpSlider.maxValue = hpCapa;
        hpSlider.value = hp;
        hpText.text = getHpString();
        charController.initHp(hpCapa, hp);

        levelText.text = "Lv. " + level;
    }

    private string getHpString()
    {
        string hpStr = $"{hp:N0}";
        string hpCapaStr = $"{hpCapa:N0}";
        return hpStr + " / " + hpCapaStr;
    }

    
    void Update()
    {
        
    }

    public void updateExp()
    {
        exp += 30 * Mathf.Pow(expWeight, level - 1);
        if(exp >= 100)
        {
            level += 1;
            exp -= 100;
            levelText.text = "Lv. " + level;
            //start level up fx;
            charController.startLevelUpFx();
            //get stat point 
            updateSp();
        }
        
        expSlider.value = exp;
        expText.text = exp.ToString("F2") + "%";
    }

    private void updateSp()
    {
        sp += Mathf.FloorToInt(Mathf.Pow(spWeight, level - 1));
        Debug.Log("sp : " + sp);
    }

    public void updateHp(int attackPower)
    {
        hp -= attackPower;
        if (hp < 0)
        {
            hp = 0;
        }

        hpSlider.value = hp;
        hpText.text = getHpString();
    }

    public int getHp()
    {
        return hp;
    }

    public int getHpCapa()
    {
        return hpCapa;
    }
}
