using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class ItemSO : ScriptableObject
    {
        public int ID => GetInstanceID();

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea(2, 5)]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite Icon;
    }
}