using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NineHundredLbs.Controly.UI
{
    /// <summary>
    /// Interface for properties of controllers of <see cref="ToggleGroup"/> objects.
    /// </summary>
    /// <typeparam name="TToggleProperties">Type of properties for toggles controlled by the toggle group holding these properties.</typeparam>
    public interface IToggleGroupProperties<TToggleProperties> : IViewProperties where TToggleProperties : IToggleProperties
    {
        /// <summary>
        /// Gets and returns a list of properties for toggles controlled
        /// by the toggle group holding these properties.
        /// </summary>
        /// <returns>List of toggle properties.</returns>
        List<TToggleProperties> GetToggleProperties();
    }

    /// <summary>
    /// Base implementation for a controller of a <see cref="UnityEngine.UI.ToggleGroup"/> with the given properties of type <typeparamref name="TToggleGroupProperties"/>,
    /// toggles of type <typeparamref name="TToggleController"/>, and toggle properties of type <typeparamref name="TToggleProperties"/>
    /// </summary>
    /// <typeparam name="TToggleGroupProperties">Type of properties for this controller.</typeparam>
    /// <typeparam name="TToggleController">Type of controlled toggles.</typeparam>
    /// <typeparam name="TToggleProperties">Type of properties of controlled toggles.</typeparam>
    public abstract class AToggleGroupController<TToggleGroupProperties, TToggleController, TToggleProperties> : AViewController<TToggleGroupProperties>
        where TToggleGroupProperties : IToggleGroupProperties<TToggleProperties>
        where TToggleController : AToggleController<TToggleProperties>
        where TToggleProperties : IToggleProperties
    {
        #region Properties
        /// <summary>
        /// Controlled <see cref="UnityEngine.UI.ToggleGroup"/> component.
        /// </summary>
        public ToggleGroup ToggleGroup => toggleGroup;

        /// <summary>
        /// Controlled toggle components.
        /// </summary>
        public List<TToggleController> Toggles => toggles;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Container where toggles are held and populated.")]
        [SerializeField] private RectTransform toggleContainer = null;

        [Tooltip("Controlled ToggleGroup component.")]
        [SerializeField] private ToggleGroup toggleGroup = null;

        [Tooltip("Prefab of controlled toggle.")]
        [SerializeField] private GameObject togglePrefab = null;

        [Tooltip("Controlled toggles.")]
        [SerializeField] private List<TToggleController> toggles = new List<TToggleController>();
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            foreach (TToggleController toggle in Toggles)
                toggle.Toggled += (x, y) => Toggle_Toggled(toggle, y);
        }
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            if (toggles.Count != 0)
                ClearToggles();
            PopulateToggles();
        }

        /// <summary>
        /// Handler method for populating controlled toggles.
        /// </summary>
        protected virtual void PopulateToggles()
        {
            foreach (TToggleProperties toggleProperties in Properties.GetToggleProperties())
            {
                TToggleController toggle = Instantiate(togglePrefab).GetComponent<TToggleController>();
                toggle.UIToggle.transform.SetParent(toggleContainer, false);
                toggle.UIToggle.Toggle.group = toggleGroup;
                toggle.Toggled += (x, y) => Toggle_Toggled(toggle, y);
                toggle.SetProperties(toggleProperties);
                toggles.Add(toggle);
            }
        }

        /// <summary>
        /// Handler method for clearing controlled toggles.
        /// </summary>
        protected virtual void ClearToggles()
        {
            foreach (TToggleController toggle in toggles)
            {
                toggle.Toggled -= (x, y) => Toggle_Toggled(toggle, y);
                Destroy(toggle.UIToggle.gameObject);
            }
            toggles.Clear();
        }

        /// <summary>
        /// Handler method for when a controlled toggle's value is changed.
        /// </summary>
        /// <param name="toggle">The toggled toggle.</param>
        /// <param name="value">The value toggled to.</param>
        protected abstract void Toggle_Toggled(TToggleController toggle, bool value);
        #endregion
    }
}
