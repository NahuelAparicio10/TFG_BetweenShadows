
public readonly struct DodgeCmd : ICommand
{
    public int Priority => 100;
}

public readonly struct LightAttackCmd : ICommand
{
    public int Priority => 90;
}

public readonly struct HeavyAttackCmd : ICommand
{
    public int Priority => 80;
}

public readonly struct SprintCmd : ICommand
{
    public int Priority => 60;
}

public readonly struct InteractCmd : ICommand
{
    public int Priority => 50;
}