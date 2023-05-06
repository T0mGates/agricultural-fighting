using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    private int numUnits;
    [Header("Unit Stats")]
    [SerializeField]
    protected int atk;
    [SerializeField]
    protected int def;
    [SerializeField]
    protected int aoe;
    [SerializeField]
    protected int healing;

    [Header("Related UI")]
    public TMP_Text atkText;
    public TMP_Text defText;
    public TMP_Text aoeText;
    public TMP_Text healingText;
    public TMP_Text numUnitsText;

    // Start is called before the first frame update
    void Start()
    {
        numUnits = 0;
        UpdateText();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnits(){
        numUnits += 1;
        UpdateText();
    }
    public void AddUnits(int numToAdd){
        numUnits += numToAdd;
        UpdateText();
    }

    public void UpdateText(){
        atkText.text = "Atk: " + atk.ToString();
        defText.text = "Def: " + def.ToString();
        aoeText.text = "AOE: " + aoe.ToString();
        healingText.text = "Healing: " + healing.ToString();
        numUnitsText.text = numUnits.ToString();
    }

    public int GetNumUnits(){
        return numUnits;
    }
}
