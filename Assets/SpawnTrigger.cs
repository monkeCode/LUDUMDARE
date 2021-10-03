using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
   public bool canSpawn;
    List<SpawnPoint> points;
    private void Start()
    {
        points = new List<SpawnPoint>(transform.GetComponentsInChildren<SpawnPoint>());
    }
    private IEnumerator SpawnMonsters(int count, GameObject spawnPoint, Entity mob)
    {
        for (var i = 0; i < count; i++)
        {
            Instantiate(mob, spawnPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !canSpawn)
            return;
        canSpawn = false;
        foreach(var point in points)
        {
            StartCoroutine(SpawnMonsters(point.mobCount, point.gameObject, point.Mob));
        }
    }
}
