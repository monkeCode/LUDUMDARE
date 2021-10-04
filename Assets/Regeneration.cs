using UnityEngine;

public class Regeneration : MonoBehaviour
{
    public int healthRegeneration;
    public LayerMask layerPlayer;
    public float timeRestore = 1f;

    private float lastTimeRestore;
    private bool isPlayerStay;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (isPlayerStay && Time.time - lastTimeRestore > timeRestore)
        {
            lastTimeRestore = Time.time;
            player.RestoreHp(healthRegeneration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & layerPlayer) != 0)
        {
            isPlayerStay = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & layerPlayer) != 0)
        {
            isPlayerStay = false;
        }
    }
}
