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
        public Bullet flamethrowerBullet;

        public Dictionary<TypeItem, Bullet> GetBulletDictionary()
        {
            var dic = new Dictionary<TypeItem, Bullet>();
            dic[TypeItem.Default] = pistolBullet;
            dic[TypeItem.Laser] = laserBullet;
            dic[TypeItem.Grenade] = grenadeLauncherBullet;
            dic[TypeItem.Flamethrower] = flamethrowerBullet;
            return dic;
        }
    }
}