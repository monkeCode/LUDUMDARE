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
    public RawImage inventoryImage;

    public Texture[] images;

    private void Start()
    {
        var player = FindObjectOfType<Player>();
        var weapon = player.GetComponent<Weapon>();
        player.HealthChanged += (sender, hp) => UpdateScrollbar(HealthSlider, hp.Current, hp.Max);
        weapon.OverheatChanged += (sender, overheat) => UpdateScrollbar(OverheatSlider, overheat, 100);
        Reactor.OnHealthChanged += (sender, data) => UpdateScrollbar(ReactorSlider, data.Health, 100);
        player.itemChanged += (sender, data) => ChangeImage(data.type);
    }

    private static void UpdateScrollbar(Slider slider, float current, float max)
    {
        slider.value = current / max * 100;
    }

    private void ChangeImage(TypeItem type)
    {
        inventoryImage.texture = images[(int)type];
        inventoryImage.color = new Color(1, 1, 1, images[(int) type] != null  ? 1 : 0);
    }
}
