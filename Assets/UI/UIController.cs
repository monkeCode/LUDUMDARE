using System.Collections;
using System.Collections.Generic;
using ReactorScripts;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider OverheatSlider;
    public Slider HealthSlider;
    public Slider ReactorSlider;

    private void Start()
    {
        var player = FindObjectOfType<Player>();
        var weapon = player.GetComponent<Weapon>();
        player.HealthChanged += (sender, hp) => UpdateScrollbar(HealthSlider, hp.Current, hp.Max);
        weapon.OverheatChanged += (sender, overheat) => UpdateScrollbar(OverheatSlider, overheat, 100);
        Reactor.OnHealthChanged += (sender, data) => UpdateScrollbar(ReactorSlider, data.Health, 100);
    }
    
    public void UpdateScrollbar(Slider slider, float current, float max)
    {
        slider.value = current / max * 100;
    } 
}
