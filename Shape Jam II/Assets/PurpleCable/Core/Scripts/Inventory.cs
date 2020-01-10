using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace PurpleCable
{
    /// <summary>
    /// Inventory [MonoBehaviour]
    /// </summary>    
    public class Inventory : MonoBehaviour
    {
        #region Instance

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static Inventory _instance;

        #endregion

        #region Data

        /// <summary>
        /// Item definitions (available items)
        /// </summary>
        private static ItemDef[] _itemDefs;

        /// <summary>
        /// Item collection (current inventory)
        /// </summary>
        public static Dictionary<ItemDef, int> Items { get; } = new Dictionary<ItemDef, int>();

        #endregion

        #region Events

        /// <summary>
        /// Triggered when an item is added
        /// </summary>
        public static event Action<ItemDef> ItemAdded;

        /// <summary>
        /// Triggered when an item is removed
        /// </summary>
        public static event Action<ItemDef> ItemRemoved;

        /// <summary>
        /// Triggered when the inventory is cleared
        /// </summary>
        public static event Action ItemsCleared;

        #endregion

        #region Unity callbacks

        private void Start()
        {
            #region Singleton
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);
            #endregion

            #region Item definitions
            _itemDefs = Resources.LoadAll<ItemDef>("Items");

            if (_itemDefs == null || _itemDefs.Length == 0)
                _itemDefs = Resources.LoadAll<Sprite>("Items").Select(x => CreateItemDefFromSprite(x)).ToArray();
            #endregion

            Clear();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an ItemDef from a sprite (using its name and image)
        /// </summary>
        /// <param name="sprite">The sprite</param>
        /// <returns>A new ItemDef with the sprite's name and image</returns>
        private static ItemDef CreateItemDefFromSprite(Sprite sprite)
        {
            var itemDef = ScriptableObject.CreateInstance<ItemDef>();
            itemDef.name = sprite.name;
            itemDef.DisplayImage = sprite;
            itemDef.DisplayName = sprite.name;

            return itemDef;
        }

        /// <summary>
        /// Gets a random item from the available items
        /// </summary>
        /// <returns></returns>
        public static ItemDef GetRandomItem()
            => _itemDefs.GetRandom();

        /// <summary>
        /// Gets an item count
        /// </summary>
        /// <param name="itemDef">The item to count</param>
        /// <returns>The item count</returns>
        public static int GetItemCount(ItemDef itemDef)
        {
            if (Items.TryGetValue(itemDef, out int count))
                return count;

            return 0;
        }

        /// <summary>
        /// Adds an item to the inventory (Triggers ItemAdded)
        /// </summary>
        /// <param name="itemDef">The item to add</param>
        public static void AddItem(ItemDef itemDef)
        {
            if (itemDef == null)
                return;

            if (Items.ContainsKey(itemDef))
                Items[itemDef]++;
            else
                Items.Add(itemDef, 1);

            ItemAdded?.Invoke(itemDef);
        }

        /// <summary>
        /// Removes an item from the inventory (Triggers ItemRemoved)
        /// </summary>
        /// <param name="itemDef">The item to remove</param>
        public static void RemoveItem(ItemDef itemDef)
        {
            if (Items.TryGetValue(itemDef, out int number))
            {
                if (number == 1)
                    Items.Remove(itemDef);
                else
                    Items[itemDef]--;

                ItemRemoved?.Invoke(itemDef);
            }
        }

        /// <summary>
        /// Sets an item count (for deserializing)
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="count">The count</param>
        public static void SetItemCount(string itemName, int count)
        {
            ItemDef itemDef = _itemDefs.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase));

            if (itemDef == null)
                return;

            if (count <= 0)
            {
                RemoveItem(itemDef);
                return;
            }

            Items[itemDef] = count;

            ItemAdded?.Invoke(itemDef);
        }

        /// <summary>
        /// Clears the inventory (Triggers ItemsCleared)
        /// </summary>
        public static void Clear()
        {
            Items.Clear();

            ItemsCleared?.Invoke();
        }

        /// <summary>
        /// Deserializes the data from an xml element
        /// </summary>
        /// <param name="element">The xml element</param>
        public static void FromXElement(XElement element)
        {
            if (element == null)
                return;

            Clear();

            var xItemList = element.Elements();

            if (xItemList != null)
            {
                foreach (var xItem in xItemList)
                {
                    SetItemCount(xItem.Name.LocalName, (int)xItem);
                }
            }
        }

        /// <summary>
        /// Serialized the data into an xml element
        /// </summary>
        /// <param name="name">The name of the element</param>
        /// <returns>Data serialized into an xml element</returns>
        public static XElement ToXElement(string name)
        {
            return new XElement(name,
                Items.Select(x => new XElement(x.Key.Name, x.Value)));
        }

        #endregion
    }
}
