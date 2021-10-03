using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnItemInspector : MonoBehaviour
{
    // Start is called before the first frame update
    public ReactorScripts.Item Item1;
    public ReactorScripts.Item Item2;
    public ReactorScripts.Item Item3;
    private List<Transform> possibleSpawnPositions;
    private Dictionary<Transform, bool> positionIsTaken = new Dictionary<Transform, bool>();
    [SerializeField]private Dictionary<string, Transform> ItemsPositions = new Dictionary<string, Transform>();
    private System.Random rnd = new System.Random();

    void Start()
    {
        possibleSpawnPositions = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        foreach(var point in possibleSpawnPositions)
        {
            positionIsTaken[point] = false; 
        }
        SpawnItem(Item1);
        Debug.Log(Item1.data.name);
        SpawnItem(Item2);
        Debug.Log(Item2.data.name);
        SpawnItem(Item3);
        Debug.Log(Item3.data.name);
    }

    private Transform GetPosition()
    {
        var position = rnd.Next(possibleSpawnPositions.Count);
        while (positionIsTaken[possibleSpawnPositions[position]])
            position = rnd.Next(possibleSpawnPositions.Count);
        return possibleSpawnPositions[position];
    }
    private void SpawnItem(ReactorScripts.Item item)
    {
        var position = GetPosition();
        Instantiate(item, position.position, position.rotation);
        Debug.Log("spawned");
        ItemsPositions[item.data.name] = position;
        positionIsTaken[position] = true;
    }

    public void TakeItem(ReactorScripts.Item item)
    {
        var itemPosition = ItemsPositions[item.data.name];
        Debug.Log(item.data.name);
        positionIsTaken[itemPosition] = false;
        SpawnItem(item);
    }
}