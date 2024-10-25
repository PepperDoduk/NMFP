using System;

public interface N_IPlayerModel
{
    public int MaxHealth { get; }
    public float MoveSpeed { get; }
    public float JumpSpeed { get; }
    public float ReloadSpeed { get; }

    public void TakeDamage(int pDamage);
}