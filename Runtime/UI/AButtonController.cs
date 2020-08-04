using System;
using UnityEngine;

using Doozy.Engine.UI;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
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
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIButton"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class AButtonController : MonoBehaviour, IButtonController
    {
        #region Properties
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
        /// Toggles the interactability of <see cref="UIButton"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        public void ToggleInteractability(bool value)
        {
            UIButton.Interactable = value;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handler method for adding listeners.
        /// </summary>
        protected virtual void AddListeners()
        {
            UIButton.OnClick.OnTrigger.Event.AddListener(OnClick);
        }

        /// <summary>
        /// Handler method for removing listeners.
        /// </summary>
        protected virtual void RemoveListeners()
        {
            UIButton.OnClick.OnTrigger.Event.RemoveListener(OnClick);
        }

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

    /// <summary>
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIButton"/> with the given properties of type <typeparamref name="TButtonProperties"/>.
    /// </summary>
    /// <typeparam name="TButtonProperties">Type of properties for this controller.</typeparam>
    [DisallowMultipleComponent]
    public abstract class AButtonController<TButtonProperties> : AEntityController<TButtonProperties>, IButtonController
    {
        #region Properties
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
        /// Toggles the interactability of <see cref="UIButton"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        public void ToggleInteractability(bool value)
        {
            UIButton.Interactable = value;
        }
        #endregion

        #region Protected Methods
        protected override void AddListeners()
        {
            base.AddListeners();
            UIButton.OnClick.OnTrigger.Event.AddListener(OnClick);
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            UIButton.OnClick.OnTrigger.Event.RemoveListener(OnClick);
        }

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