using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int curPage = 1;
    [SerializeField]
    private int maxPages;
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
    public GameObject[] pages;
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

    public void NextPage(){
        if(curPage == maxPages){
            curPage = 1;
        }
        else{
            curPage++;
        }
        ChangePage();
    }

    private void ChangePage(){
        for(int i = 0; i < pages.Length; i++){
            pages[i].SetActive(false);
        }
        pages[curPage-1].SetActive(true);
    }

    public void ChariotUpgrade(){
        if(warrior.GetNumUnits() >= chariotWarCost && wizard.GetNumUnits() >= chariotWizCost && priest.GetNumUnits() >= chariotPriestCost){
            warrior.AddUnits(-chariotWarCost);
            wizard.AddUnits(-chariotWizCost);
            priest.AddUnits(-chariotPriestCost);
            chariotCap = (int)(chariotCap * 1.5f);

            chariotWarCost = (int)(chariotWarCost * 1.8f);
            if(chariotWizCost == 0 && chariotWarCost >= 2000){
                chariotWizCost = 200;
            }
            else{
                chariotWizCost = (int)(chariotWizCost * 1.7f);
            }
            if(chariotPriestCost == 0 && chariotWizCost >= 2000){
                chariotPriestCost = 150;
            }
            else{
                chariotPriestCost = (int)(chariotPriestCost * 1.6f);
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
        if(warrior.GetNumUnits() >= 300){
            warrior.AddUnits(-300);
            wizardUnlock.SetActive(false);
            wizardObj.SetActive(true);
        }
    }

    public void UnlockPriests(){
        if(wizard.GetNumUnits() >= 300){
            wizard.AddUnits(-300);
            priestUnlock.SetActive(false);
            priestObj.SetActive(true);
        }
    }

    private float PercentUnitsLeft(int units, float survivability, int monsterAtk, float unitEfficiency){
        float result = Mathf.Log(units) * (-1 * unitEfficiency) / ((Mathf.Pow(Mathf.Log(monsterAtk), Mathf.Log(survivability))) - Mathf.Log(monsterAtk));
        result = ((1-result) - (Mathf.Log(monsterAtk) / (Mathf.Log(monsterAtk*survivability) - Mathf.Log(monsterAtk))));
        return result;
    }

    public void StartBattle(){
        if((warriorSlider.value <= warrior.GetNumUnits() && wizardSlider.value <= wizard.GetNumUnits() && priestSlider.value <= priest.GetNumUnits()) && (warriorSlider.value > 0 || wizardSlider.value > 0 || priestSlider.value > 0)){
            switch(curBattleIndex){
            case 0:
                FightSlime();
                break;
            case 1:
                FightGoblin();
                break;
            case 2:
                FightSkeleton();
                break;
            case 3:
                FightUndead();
                break;
            case 4:
                FightBats();
                break;
            case 5:
                FightEvilKnights();
                break;
            case 6:
                FightHellsAssassin();
                break;
            case 7:
                FightBabyDragons();
                break;
            case 8:
                FightElderDragon();
                break;
            case 9:
                FightGodOfDeath();
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

    private float Fight(int monsThreshold, int monsDefense, int monsAttack, float monsNumUnitEfficiency, float healingEfficiency, float defenseEfficiency, float monsAtkEfficiency, float monsAOEEfficiency){
        float x = (warriorSlider.value * warrior.GetAtk()) + (wizardSlider.value * wizard.GetAtk()) + (priestSlider.value * priest.GetAtk());
        float a = monsDefense;
        float z = monsAtkEfficiency;

        float y = (x * (x - a))/(6-z);

        x = (warriorSlider.value * warrior.GetAOE()) + (wizardSlider.value * wizard.GetAOE()) + (priestSlider.value * priest.GetAOE());
        a = monsDefense;
        z = monsAOEEfficiency;

        float yPrime = (x * (x + a))/(6-z);

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
        if(curBattleIndex == unlockedIndex){
            unlockedIndex++;
        }
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
            outcomeText.text = "You sent out " + warriorSlider.value.ToString() + " warriors, " + wizardSlider.value.ToString() + " wizards and " + priestSlider.value.ToString() + " priests out to take down the " + title[curBattleIndex] + ", but they were not strong enough! They simply could not take the " + title[curBattleIndex] + " down!";
        }
        //too weak defense wise
        else{
            outcomeText.text = "You sent out " + warriorSlider.value.ToString() + " warriors, " + wizardSlider.value.ToString() + " wizards and " + priestSlider.value.ToString() + " priests out to take down the " + title[curBattleIndex] + ", but they were not resilient enough! The " + title[curBattleIndex] + " was able to defeat them swiftly.";
        }
        warrior.AddUnits(-1 * (int)(warriorSlider.value));
        wizard.AddUnits(-1 * (int)(wizardSlider.value));
        priest.AddUnits(-1 * (int)(priestSlider.value));
    }


    //very disgusting copy pastes, just can't bother as this is a simple low fidelity prototype
    private void FightSlime(){
        int monsThresholdMin = 60;
        int monsThresholdMax = 100;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 20;
        int monsDefenseMax = 33;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 23;
        int monsAttackMax = 35;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 1;
        float healingEfficiency = 1;
        float defenseEfficiency = 1;

        //0 - 5
        float monsAtkEfficiency = 5;
        float monsAOEEfficiency = 1;

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

    private void FightGoblin(){
        int monsThresholdMin = 80;
        int monsThresholdMax = 120;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 28;
        int monsDefenseMax = 40;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 45;
        int monsAttackMax = 58;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 1;
        float healingEfficiency = .5f;
        float defenseEfficiency = .8f;

        //0 - 5
        float monsAtkEfficiency = 5;
        float monsAOEEfficiency = 1;

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
                rewardText.text = "Your warriors used some of the Goblin's teeth to make sharper swords:\n++Warrior Atk";
                warrior.SetAtk(warrior.GetAtk() + 2);
                break;
            case 1:
                rewardText.text = "Your warriors used some of the Goblin's dagger's shards to enhance their swords:\n+Warrior Atk!";
                warrior.SetAtk(warrior.GetAtk() + 1);
                break;

            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }

    private void FightSkeleton(){
        int monsThresholdMin = 80;
        int monsThresholdMax = 120;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 36;
        int monsDefenseMax = 44;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 38;
        int monsAttackMax = 50;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 1;
        float healingEfficiency = .5f;
        float defenseEfficiency = .8f;

        //0 - 5
        float monsAtkEfficiency = 5;
        float monsAOEEfficiency = 1;

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
                rewardText.text = "Your warriors used the Skeleton's impenetrable skull bones to strengthen their armor:\n+++Warrior Def";
                warrior.SetDef(warrior.GetDef() + 3);
                break;
                
            case 1:
                rewardText.text = "Your warriors used the Skeleton's strong ribcage bones to strengthen their armor:\n++Warrior Def";
                warrior.SetDef(warrior.GetDef() + 2);
                break;
                
            case 2:
                rewardText.text = "Your warriors used some of the Skeleton's flimsy finger bones to strengthen their armor:\n+Warrior Def!";
                warrior.SetDef(warrior.GetDef() + 1);
                break;

            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }

    private void FightUndead(){
        int monsThresholdMin = 140;
        int monsThresholdMax = 170;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 60;
        int monsDefenseMax = 68;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 57;
        int monsAttackMax = 65;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 1;
        float healingEfficiency = 1;
        float defenseEfficiency = 1;

        //0 - 5
        float monsAtkEfficiency = 5;
        float monsAOEEfficiency = 1;

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
                rewardText.text = "Your warriors used the Undead's rotten teeth to sharpen their swords:\n+Warrior Atk";
                warrior.SetAtk(warrior.GetAtk() + 1);
                break;
                
            case 1:
                rewardText.text = "Your warriors used the Undead's slimy flesh to strengthen their armor:\n+Warrior Def";
                warrior.SetDef(warrior.GetDef() + 1);
                break;

            case 2:
                rewardText.text = "Your warriors used the Undead's gold tooth to strengthen their swords:\n++Warrior Atk!";
                warrior.SetAtk(warrior.GetAtk() + 2);
                break;

            case 3:
                rewardText.text = "Your warriors used the Undead's strong skull bones to strengthen their armor:\n++Warrior Def!";
                warrior.SetDef(warrior.GetDef() + 2);
                break;

            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }

    private void FightBats(){
        int monsThresholdMin = 130;
        int monsThresholdMax = 160;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 60;
        int monsDefenseMax = 65;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 150;
        int monsAttackMax = 180;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 1;
        float healingEfficiency = .8f;
        float defenseEfficiency = .8f;

        //0 - 5
        float monsAtkEfficiency = 3;
        float monsAOEEfficiency = 3;

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
                rewardText.text = "Your warriors used some of the Bats' knife-sharp teeth to make sharper swords and armor:\n+Warrior Atk, +Warrior Def";
                warrior.SetAtk(warrior.GetAtk() + 1);
                warrior.SetDef(warrior.GetDef() + 1);
                break;
            case 1:
                rewardText.text = "Your wizards used some of the Bats' magical wings to learn new spells:\n+Wizard AOE!";
                wizard.SetAOE(wizard.GetAOE() + 1);
                break;
             case 2:
                rewardText.text = "Your wizards used some of the Bats' toenails to strengthen their staffs:\n+Wizard Atk!";
                wizard.SetAtk(wizard.GetAtk() + 1);
                break;
            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }

    private void FightEvilKnights(){
        int monsThresholdMin = 200;
        int monsThresholdMax = 230;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 160;
        int monsDefenseMax = 180;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 140;
        int monsAttackMax = 160;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 1;
        float healingEfficiency = 1;
        float defenseEfficiency = 1;

        //0 - 5
        float monsAtkEfficiency = 2;
        float monsAOEEfficiency = 4;

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
                rewardText.text = "Your wizards used the Evil Knights' swords to sharpen their staffs:\n++Wizard Atk";
                wizard.SetAtk(wizard.GetAtk() + 2);
                break;
            case 1:
                rewardText.text = "Your wizards used some of the Evil Knights' dark energy to imbue their staffs with power:\n++Wizard AOE!";
                wizard.SetAOE(wizard.GetAOE() + 2);
                break;

            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }
    private void FightHellsAssassin(){
        int monsThresholdMin = 170;
        int monsThresholdMax = 200;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 80;
        int monsDefenseMax = 100;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 250;
        int monsAttackMax = 275;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 1;
        float healingEfficiency = .2f;
        float defenseEfficiency = .5f;

        //0 - 5
        float monsAtkEfficiency = 5;
        float monsAOEEfficiency = 1;

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
                rewardText.text = "Your wizards learned how to defend themselves fighting hell's assassin:\n+Wizard Def";
                wizard.SetDef(wizard.GetDef() + 1);
                break;
            case 1:
                rewardText.text = "Your wizards gained some powers of hell:\n+Wizard Atk, +Wizard AOE!";
                wizard.SetAOE(wizard.GetAOE() + 1);
                wizard.SetAtk(wizard.GetAtk() + 1);
                break;

            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }
    private void FightBabyDragons(){
        int monsThresholdMin = 220;
        int monsThresholdMax = 240;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 180;
        int monsDefenseMax = 200;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 240;
        int monsAttackMax = 260;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = .8f;
        float healingEfficiency = .8f;
        float defenseEfficiency = 1;

        //0 - 5
        float monsAtkEfficiency = 5;
        float monsAOEEfficiency = 1;

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
                rewardText.text = "Your wizards learned the baby dragons' fire spells:\n+++Wizard AOE";
                wizard.SetAOE(wizard.GetAOE() + 3);
                break;
            case 1:
                rewardText.text = "Your wizards discovered newfound power after being exposed the the baby dragons' spells:\n++Wizard AOE!";
                wizard.SetAOE(wizard.GetAOE() + 2);
                break;
             case 2:
                rewardText.text = "Your wizards got better at spell flinging:\n+Wizard AOE!";
                wizard.SetAOE(wizard.GetAOE() + 1);
                break;
            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }
    private void FightElderDragon(){
        int monsThresholdMin = 400;
        int monsThresholdMax = 440;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 250;
        int monsDefenseMax = 275;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 320;
        int monsAttackMax = 335;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 0.75f;
        float healingEfficiency = .75f;
        float defenseEfficiency = .75f;

        //0 - 5
        float monsAtkEfficiency = 5;
        float monsAOEEfficiency = 1;

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
                rewardText.text = "Your priests used the dragon eye's liquid to imbue their tomes:\n++Priest Healing";
                priest.SetHealing(priest.GetHealing() + 2);
                break;
            case 1:
                rewardText.text = "Your priests contained some dragon fire for future fights:\n+Priest AOE!";
                priest.SetAOE(priest.GetAOE() + 1);
                break;
             case 2:
                rewardText.text = "Your priests used the dragon's saliva to imbue their tomes:\n+Priest Healing!";
                priest.SetHealing(priest.GetHealing() + 1);
                break;
            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }
    private void FightGodOfDeath(){
        int monsThresholdMin = 700;
        int monsThresholdMax = 700;
        int monsThreshold = Random.Range(monsThresholdMin, monsThresholdMax+1);

        int monsDefenseMin = 350;
        int monsDefenseMax = 350;
        int monsDefense = Random.Range(monsDefenseMin, monsDefenseMax+1);

        int monsAttackMin = 400;
        int monsAttackMax = 400;
        int monsAttack = Random.Range(monsAttackMin, monsAttackMax+1);

        //0 - 1
        float monsNumUnitEfficiency = 0.4f;
        float healingEfficiency = 1f;
        float defenseEfficiency = 0.75f;

        //0 - 5
        float monsAtkEfficiency = 4;
        float monsAOEEfficiency = 3;

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
                rewardText.text = "You've gained eternal life. Good game.";
                break;
            default:
                Debug.Log("WEIRD.. SHOULDNT HAPPEN EITHER");
                break;
        }
    }
}
