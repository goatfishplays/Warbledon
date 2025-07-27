using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EntityHealth eh; // TODO: is there a better way to link the player health to the health bar.
    public Image barImage;
    public bool isSubscribed = false;
    // public Slider healthSlider;
    // public TextMeshProUGUI healthNumbers;

    // // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     // healthSlider.minValue = 0;
    //     // healthSlider.maxValue = player.entityHealth.GetMaxHealth();
    //     // healthSlider.value = player.entityHealth.GetHealth();
    //     // player.entityHealth.OnHealthChange += HandleHealthChange;
    //     // SetHealthNumbers(healthSlider.value, healthSlider.maxValue);

    // } 

    public void Initialize(EntityHealth newEntityHealth)
    {
        if (isSubscribed)
        {
            eh.OnHealthChange -= UpdateHealthbar;
        }
        eh = newEntityHealth;
        eh.OnHealthChange += UpdateHealthbar;
        isSubscribed = true;
        UpdateHealthbar();
    }

    void OnDestroy()
    {
        if (isSubscribed)
        {
            eh.OnHealthChange -= UpdateHealthbar;
        }
    }


    /// <summary>
    /// Note that the param doesn't do anything, just need because OnHealthChange exists
    /// </summary>
    /// <param name="_"></param>
    public void UpdateHealthbar()
    {
        barImage.fillAmount = eh.GetHealth() / eh.GetMaxHealth();
    }

    // // Unused
    // //void Update()
    // //{

    // //}

    // // This is attached to player.entityHealth.OnHealthChange
    // // Changes the slider when health is changed. Input is sanitized by the entityHealth script
    // void HandleHealthChange(float delta)
    // {
    //     healthSlider.value += delta;
    //     SetHealthNumbers(healthSlider.value, healthSlider.maxValue);
    // }

    // void SetHealthNumbers(float currentHealth, float maxHealth)
    // {
    //     // TODO: This is inefficient. Likely can make it update less.
    //     healthNumbers.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    // }
}
