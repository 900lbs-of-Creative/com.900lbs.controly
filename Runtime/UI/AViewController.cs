using System;
using UnityEngine;

using Doozy.Engine.UI;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for controllers of <see cref="Doozy.Engine.UI.UIView"/> objects.
    /// </summary>
    public interface IViewController
    {
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIView"/> component.
        /// </summary>
        UIView UIView { get; }

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.ShowBehavior"/> starts.
        /// </summary>
        Action<IViewController> ShowStarted { get; set; }

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.ShowBehavior"/> finishes.
        /// </summary>
        Action<IViewController> ShowFinished { get; set; }

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.HideBehavior"/> starts.
        /// </summary>
        Action<IViewController> HideStarted { get; set; }

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.HideBehavior"/> finishes.
        /// </summary>
        Action<IViewController> HideFinished { get; set; }
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIView"/> with the given properties of type <typeparamref name="TViewProperties"/>.
    /// </summary>
    /// <typeparam name="TViewProperties">Type of properties for this controller.</typeparam>
    [DisallowMultipleComponent]
    public abstract class AViewController<TViewProperties> : AEntityController<TViewProperties>, IViewController
    {
        #region Properties
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIView"/> component.
        /// </summary>
        public UIView UIView => uiView;

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.ShowBehavior"/> starts.
        /// </summary>
        public Action<IViewController> ShowStarted { get; set; }

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.ShowBehavior"/> finishes.
        /// </summary>
        public Action<IViewController> ShowFinished { get; set; }

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.HideBehavior"/> starts.
        /// </summary>
        public Action<IViewController> HideStarted { get; set; }

        /// <summary>
        /// Invoked when <see cref="UIView"/> component's <see cref="UIView.HideBehavior"/> finishes.
        /// </summary>
        public Action<IViewController> HideFinished { get; set; }
        #endregion

        #region Serialized Private Variables
        [Tooltip("Controlled UIView component.")]
        [SerializeField] private UIView uiView = default;
        #endregion

        #region Protected Methods
        protected override void AddListeners()
        {
            base.AddListeners();
            uiView.ShowBehavior.OnStart.Event.AddListener(OnShowStarted);
            uiView.ShowBehavior.OnFinished.Event.AddListener(OnShowFinished);
            uiView.HideBehavior.OnStart.Event.AddListener(OnHideStarted);
            uiView.HideBehavior.OnFinished.Event.AddListener(OnHideFinished);
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            uiView.ShowBehavior.OnStart.Event.RemoveListener(OnShowStarted);
            uiView.ShowBehavior.OnFinished.Event.RemoveListener(OnShowFinished);
            uiView.HideBehavior.OnStart.Event.RemoveListener(OnHideStarted);
            uiView.HideBehavior.OnFinished.Event.RemoveListener(OnHideFinished);
        }

        /// <summary>
        /// Handler method for when the <see cref="uiView"/> component's <see cref="UIView.ShowBehavior"/> starts.
        /// </summary>
        protected virtual void OnShowStarted()
        {
            ShowStarted?.Invoke(this);
        }

        /// <summary>
        /// Handler method for when the <see cref="uiView"/> component's <see cref="UIView.ShowBehavior"/> finishes.
        /// </summary>
        protected virtual void OnShowFinished()
        {
            ShowFinished?.Invoke(this);
        }

        /// <summary>
        /// Handler method for when the <see cref="uiView"/> component's <see cref="UIView.HideBehavior"/> starts.
        /// </summary>
        protected virtual void OnHideStarted()
        {
            HideStarted?.Invoke(this);
        }

        /// <summary>
        /// Handler method for when the <see cref="uiView"/> component's <see cref="UIView.HideBehavior"/> finishes.
        /// </summary>
        protected virtual void OnHideFinished()
        {
            HideFinished?.Invoke(this);
        }
        #endregion
    }
    #endregion
}
