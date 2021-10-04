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
    public Slider ShipSlider;
    public RawImage reactorBgImage;
    public RawImage reactorImage;
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
        Reactor.OnItemRequired += (sender, item) => ChangeReactorRequirement(item);
        SpaceShipTimer.TimeChanged += (sender, time) => UpdateScrollbar(ShipSlider, time.Current, time.Max);
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

    private void ChangeReactorRequirement(TypeItem type)
    {
        var isNotNull = images[(int) type] != null;
        reactorBgImage.enabled = isNotNull;
        reactorImage.texture = images[(int)type];
        reactorImage.color = new Color(1, 1, 1, isNotNull ? 1 : 0);
    }
}
