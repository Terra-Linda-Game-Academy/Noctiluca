using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potions {
    [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory", order = 0)]
    public class Inventory : ScriptableObject {
        [NonSerialized] private List<Potion> potions     = new List<Potion>();
        [NonSerialized] private int          activeIndex = 0;

        public Potion Current => potions[activeIndex];
        public bool IsEmpty => potions.Count <= 0;

        public Potion this[int offset] {
            get {
                int newIndex = (activeIndex + offset) % potions.Count;
                newIndex = newIndex >= 0 ? newIndex : potions.Count + newIndex;
                return potions[newIndex];
            }
        }

        public void Add(Potion potion) {
            potions.Insert(activeIndex++, potion);
        }

        public void Remove() {
            potions.RemoveAt(activeIndex);
            if (activeIndex >= potions.Count) 
                activeIndex = potions.Count - 1;
        }

        public void SelectNext() {
            if (activeIndex == potions.Count - 1) {
                activeIndex = 0;
            } else { 
                activeIndex++;
            }
        }

        public void SelectPrevious() {
            if (activeIndex == 0) {
                activeIndex = potions.Count - 1;
            } else {
                activeIndex--;
            }
        }
    }
}