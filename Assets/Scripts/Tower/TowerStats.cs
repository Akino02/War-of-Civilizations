using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShowStats : MonoBehaviour
{
    ButtonScript buttonS;

    public Text turretLvlText;
    public Text turretDamageText;
    public Text turretFireRateText;
    public Text turretPriceText;

    private void Awake()
    {
        buttonS = GetComponent<ButtonScript>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckTurretStats();
        turretPriceText.text = IsTurretActive();
    }
    private void CheckTurretStats()
    {
        if (buttonS.towerS.gameObject.activeSelf == true)
        {
            turretLvlText.text = $"Level: {buttonS.towerS.lvl + 1}";
            turretFireRateText.text = $"FireRate: {UnityConfiguration.fireRate[buttonS.towerS.lvl]}s";
            turretDamageText.text = $"Damage: {buttonS.towerS.bulletDamage * (buttonS.towerS.lvl + 1)}";
            return;
        }
        turretLvlText.text = $"Level: {buttonS.currLevelBase+1}";
        turretFireRateText.text = $"FireRate: {UnityConfiguration.fireRate[buttonS.currLevelBase]}s";
        turretDamageText.text = $"Damage: {UnityConfiguration.bulletDamage * (buttonS.currLevelBase + 1)}";
    }

    private string IsTurretActive()
    {
        if (buttonS.towerS.gameObject.activeSelf == true)
        {
            return $"Price: {(int)Mathf.Round((UnityConfiguration.moneyForTurret * (buttonS.towerS.lvl + 1)) * UnityConstants.getMoneyBackPercentage)}";
        }
        return $"Price: {UnityConfiguration.moneyForTurret * (buttonS.currLevelBase + 1)}";
    }
}
