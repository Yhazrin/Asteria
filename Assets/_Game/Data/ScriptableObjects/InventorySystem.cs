using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Simple inventory system for expedition items.
    /// Tracks collected seeds, photos, and souvenirs.
    /// </summary>
    public sealed class InventorySystem : MonoBehaviour
    {
        static InventorySystem _instance;

        [Header("Settings")]
        [SerializeField] int maxSlots = 20;

        readonly List<InventoryItem> _items = new();

        public static InventorySystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<InventorySystem>();
                    if (_instance == null)
                    {
                        var go = new GameObject("InventorySystem");
                        _instance = go.AddComponent<InventorySystem>();
                    }
                }
                return _instance;
            }
        }

        // Events
        public event Action<InventoryItem> OnItemAdded;
        public event Action<InventoryItem> OnItemRemoved;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Add an item to inventory.
        /// </summary>
        public bool AddItem(InventoryItem item)
        {
            if (item == null) return false;
            if (_items.Count >= maxSlots) return false;

            // Stack if stackable
            if (item.isStackable)
            {
                var existing = _items.Find(i => i.itemId == item.itemId);
                if (existing != null)
                {
                    existing.quantity += item.quantity;
                    OnItemAdded?.Invoke(existing);
                    return true;
                }
            }

            _items.Add(item);
            OnItemAdded?.Invoke(item);
            Debug.Log($"[Inventory] Added: {item.displayName} x{item.quantity}");
            return true;
        }

        /// <summary>
        /// Remove an item from inventory.
        /// </summary>
        public bool RemoveItem(string itemId, int quantity = 1)
        {
            var item = _items.Find(i => i.itemId == itemId);
            if (item == null) return false;

            if (item.isStackable)
            {
                item.quantity -= quantity;
                if (item.quantity <= 0)
                {
                    _items.Remove(item);
                }
            }
            else
            {
                _items.Remove(item);
            }

            OnItemRemoved?.Invoke(item);
            return true;
        }

        /// <summary>
        /// Check if inventory has an item.
        /// </summary>
        public bool HasItem(string itemId, int quantity = 1)
        {
            var item = _items.Find(i => i.itemId == itemId);
            return item != null && item.quantity >= quantity;
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        public IReadOnlyList<InventoryItem> GetItems()
        {
            return _items.AsReadOnly();
        }

        /// <summary>
        /// Get items by category.
        /// </summary>
        public List<InventoryItem> GetItemsByCategory(string category)
        {
            return _items.FindAll(i => i.category == category);
        }

        /// <summary>
        /// Get item count.
        /// </summary>
        public int GetItemCount()
        {
            return _items.Count;
        }

        /// <summary>
        /// Clear inventory.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }
    }

    [Serializable]
    public class InventoryItem
    {
        public string itemId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string category; // "seed", "photo", "souvenir", "material"
        public int quantity = 1;
        public bool isStackable = true;
        public int maxStack = 99;
        public Sprite icon;
        public Color tintColor = Color.white;
    }
}
