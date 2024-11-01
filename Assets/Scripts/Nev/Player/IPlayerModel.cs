using System;

public interface IPlayerModel
{
    public int MaxHealth { get; }
    public float MoveSpeed { get; }
    public float JumpSpeed { get; }
    public float ReloadSpeed { get; }

    public void TakeDamage(int pDamage);
}