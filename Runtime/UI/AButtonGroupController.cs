using System;
using System.Collections.Generic;
using UnityEngine;

namespace NineHundredLbs.Controly.UI
{
    /// <summary>
    /// Interface for properties of controllers of button group objects.
    /// </summary>
    /// <typeparam name="TButtonProperties">Type of properties for buttons controlled by the button group holding these properties.</typeparam>
    public interface IButtonGroupProperties<TButtonProperties>
    {
        /// <summary>
        /// Gets and returns a list of properties for buttons controlled
        /// by the button group holding these properties.
        /// </summary>
        /// <returns>List of button properties.</returns>
        List<TButtonProperties> GetButtonProperties();
    }

    /// <summary>
    /// Base implementation for a controller of a group of buttons with the given properties of type <typeparamref name="TButtonGroupProperties"/>,
    /// buttons of type <typeparamref name="TButtonController"/>, and button properties of type <typeparamref name="TButtonProperties"/>.
    /// </summary>
    /// <typeparam name="TButtonGroupProperties">Type of properties of this controller.</typeparam>
    /// <typeparam name="TButtonController">Type of controlled buttons.</typeparam>
    /// <typeparam name="TButtonProperties">Type of properties of controlled buttons.</typeparam>
    public abstract class AButtonGroupController<TButtonGroupProperties, TButtonController, TButtonProperties> : AEntityController<TButtonGroupProperties>
        where TButtonGroupProperties : IButtonGroupProperties<TButtonProperties>
        where TButtonController : AButtonController<TButtonProperties>
    {
        #region Properties
        /// <summary>
        /// Invoked when a controlled <see cref="TButtonController"/> is clicked.
        /// </summary>
        public Action<TButtonController> ButtonClicked { get; set; }

        /// <summary>
        /// Controlled button components.
        /// </summary>
        public List<TButtonController> Buttons => buttons;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Container where buttons are populated.")]
        [SerializeField] private RectTransform buttonContainer = default;

        [Tooltip("Prefab of controlled button.")]
        [SerializeField] private TButtonController buttonPrefab = default;

        [Tooltip("Controlled buttons.")]
        [SerializeField] private List<TButtonController> buttons = default;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            foreach (var button in Buttons)
                button.Clicked += (x) => Button_Clicked(button);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Toggles the interactability of controlled <see cref="Buttons"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        public void ToggleInteractability(bool value)
        {
            foreach (var button in Buttons)
                button.ToggleInteractability(value);
        }
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            foreach (var button in Buttons)
                Destroy(button.UIButton.gameObject);
            Buttons.Clear();

            foreach (var buttonProperties in Properties.GetButtonProperties())
            {
                var button = Instantiate(buttonPrefab.UIButton.gameObject, buttonContainer).GetComponent<TButtonController>();
                button.UIButton.transform.SetParent(buttonContainer, false);
                button.Clicked += (x) => Button_Clicked(button);
                button.SetProperties(buttonProperties);
                InitializeButton(button);
                Buttons.Add(button); 
            }
        }

        /// <summary>
        /// Gets and returns a button based on the given <paramref name="button"/>.
        /// </summary>
        /// <param name="button">Properties of the button to get.</param>
        /// <returns>A button object.</returns>
        protected abstract void InitializeButton(TButtonController button);

        /// <summary>
        /// Handler method for when a controlled button is clicked.
        /// </summary>
        /// <param name="button">The clicked button.</param>
        protected virtual void Button_Clicked(TButtonController button)
        {
            ButtonClicked?.Invoke(button);
        }
        #endregion
    }
}
