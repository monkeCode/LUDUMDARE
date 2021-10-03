using Unity.VisualScripting;
using UnityEngine;

public abstract class RootDoor : MonoBehaviour
{
    public GameObject @out;
    public bool locked = false;
    public string key;

    public void TryOpen()
    {
        if (!locked || Player.Keys.Contains(key))
            Open();
    }
    public abstract void Open();
}