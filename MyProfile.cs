using UnityEngine;
using UnityEngine.UI;

public class MyProfile : MonoBehaviour
{

    public static MyProfile Instance;


    int hpCapa;
    int hp = 1000; //db
    float exp = 0f; //db 
    int level = 1; //db
    int dia = 30000; //db
    int sp = 2000000;  //db


    int[] statLevel = {1, 1, 1, 1, 1 };
    int[] stat = {1000, 700, 1000, 1000, 1000 };


    //local saved
    int gold = 0;
    int missionLevel = 0;
    int buildingCount = 0;
    int[] totalBuildingCount = { 7, 12 };
    
    string[] missionName = {"¼®±Ã °Ç¼³", "¼®±Ã °Ç¼³"};


    //fixed constant
    float expWeight = 0.89f;
    float spWeight = 1.08f;


    public TMPro.TextMeshProUGUI levelText;

    public Slider expSlider;
    public TMPro.TextMeshProUGUI expText;

    public Slider hpSlider;
    public TMPro.TextMeshProUGUI hpText;

    CharController charController;

    bool[] buff = { false, false, false, false, false};


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
        levelText.text = "Lv. " + level;

        updateHpCapa();
    }

    public void updateHpCapa()
    {
        hpCapa = getStat(2);
        //Debug.Log("hpCapa " + hpCapa);
        hpSlider.maxValue = hpCapa;
        hpSlider.value = hp;
        hpText.text = getHpString();
        charController.initHp(hpCapa, hp);
    }

    private string getHpString()
    {
        string hpStr = $"{hp:N0}";
        string hpCapaStr = $"{hpCapa:N0}";
        return hpStr + " / " + hpCapaStr;
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
            //heal the hp
            hp = hpCapa;
            updateHpCapa();
        }
        
        expSlider.value = exp;
        expText.text = exp.ToString("F2") + "%";
    }

    private void updateSp()
    {
        sp += Mathf.FloorToInt(Mathf.Pow(spWeight, level - 1));
        Debug.Log("sp : " + sp);
    }

    public void gameOver()
    {
        hp = hpCapa;
        updateHpCapa();
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
    public int getSp()
    {
        return sp; 
    }

    public int getDia()
    {
        return dia;
    }

    public int getStat(int idx)
    {
        int b = 0; 
        
        if(buff[idx])
        {
            b = (int)(stat[idx] * 0.1f);
        }
        //Debug.Log("idx " + idx + " / " + (stat[idx] + b) + " / " + buff[idx]);
        return stat[idx] + b;
    }

    public int getStatLevel(int idx)
    {
        return statLevel[idx];
    }

    public bool useStatPoint()
    {
        if(sp > 0)
        {
            sp -= 1;
            return true;
        } else
        {
            return false;
        }
        
    }

    public void addDia(int amount)
    {
        dia += amount;
    }

    public bool useDia(int amount)
    {
        if(dia - amount >= 0)
        {
            dia -= amount;
            return true;
        } else
        {
            return false;
        }
    }

    public void levelUpStat(int idx)
    {
        //str, dex, con, inte, wis
        statLevel[idx] += 1;

      
        if(idx == 0)
        {
            stat[0] += 500;
        } else if(idx == 1)
        {
            stat[1] += 7;
        } else if(idx == 2)
        {
            stat[2] += 1000;
            updateHpCapa();
        } else if(idx == 3)
        {
            stat[idx] += 10;
        } else if(idx == 4)
        {
            stat[idx] += 20;
        }


    }

    public void setBuff(int idx, bool status)
    {
        buff[idx] = status;
    }

    public int getGold()
    {
        return gold;
    }

    public void addGold()
    {
        gold += 1;
    }

    public void useGold()
    {
        if (gold - 1 >= 0)
        {
            gold -= 1;
        }
    }

    public int getTotalBuildingCount()
    {
        if(totalBuildingCount.Length <= missionLevel)
        {
            return totalBuildingCount[totalBuildingCount.Length - 1];
        } else
        {
            return totalBuildingCount[missionLevel];
        }
        
    }

    public int getBuildingCount()
    {
        return buildingCount;
    }

    public string getMissionName()
    {
        if(missionName.Length <= missionLevel)
        {
            return missionName[missionName.Length - 1];
        }
        return missionName[missionLevel];
    }

    public void addBuildingCount()
    {
        if(buildingCount + 1 <= getTotalBuildingCount())
        {
            buildingCount += 1;
        }
    }

    public bool addMissionLevelAndClearBuildingCount()
    {
        if(missionLevel + 1 < missionName.Length)
        {
            buildingCount = 0;
            missionLevel += 1;
            return true;
        } else
        {
            buildingCount = 0;
            missionLevel += 1;
            return false;
        }
    }

    public int getMissionLevel()
    {
        return missionLevel;
    }
}
