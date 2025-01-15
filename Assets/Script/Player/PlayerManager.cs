using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Copy_Ability
{
    Normal,
    Sword,
    Fire,
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;

    public int HP = 0;
    public int MaxHP = 0;
    public Copy_Ability copy_Ability = Copy_Ability.Normal;


}
