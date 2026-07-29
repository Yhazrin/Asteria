using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Inventory panel UI showing collected items.
    /// Displays seeds, photos, souvenirs, and materials.
    /// </summary>
    public sealed class InventoryPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] KeyCode toggleKey = KeyCode.I;
        [SerializeField] int columns = 4;
        [SerializeField] float slotSize = 80f;
        [SerializeField] float slotSpacing = 10f;

        GameObject _panelRoot;
        Transform _gridContainer;
        Text _titleText;
        Text _itemCountText;
        Text _itemDescriptionText;
        Image _itemPreview;
        CanvasGroup _canvasGroup;

        readonly List<InventorySlot> _slots = new();
        bool _isVisible;
        int _selectedSlot = -1;

        void Start()
        {
            BuildUI();
            Hide();
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            _panelRoot = new GameObject("InventoryPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(600, 500);

            _canvasGroup = _panelRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.08f, 0.1f, 0.15f, 0.95f);

            // Title
            _titleText = CreateText(_panelRoot.transform, "Title", "背包",
                new Vector2(0, 220), new Vector2(500, 40), 28, TextAnchor.MiddleCenter,
                new Color(0.95f, 0.85f, 0.4f));

            // Item count
            _itemCountText = CreateText(_panelRoot.transform, "ItemCount", "0/20",
                new Vector2(200, 220), new Vector2(100, 30), 16, TextAnchor.MiddleRight,
                new Color(0.6f, 0.6f, 0.7f));

            // Grid container
            var gridGo = new GameObject("GridContainer");
            gridGo.transform.SetParent(_panelRoot.transform, false);
            var gridRect = gridGo.AddComponent<RectTransform>();
            gridRect.anchorMin = new Vector2(0, 0.3f);
            gridRect.anchorMax = new Vector2(1, 0.9f);
            gridRect.offsetMin = new Vector2(20, 10);
            gridRect.offsetMax = new Vector2(-20, -10);
            _gridContainer = gridGo.transform;

            // Create slots
            CreateSlots();

            // Item description
            _itemDescriptionText = CreateText(_panelRoot.transform, "Description", "选择一个物品查看详情",
                new Vector2(0, -150), new Vector2(560, 60), 16, TextAnchor.UpperLeft,
                new Color(0.7f, 0.7f, 0.75f));

            // Item preview
            var previewGo = new GameObject("ItemPreview");
            previewGo.transform.SetParent(_panelRoot.transform, false);
            var previewRect = previewGo.AddComponent<RectTransform>();
            previewRect.anchorMin = new Vector2(0, 0);
            previewRect.anchorMax = new Vector2(0.3f, 0.3f);
            previewRect.offsetMin = new Vector2(20, 20);
            previewRect.offsetMax = new Vector2(-10, -10);
            _itemPreview = previewGo.AddComponent<Image>();
            _itemPreview.color = new Color(0.2f, 0.25f, 0.3f);

            // Close button
            var closeBtn = CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(200, -220), new Vector2(100, 35), Hide,
                new Color(0.4f, 0.4f, 0.5f));
        }

        void CreateSlots()
        {
            for (int i = 0; i < 20; i++)
            {
                int row = i / columns;
                int col = i % columns;

                float x = (col - (columns - 1) * 0.5f) * (slotSize + slotSpacing);
                float y = -row * (slotSize + slotSpacing);

                var slot = new InventorySlot();
                slot.index = i;

                var slotGo = new GameObject($"Slot_{i}");
                slotGo.transform.SetParent(_gridContainer, false);

                var slotRect = slotGo.AddComponent<RectTransform>();
                slotRect.anchorMin = slotRect.anchorMax = new Vector2(0.5f, 0.5f);
                slotRect.pivot = new Vector2(0.5f, 0.5f);
                slotRect.anchoredPosition = new Vector2(x, y);
                slotRect.sizeDelta = new Vector2(slotSize, slotSize);

                var slotBg = slotGo.AddComponent<Image>();
                slotBg.color = new Color(0.15f, 0.18f, 0.25f);

                var button = slotGo.AddComponent<Button>();
                button.targetGraphic = slotBg;
                int slotIndex = i;
                button.onClick.AddListener(() => OnSlotClicked(slotIndex));

                // Item icon placeholder
                var iconGo = new GameObject("Icon");
                iconGo.transform.SetParent(slotGo.transform, false);
                var iconRect = iconGo.AddComponent<RectTransform>();
                iconRect.anchorMin = Vector2.zero;
                iconRect.anchorMax = Vector2.one;
                iconRect.offsetMin = new Vector2(5, 5);
                iconRect.offsetMax = new Vector2(-5, -5);
                slot.icon = iconGo.AddComponent<Image>();
                slot.icon.color = Color.clear;

                // Quantity text
                var qtyGo = new GameObject("Quantity");
                qtyGo.transform.SetParent(slotGo.transform, false);
                var qtyRect = qtyGo.AddComponent<RectTransform>();
                qtyRect.anchorMin = new Vector2(1, 0);
                qtyRect.anchorMax = new Vector2(1, 0);
                qtyRect.pivot = new Vector2(1, 0);
                qtyRect.anchoredPosition = new Vector2(-2, 2);
                qtyRect.sizeDelta = new Vector2(30, 20);
                slot.quantityText = qtyGo.AddComponent<Text>();
                slot.quantityText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                slot.quantityText.fontSize = 14;
                slot.quantityText.color = Color.white;
                slot.quantityText.alignment = TextAnchor.MiddleRight;

                slot.root = slotGo;
                _slots.Add(slot);
            }
        }

        void OnSlotClicked(int index)
        {
            _selectedSlot = index;
            UpdateItemDetails();
        }

        void UpdateItemDetails()
        {
            var inventory = Data.InventorySystem.Instance;
            if (inventory == null) return;

            var items = inventory.GetItems();
            if (_selectedSlot < 0 || _selectedSlot >= items.Count)
            {
                _itemDescriptionText.text = "选择一个物品查看详情";
                _itemPreview.color = new Color(0.2f, 0.25f, 0.3f);
                return;
            }

            var item = items[_selectedSlot];
            _itemDescriptionText.text = $"【{item.displayName}】\n{item.description}\n数量: {item.quantity}";
        }

        /// <summary>
        /// Refresh the inventory display.
        /// </summary>
        public void Refresh()
        {
            var inventory = Data.InventorySystem.Instance;
            if (inventory == null) return;

            var items = inventory.GetItems();
            _itemCountText.text = $"{inventory.GetItemCount()}/20";

            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];

                if (i < items.Count)
                {
                    var item = items[i];
                    slot.icon.color = item.tintColor;
                    slot.quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
                }
                else
                {
                    slot.icon.color = Color.clear;
                    slot.quantityText.text = "";
                }
            }
        }

        public void Toggle()
        {
            if (_isVisible) Hide();
            else Show();
        }

        public void Show()
        {
            _panelRoot.SetActive(true);
            Refresh();
            StartCoroutine(FadeIn());
            _isVisible = true;
        }

        public void Hide()
        {
            StartCoroutine(FadeOut());
            _isVisible = false;
        }

        System.Collections.IEnumerator FadeIn()
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime * 3f;
                yield return null;
            }
            _canvasGroup.alpha = 1f;
        }

        System.Collections.IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime * 3f;
                yield return null;
            }
            _canvasGroup.alpha = 0f;
            _panelRoot.SetActive(false);
        }

        static Text CreateText(Transform parent, string name, string content,
            Vector2 offset, Vector2 size, int fontSize, TextAnchor alignment, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;
            var text = go.AddComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = alignment;
            return text;
        }

        static Button CreateButton(Transform parent, string name, string label,
            Vector2 offset, Vector2 size, UnityEngine.Events.UnityAction onClick, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;
            var img = go.AddComponent<Image>();
            img.color = color;
            var button = go.AddComponent<Button>();
            button.targetGraphic = img;
            button.onClick.AddListener(onClick);
            var textGo = new GameObject("Text");
            textGo.transform.SetParent(go.transform, false);
            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = textRect.offsetMax = Vector2.zero;
            var text = textGo.AddComponent<Text>();
            text.text = label;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 18;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            return button;
        }

        class InventorySlot
        {
            public int index;
            public GameObject root;
            public Image icon;
            public Text quantityText;
        }
    }
}
