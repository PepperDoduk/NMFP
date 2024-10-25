using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(
    fileName = "PlayerData",
    menuName = "Data/PlayerData",
    order = int.MinValue)]
public class N_PlayerData : ScriptableObject
{
    public int MaxHealth;
    public float MoveSpeed;
    public float JumpSpeed;
    public float ReloadSpeed;
}
