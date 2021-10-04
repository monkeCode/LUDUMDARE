using Unity.VisualScripting;
using UnityEngine;

public abstract class RootDoor : MonoBehaviour
{
    public GameObject @out;
    public bool locked = false;
    public string key;

    public bool TryOpen()
    {
        if (!locked || Player.Keys.Contains(key))
        {
            Open();
            return true;
        }

        return false;
    }
    public abstract void Open();
}