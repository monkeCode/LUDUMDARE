using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ReactorScripts
{
    public class Item : MonoBehaviour, IItem
    {
        public string itemName;
        public TypeItem type;
        public int repair;
        
        public string Name { get => itemName; set => itemName = value; }
        public TypeItem Type { get => type; set => type = value; }
        public int Repair { get => repair; set => repair = value; }
    }
}