
public interface IStamina
{
    void OnConsumeStamina(float amount);
    bool HasStaminaToAction(float cost);
}
