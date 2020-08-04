using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NineHundredLbs.Controly.UI
{
    /// <summary>
    /// Interface for properties of controllers of <see cref="ToggleGroup"/> objects.
    /// </summary>
    /// <typeparam name="TToggleProperties">Type of properties for toggles controlled by the toggle group holding these properties.</typeparam>
    public interface IToggleGroupProperties<TToggleProperties>
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
    public abstract class AToggleGroupController<TToggleGroupProperties, TToggleController, TToggleProperties> : AEntityController<TToggleGroupProperties>
        where TToggleGroupProperties : IToggleGroupProperties<TToggleProperties>
        where TToggleController : AToggleController<TToggleProperties>
    {
        #region Properties
        /// <summary>
        /// Invoked when a controlled <see cref="TToggleController"/> is 
        /// </summary>
        public Action<TToggleController, bool> ValueChanged { get; set; }

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
        [Tooltip("Container where toggles are populated.")]
        [SerializeField] private RectTransform toggleContainer = default;

        [Tooltip("Controlled ToggleGroup component.")]
        [SerializeField] private ToggleGroup toggleGroup = default;

        [Tooltip("Prefab of controlled toggle.")]
        [SerializeField] private TToggleController togglePrefab = default;

        [Tooltip("Controlled toggles.")]
        [SerializeField] private List<TToggleController> toggles = default;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            foreach (var toggle in Toggles)
                toggle.ValueChanged += (x, value) => Toggle_ValueChanged(toggle, value);
        }
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            foreach (var toggle in Toggles)
                Destroy(toggle.UIToggle.gameObject);
            Toggles.Clear();

            foreach (var toggleProperties in Properties.GetToggleProperties())
            {
                var toggle = Instantiate(togglePrefab.UIToggle.gameObject, toggleContainer).GetComponent<TToggleController>();
                toggle.UIToggle.transform.SetParent(toggleContainer, false);
                toggle.UIToggle.Toggle.group = toggleGroup;
                toggle.ValueChanged += (x, value) => Toggle_ValueChanged(toggle, value);
                toggle.SetProperties(toggleProperties);
                InitializeToggle(toggle);
                Toggles.Add(toggle);
            }
        }

        /// <summary>
        /// Initializes the given <paramref name="toggle"/>.
        /// </summary>
        /// <param name="toggle">The toggle to initialize.</param>
        protected abstract void InitializeToggle(TToggleController toggle);

        /// <summary>
        /// Handler method for when a controlled toggle's value is changed.
        /// </summary>
        /// <param name="toggle">The toggle whose value was changed.</param>
        /// <param name="value">The value changed to.</param>
        protected virtual void Toggle_ValueChanged(TToggleController toggle, bool value)
        {
            ValueChanged?.Invoke(toggle, value);
        }
        #endregion
    }
}
