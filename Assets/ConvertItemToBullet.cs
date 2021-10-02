using System.Collections.Generic;
using ReactorScripts;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Convertor", menuName = "Convertor", order = 0)]
    public class ConvertItemToBullet : ScriptableObject
    {
        public Bullet pistolBullet;
        public Bullet laserBullet;
        public Bullet grenadeLauncherBullet;

        public Dictionary<TypeItem, Bullet> GetBulletDictionary()
        {
            var dic = new Dictionary<TypeItem, Bullet>();
            dic[TypeItem.Item1] = laserBullet;
            dic[TypeItem.Item0] = pistolBullet;
            dic[TypeItem.Item2] = grenadeLauncherBullet;
            return dic;
        }
    }
}