using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyScript : MonoBehaviour
{
    public Slider slider;
    public Text showDifficultyText;
    //private Difficulty currDifficultySet;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = Enum.GetValues(typeof(Difficulty)).Length - 1;
        //currDifficultySet = SetDifficulty((int)slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        SetDifficultyToGame((int)slider.value);
        SetTextToShow();
        //Debug.Log(SetDifficulty((int)slider.value));
    }
    private Difficulty SetDifficulty(int index)
    {
        return (Difficulty)Enum.GetValues(typeof(Difficulty)).GetValue((int)slider.value);
    }
    private void SetTextToShow()
    {
        Color difficultyColor = Color.Lerp(Color.green, Color.red, (((int)slider.value)/slider.maxValue));
        showDifficultyText.color = difficultyColor;
        showDifficultyText.text = (SetDifficulty((int)slider.value)).ToString();
    }
    private void SetDifficultyToGame(int index)
    {
        //pro ziskani hodnoty z enum
        EnemySpawn.currDifficulty = SpawnAfterAttribute.GetEnemies(SetDifficulty(index));
    }
}


public enum Difficulty
{
    [SpawnAfter(5)]
    Easy,
    [SpawnAfter(7)]
    Medium,
    [SpawnAfter(9)]
    Hard,
    /*[SpawnAfter(60)]
    Impossible,*/
}

[AttributeUsage(AttributeTargets.All)]
public class SpawnAfterAttribute : Attribute
{
    public int enemies { get; private set; }
    public SpawnAfterAttribute(int enemies)
    {
        this.enemies = enemies;
    }
    public static int GetEnemies(Enum value)
    {
        Type type = value.GetType();

        string name = Enum.GetName(type, value);

        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                SpawnAfterAttribute attr = GetCustomAttribute(field, typeof(SpawnAfterAttribute)) as SpawnAfterAttribute;
                if (attr != null)
                {
                    return attr.enemies;
                }
            }
        }

        return 5;
    }
}