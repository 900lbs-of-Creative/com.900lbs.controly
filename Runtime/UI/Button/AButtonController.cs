using System;
using UnityEngine;

using Doozy.Engine.UI;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of controllers of <see cref="Doozy.Engine.UI.UIButton"/> objects.
    /// </summary>
    public interface IButtonProperties : IEntityProperties { }

    /// <summary>
    /// Interface that controllers of <see cref="Doozy.Engine.UI.UIButton"/> objects must implement.
    /// </summary>
    public interface IButtonController
    {
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIButton"/> component.
        /// </summary>
        UIButton UIButton { get; }

        /// <summary>
        /// Invoked when <see cref="UIButton"/> component's <see cref="UIButton.OnClick"/> occurs.
        /// </summary>
        Action<IButtonController> Clicked { get; set; }

        /// <summary>
        /// Toggles the interactability of <see cref="UIButton"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        void ToggleInteractability(bool value);
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of a <see cref="UIButton"/> with default properties of type <see cref="IButtonProperties"/>.
    /// </summary>
    public abstract class AButtonController : AButtonController<IButtonProperties> { }

    /// <summary>
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIButton"/> with the given properties of type <typeparamref name="TButtonProperties"/>.
    /// </summary>
    /// <typeparam name="TButtonProperties">Type of properties for this controller.</typeparam>
    [DisallowMultipleComponent]
    public abstract class AButtonController<TButtonProperties> : MonoBehaviour , IEntityController<TButtonProperties>, IButtonController
        where TButtonProperties : IButtonProperties
    {
        #region Properties
        /// <summary>
        /// Properties of this button controller.
        /// </summary>
        public TButtonProperties Properties => properties;

        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIButton"/> component.
        /// </summary>
        public UIButton UIButton => uiButton;

        /// <summary>
        /// Invoked when <see cref="UIButton"/> component's <see cref="UIButton.OnClick"/> occurs.
        /// </summary>
        public Action<IButtonController> Clicked { get; set; }
        #endregion

        #region Serialized Private Variables
        [Tooltip("Properties of this button controller")]
        [SerializeField] private TButtonProperties properties = default;

        [Tooltip("Controlled UIButton component.")]
        [SerializeField] private UIButton uiButton = default;
        #endregion

        #region Private Variables
        /// <summary>
        /// Used to internally track changes in interactability on <see cref="UIButton"/> component
        /// and subsequently send callbacks through <see cref="OnInteractabilityChanged(bool)"/>.
        /// </summary>
        private bool m_previousInteractable = true;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            m_previousInteractable = true;
        }

        protected virtual void OnEnable()
        {
            AddListeners();
        }

        protected virtual void OnDisable()
        {
            RemoveListeners();
        }

        protected virtual void Update()
        {
            if (m_previousInteractable != UIButton.Interactable)
            {
                m_previousInteractable = UIButton.Interactable;
                OnInteractabilityChanged(UIButton.Interactable);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">Properties to initialize with.</param>
        public void SetProperties(TButtonProperties properties)
        {
            this.properties = properties;
            OnPropertiesSet();
        }

        /// <summary>
        /// Toggles the interactability of <see cref="UIButton"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        public void ToggleInteractability(bool value)
        {
            UIButton.Interactable = value;
        }
        #endregion

        #region Protected Methods
        protected virtual void AddListeners()
        {
            UIButton.OnClick.OnTrigger.Event.AddListener(OnClick);
        }

        protected virtual void RemoveListeners()
        {
            UIButton.OnClick.OnTrigger.Event.RemoveListener(OnClick);
        }

        /// <summary>
        /// Handler method for when <see cref="properties"/> have been set.
        /// At this point, <see cref="Properties"/> can now be safely accessed.
        /// </summary>
        protected virtual void OnPropertiesSet() { }

        /// <summary>
        /// Handler method for backend functionality when <see cref="UIButton"/> component's <see cref="UIButton.OnClick"/> occurs.
        /// </summary>
        protected virtual void HandleOnClick()
        {
            Clicked?.Invoke(this);
        }

        /// <summary>
        /// Handler method for frontend functionality when <see cref="UIButton"/> component's <see cref="UIButton.OnClick"/> occurs.
        /// </summary>
        protected virtual void AnimateOnClick() { }

        /// <summary>
        /// Handler method for backend functionality when <see cref="UIButton"/> component's <see cref="UIButton.Interactable"/> value
        /// changes.
        /// </summary>
        /// <param name="value">Whether interactability was changed to interactable (true) or uninteractable (false).</param>
        protected virtual void HandleInteractabilityChanged(bool value) { }

        /// <summary>
        /// Handler method for frontend functionality when <see cref="UIButton"/> component's <see cref="UIButton.Interactable"/> value
        /// changes.
        /// </summary>
        /// <param name="value">Whether interactability was changed to interactable (true) or uninteractable (false).</param>
        protected virtual void AnimateInteractabilityChanged(bool value) { }
        #endregion

        #region Private Methods
        /// <summary>
        /// Handler method for when <see cref="UIButton"/> component's <see cref="UIButton.OnClick"/> occurs.
        /// Delegates functionality for backend and frontend behaviour to <see cref="HandleOnClick"/>
        /// and <see cref="AnimateOnClick"/>, respectively.
        /// </summary>
        private void OnClick()
        {
            HandleOnClick();
            AnimateOnClick();
        }

        /// <summary>
        /// Handler method for when <see cref="UIButton"/> component's <see cref="UIButton.Interactable"/> value
        /// changes. Delegates functionality for backend and frontend behaviour to <see cref="HandleInteractabilityChanged(bool)"/>
        /// and <see cref="AnimateInteractabilityChanged(bool)"/>, respectively.
        /// </summary>
        /// <param name="value">Whether interactability was changed to interactable (true) or uninteractable (false).</param>
        private void OnInteractabilityChanged(bool value)
        {
            HandleInteractabilityChanged(value);
            AnimateInteractabilityChanged(value);
        }
        #endregion
    }
    #endregion
}