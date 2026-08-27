using System;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.Categorization;

public enum WeaponType
{
    Sword,
    Staff,
    Dagger,
    Shield
}

public enum ConsumableType
{
    HealthPotion,
    ManaPotion,
    StaminaPotion,
    BuffPotion
}

[CreateAssetMenu(fileName = "PlayerItems", menuName = "Heros/PlayerItems")]
public class PlayerItems : ScriptableObject
{

    public List<Weapons> Weapons;
    public List<Consumables> Consumables;
}

[System.Serializable]
public class Weapons
{
    public GameObject prefab;
    public string ID;
    [Min(0)] public int damage;
    [Min(0)] public int attackSpeed;
    public WeaponType weaponType;
}
[System.Serializable]

public class Consumables
{
    public GameObject prefab;
    public string ID;
    [Min(0)] public int effectValue;
    public ConsumableType consumableType;
}
