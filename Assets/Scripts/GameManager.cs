using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class TwoDimensionalFloatArray{
        public float[] array;
    }
    [System.Serializable]
    public class TwoDimensionalStringArray{
        public string[] array;
    }

    [Header("Battle Related")]
    [SerializeField]
    private GameObject[] battles;
    private int unlockedIndex = 0;
    private int curBattleIndex = 0;

    [SerializeField]
    private string[] descriptions;
    [SerializeField]
    private string[] title;
    [SerializeField]
    private TwoDimensionalFloatArray[] rewardsPercent;
    [SerializeField]
    private TwoDimensionalStringArray[] rewardsNames;

    [Header("Units Related")]
    [SerializeField]
    private Unit warrior;
    [SerializeField]
    private Unit wizard;
    [SerializeField]
    private Unit priest;

    [Header("UI Related")]
    public TMP_Text chariotCapText;
    public TMP_Text chariotCapTextTwo;
    public TMP_Text chariotUpgradeText;
    [SerializeField]
    private GameObject battleScreen;
    [SerializeField]
    private GameObject mainUIScreen;
    public TMP_Text battleTitleText;
    public TMP_Text battleDescriptionText;
    public TMP_Text battleRewardsText;
    public TMP_Text numWarriorsText;
    public TMP_Text numWizardsText;
    public TMP_Text numPriestsText;
    public Slider warriorSlider;
    public Slider wizardSlider;
    public Slider priestSlider;
    [SerializeField]
    private GameObject priestUnlock;
    [SerializeField]
    private GameObject wizardUnlock;
    [SerializeField]
    private GameObject wizardObj;
    [SerializeField]
    private GameObject priestObj;

    [Header("Reward UI Related")]
    [SerializeField]
    private GameObject rewardsUI; 
    public TMP_Text outcomeText;
    public TMP_Text casualtyText;
    public TMP_Text rewardText;
    public TMP_Text monsterTitleText;

    private int chariotCap = 50;
    private int chariotWarCost = 300;
    private int chariotWizCost = 0;
    private int chariotPriestCost = 0;

    private int prevWarValue = 0;
    private int prevWizValue = 0;
    private int prevPriestValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChariotUpgrade(){
        if(warrior.GetNumUnits() >= chariotWarCost && wizard.GetNumUnits() >= chariotWizCost && priest.GetNumUnits() >= chariotPriestCost){
            warrior.AddUnits(-chariotWarCost);
            wizard.AddUnits(-chariotWizCost);
            priest.AddUnits(-chariotPriestCost);
            chariotCap = (int)(chariotCap * 1.5f);

            chariotWarCost = (int)(chariotWarCost * 2.5f);
            if(chariotWizCost == 0 && chariotWarCost >= 2000){
                chariotWizCost = 200;
            }
            else{
                chariotWizCost = (int)(chariotWizCost * 1.95f);
            }
            if(chariotPriestCost == 0 && chariotWizCost >= 2000){
                chariotPriestCost = 150;
            }
            else{
                chariotPriestCost = (int)(chariotPriestCost * 1.75f);
            }
        }
        UpdateText();
    }

    public void BattleScreen(int battleIndex){
        battleScreen.SetActive(true);
        mainUIScreen.SetActive(false);
        curBattleIndex = battleIndex;
        battleTitleText.text = title[battleIndex];
        monsterTitleText.text = title[battleIndex];
        battleDescriptionText.text = descriptions[battleIndex];
        string newRewardText = "";
        for(int i = 0; i < rewardsPercent[battleIndex].array.Length; i++){
            if(i > 0){
                newRewardText += ",\n";
            }
            newRewardText += rewardsPercent[battleIndex].array[i].ToString() + "%: " + rewardsNames[battleIndex].array[i].ToString();
        }
        battleRewardsText.text = newRewardText;
        warriorSlider.maxValue = chariotCap;
        wizardSlider.maxValue = chariotCap;
        priestSlider.maxValue = chariotCap;
    }

    public void MainUIScreen(){
        rewardsUI.SetActive(false);
        battleScreen.SetActive(false);
        mainUIScreen.SetActive(true);
        ResetSliders();
    }

    private void ResetSliders(){
        warriorSlider.value = 0;
        prevWarValue = 0;
        wizardSlider.value = 0;
        prevWizValue = 0;
        priestSlider.value = 0;
        prevPriestValue = 0;
    }

    private bool SliderValuesOverCap(){
        return warriorSlider.value + wizardSlider.value + priestSlider.value > chariotCap;
    }

    public void ChangeWarSliderValue(){
        if(!SliderValuesOverCap()){
            prevWarValue = (int) warriorSlider.value;
        }
        else{
            warriorSlider.value = prevWarValue;
        }
        numWarriorsText.text = warriorSlider.value.ToString();
    }

    public void ChangeWizSliderValue(){
        if(!SliderValuesOverCap()){
            prevWizValue = (int) wizardSlider.value;
        }
        else{
            wizardSlider.value = prevWizValue;
        }
        numWizardsText.text = wizardSlider.value.ToString();
    }

    public void ChangePriestSliderValue(){
        if(!SliderValuesOverCap()){
            prevPriestValue = (int) priestSlider.value;
        }
        else{
            priestSlider.value = prevPriestValue;
        }
        numPriestsText.text = priestSlider.value.ToString();
    }

    public void UpdateText(){
        chariotCapText.text = "Chariot Capacity: " + chariotCap.ToString();
        chariotCapTextTwo.text = "Chariot Capacity: " + chariotCap.ToString();
        chariotUpgradeText.text = chariotWarCost.ToString() + " Warriors, " + chariotWizCost.ToString() + " Wizards, " + chariotPriestCost.ToString() + " Priests";
    }

    public void UnlockWizards(){
        if(warrior.GetNumUnits() >= 10){
            warrior.AddUnits(-10);
            wizardUnlock.SetActive(false);
            wizardObj.SetActive(true);
        }
    }

    public void UnlockPriests(){
        if(wizard.GetNumUnits() >= 10){
            wizard.AddUnits(-10);
            priestUnlock.SetActive(false);
            priestObj.SetActive(true);
        }
    }

    private float PercentUnitsLeft(int units, float survivability, int monsterAtk, float unitEfficiency){
        float result = Mathf.Log(units) * (-1 * unitEfficiency) / (Mathf.Log(Mathf.Pow(monsterAtk, survivability)) - Mathf.Log(monsterAtk));
        result = ((1-result) - (Mathf.Log(monsterAtk) / (Mathf.Log(monsterAtk*survivability) - Mathf.Log(monsterAtk))));
        return result;
    }

    public void StartBattle(){
        if((warriorSlider.value <= warrior.GetNumUnits() && wizardSlider.value <= wizard.GetNumUnits() && priestSlider.value <= priest.GetNumUnits()) && (warriorSlider.value > 0 || wizardSlider.value > 0 || priestSlider.value > 0)){
            switch(curBattleIndex){
            case 0:
                FightSlime();
                break;
            }
            wizardSlider.value = 0;
            warriorSlider.value = 0;
            priestSlider.value = 0;
            prevPriestValue = 0;
            prevWarValue = 0;
            prevWizValue = 0;
        }  
    }

    private float Fight(int monsThreshold, int monsDefense, int monsAttack, int monsNumUnitEfficiency, int healingEfficiency, int defenseEfficiency, int monsAtkEfficiency, int monsAOEEfficiency){
        float x = (warriorSlider.value * warrior.GetAtk()) + (wizardSlider.value * wizard.GetAtk()) + (priestSlider.value * priest.GetAtk());
        float a = monsDefense;
        float z = monsAtkEfficiency;

        float y = (x * (x - a))/z;

        x = (warriorSlider.value * warrior.GetAOE()) + (wizardSlider.value * wizard.GetAOE()) + (priestSlider.value * priest.GetAOE());
        a = monsDefense;
        z = monsAOEEfficiency;

        float yPrime = (x * (x + a))/z;

        if((yPrime + y) * (1 + ((priest.GetHealing() * priestSlider.value) * healingEfficiency)) > monsThreshold){
            int numUnits = (int)(warriorSlider.value + priestSlider.value + wizardSlider.value);
            float survivabilityValue = ((warrior.GetDef()*warriorSlider.value + wizard.GetDef()*wizardSlider.value + priest.GetDef()*priestSlider.value) * defenseEfficiency) + ((warrior.GetHealing()*warriorSlider.value + wizard.GetHealing()*wizardSlider.value + priest.GetHealing()*priestSlider.value) * healingEfficiency);
            y = PercentUnitsLeft(numUnits, survivabilityValue, monsAttack, monsNumUnitEfficiency);
            if(y <= 0){
                return -1;
            }      
            else{
                return y;
            }
        }
                
        else{
            return -2;
        }
    }

    private void FightSlime(){
        int monsThresholdMin = 60;
        int monsThresholdMax = 110;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 10;
        int monsDefenseMax = 30;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 35;
        int monsAttackMax = 50;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        int monsNumUnitEfficiency = 1;
        int healingEfficiency = 1;
        int defenseEfficiency = 1;

        //0 - 5
        int monsAtkEfficiency = 5;
        int monsAOEEfficiency = 1;

        float unitsLeft = Fight(monsThreshold, monsDefense, monsAttack, monsNumUnitEfficiency, healingEfficiency, defenseEfficiency, monsAtkEfficiency, monsAOEEfficiency);
        
        int index = FightAftermath(unitsLeft);
        battleScreen.SetActive(false);
        rewardsUI.SetActive(true);
        
        //outcomeText; casualtyText; rewardText;
        //show upgrade panel, then switch to main UI after a while
        switch(index){
            case -2:
                Debug.Log("SHOULD NEVER SEE THIS!");
                break;
            case -1:
                rewardText.text = "No Reward!";
                break;
            
            case 0:
                rewardText.text = "Your warriors used some of the Slime's goo to upgrade their armor:\n+Warrior Def!";
                warrior.SetDef(warrior.GetDef() + 1);
                break;
            case 1:
                rewardText.text = "Your warriors used some of the Slime's goo to sharpen their swords!:\n+Warrior Atk!";
                warrior.SetAtk(warrior.GetAtk() + 1);
                break;

            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }

    private int FightAftermath(float unitsLeft){
        outcomeText.text = "You sent out " + warriorSlider.value.ToString() + " warriors, " + wizardSlider.value.ToString() + " wizards and " + priestSlider.value.ToString() + " priests out to take down the " + title[curBattleIndex] + ", and they emerged victorious!";
        casualtyText.text = "These were the casualties from this battle:\n- " + ((int)(warriorSlider.value - warriorSlider.value*unitsLeft)).ToString() + " warriors,\n- " + ((int)(wizardSlider.value - wizardSlider.value*unitsLeft)).ToString() + " wizards,\n- " + ((int)(priestSlider.value - priestSlider.value*unitsLeft)).ToString() + " priests";

        if(unitsLeft <= 0){
            BattleLost(unitsLeft);
            return -1;
        }
        else if(unitsLeft >= 0.85f){
            unitsLeft = Random.Range(0.7f, 0.85f);
        }

        warrior.AddUnits(-1 * (int)(warriorSlider.value - warriorSlider.value*unitsLeft));
        wizard.AddUnits(-1 * (int)(wizardSlider.value - wizardSlider.value*unitsLeft));
        priest.AddUnits(-1 * (int)(priestSlider.value - priestSlider.value*unitsLeft));

        unlockedIndex++;
        if(unlockedIndex < battles.Length){
            battles[unlockedIndex].SetActive(true);
        }
        int rand = Random.Range(1, 101);
        float sum = 0;
        for(int i = 0; i < rewardsPercent[curBattleIndex].array.Length; i++){
            sum += rewardsPercent[curBattleIndex].array[i];
            if(sum >= rand){
                return i;
            }
        }
        return -2;
    }

    private void BattleLost(float unitsLost){
        casualtyText.text = "These were the casualties from this battle:\n- " + ((int)(warriorSlider.value)).ToString() + " warriors,\n- " + ((int)(wizardSlider.value)).ToString() + " wizards,\n- " + ((int)(priestSlider.value)).ToString() + " priests";
        //too weak attack wise
        if(unitsLost == -2){
            outcomeText.text = "You sent out " + warriorSlider.value.ToString() + " warriors, " + wizardSlider.value.ToString() + " wizards and " + priestSlider.value.ToString() + " priests out to take down the " + title[curBattleIndex] + ", but they were not strong enough! They will never return home.";
        }
        //too weak defense wise
        else{
            outcomeText.text = "You sent out " + warriorSlider.value.ToString() + " warriors, " + wizardSlider.value.ToString() + " wizards and " + priestSlider.value.ToString() + " priests out to take down the " + title[curBattleIndex] + ", but they were not resilient enough! The " + title[curBattleIndex] + " was able to defeat them swiftly.";
        }
        warrior.AddUnits(-1 * (int)(warriorSlider.value));
        wizard.AddUnits(-1 * (int)(wizardSlider.value));
        priest.AddUnits(-1 * (int)(priestSlider.value));
    }
}
