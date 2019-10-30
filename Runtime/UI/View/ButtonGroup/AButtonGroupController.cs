using System.Collections.Generic;
using UnityEngine;

namespace NineHundredLbs.Controly.UI
{
    /// <summary>
    /// Base implementation of properties for <see cref="AButtonGroupController{TButtonGroupProperties, TButtonController, TButtonProperties}"/> objects.
    /// </summary>
    /// <typeparam name="TButtonProperties">Type of properties for buttons controlled by the button group holding these properties.</typeparam>
    public abstract class AButtonGroupProperties<TButtonProperties> : AViewProperties 
        where TButtonProperties : AButtonProperties
    {
        /// <summary>
        /// Gets and returns a list of properties for buttons controlled
        /// by the button group holding these properties.
        /// </summary>
        /// <returns>List of button properties.</returns>
        public abstract List<TButtonProperties> GetButtonProperties();
    }

    /// <summary>
    /// Base implementation for a controller of a group of buttons with the given properties of type <typeparamref name="TButtonGroupProperties"/>,
    /// buttons of type <typeparamref name="TButtonController"/>, and button properties of type <typeparamref name="TButtonProperties"/>.
    /// </summary>
    /// <typeparam name="TButtonGroupProperties">Type of properties of this controller.</typeparam>
    /// <typeparam name="TButtonController">Type of controlled buttons.</typeparam>
    /// <typeparam name="TButtonProperties">Type of properties of controlled buttons.</typeparam>
    public abstract class AButtonGroupController<TButtonGroupProperties, TButtonController, TButtonProperties> : AViewController<TButtonGroupProperties>
        where TButtonGroupProperties : AButtonGroupProperties<TButtonProperties>
        where TButtonController : AButtonController<TButtonProperties>
        where TButtonProperties : AButtonProperties
    {
        #region Properties
        /// <summary>
        /// Controlled button components.
        /// </summary>
        public List<TButtonController> Buttons => buttons;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Container where buttons are are held and populated.")]
        [SerializeField] private RectTransform buttonContainer = null;

        [Tooltip("Prefab of controlled button.")]
        [SerializeField] private GameObject buttonPrefab = null;

        [Tooltip("Controlled buttons.")]
        [SerializeField] private List<TButtonController> buttons = new List<TButtonController>();
        #endregion

        #region Unity Methods
        private void Awake()
        {
            foreach (TButtonController button in buttons)
                button.Clicked += (x) => Button_Clicked(button);
        }
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            if (buttons.Count != 0)
                ClearButtons();
            PopulateButtons();
        }

        /// <summary>
        /// Handles method for populating controlled buttons.
        /// </summary>
        protected virtual void PopulateButtons()
        {
            foreach (TButtonProperties buttonProperties in Properties.GetButtonProperties())
            {
                TButtonController button = Instantiate(buttonPrefab).GetComponent<TButtonController>();
                button.UIButton.transform.SetParent(buttonContainer, false);
                button.Clicked += (x) => Button_Clicked(button);
                button.SetProperties(buttonProperties);
                buttons.Add(button);
            }
        }

        /// <summary>
        /// Handler method for clearing controlled buttons.
        /// </summary>
        protected virtual void ClearButtons()
        {
            foreach (TButtonController button in buttons)
            {
                button.Clicked -= (x) => Button_Clicked(button);
                Destroy(button.UIButton.gameObject);
            }
            buttons.Clear();
        }

        /// <summary>
        /// Handler method for when a controlled button is clicked.
        /// </summary>
        /// <param name="button">The clicked button.</param>
        protected abstract void Button_Clicked(TButtonController button);
        #endregion
    }
}
