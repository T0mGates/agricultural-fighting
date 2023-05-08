using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitManager : MonoBehaviour
{
    [Header("Unit Related")]
    [SerializeField]
    protected Unit unit;
    [SerializeField]
    protected float startInterval;

    [SerializeField]
    protected int costPlant;
    [SerializeField]
    protected int costHire;

    [Header("UI Related")]
    [SerializeField]
    protected GameObject plantBtn;
    [SerializeField]
    protected GameObject plantText;
    [SerializeField]
    protected GameObject plantTextTwo;
    public TMP_Text hireCostText;
    public TMP_Text plantCostText;


    protected int numPerInterval;
    protected float interval;
    protected float timer;

    // Start is called before the first frame update
    void Start()
    {
        numPerInterval = 1;
        interval = -1;
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        TimerUpdate();
    }

    protected void AddUnits(int numToAdd){
        unit.AddUnits(numToAdd);
    }

    private void TimerUpdate(){
        if(interval != -1){
            if(timer <= Time.time){
                AddUnits(numPerInterval);
                timer = Time.time + interval;
            }
        }
    }

    public void UpgradeClick(string type){
        //type can = warrior, wizard, priest
        switch(type){
            case "warrior":
                if(unit.GetAtk() > 1 && unit.GetDef() > 1){
                    unit.SetAtk(unit.GetAtk() - 1);
                    unit.SetDef(unit.GetDef() - 1);
                    unit.AddBonusOnClick(1);
                }
                break;
            case "wizard":
                if(unit.GetAtk() > 1 && unit.GetAOE() > 1){
                    unit.SetAtk(unit.GetAtk() - 1);
                    unit.SetAOE(unit.GetAOE() - 1);
                    unit.AddBonusOnClick(1);
                }
                break;
            case "priest":
                if(unit.GetHealing() > 2){
                    unit.SetHealing(unit.GetHealing() - 2);
                    unit.AddBonusOnClick(1);
                }
                break;
        }
    }

    public void PlantHeads(){
        if(unit.GetNumUnits() >= costPlant){
            unit.AddUnits(-costPlant);
            costPlant *= 2;
            UpdateText(); 
            numPerInterval = (int)(numPerInterval * 2);
        }
    }

    public void HireFarmer(){
        if(unit.GetNumUnits() >= costHire){
            unit.AddUnits(-costHire);
            costHire *= 3;
            UpdateText(); 
            if(interval == -1){
            interval = startInterval;
            plantBtn.SetActive(true);
            plantText.SetActive(true);
            plantTextTwo.SetActive(true);
        }
        else{
            interval *= 0.75f;
        }
        timer = Time.time + interval;
        }
    }

    public void UpdateText(){
        plantCostText.text = costPlant.ToString();
        hireCostText.text = costHire.ToString();
    }
}
