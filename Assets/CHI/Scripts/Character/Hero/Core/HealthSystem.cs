using System;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;

    private int maxLive;
    private int currentLive;
    private int maxHealth;
    private int currentHealth;

    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth; // initialize max health
        currentHealth = maxHealth; // initialize health that can take damage/heal
    }

    public HealthSystem(int maxLive, int maxHealth)
    {
        this.maxLive = maxLive;// initialize max lives
        currentLive = maxLive; // initialize live that can take be reduced/added
        this.maxHealth = maxHealth; // initialize max health
        currentHealth = maxHealth; // initialize health that can take damage/heal
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // prevent negative health
        //currentHealth = currentHealth < 0 ? 0: currentHealth;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        // prevent heal above max health
        //currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth; 

        if (currentHealth > 100)
        {
            currentHealth = 100;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void TakeLives(int live)
    {
        currentLive -= live;
        // prevent negative life
        currentLive = currentLive < 0 ? 0 : currentLive;
    }

    public void IncreaseLives(int live)
    {
        if (maxLive <= 5)
        {
            currentLive += live;
            maxLive += live;
        }
    }

    public void Recover(int live)
    {
        currentLive += live;
        // prevent recover above max lives
        currentLive = currentLive > maxLive ? maxLive : currentLive;
    }

    public int GetLivesCount() => currentLive;

    public int GetHealthPoints() { return currentHealth; }
   
    public int GetMaxHealth => maxHealth;

    public float GetHealthPercent() => (float)currentHealth / maxHealth;
}
