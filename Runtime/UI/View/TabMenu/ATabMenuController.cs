using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of controllers of tab menu objects.
    /// </summary>
    public interface ITabMenuProperties : IViewProperties
    {
        List<ITabPageProperties> TabPageProperties { get; }
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of a tab menu with the given properties of type <typeparamref name="TTabMenuProperties"/>.
    /// </summary>
    /// <typeparam name="TTabMenuProperties">Type of properties of this controller.</typeparam>
    public abstract class ATabMenuController<TTabMenuProperties> : AViewController<TTabMenuProperties>
        where TTabMenuProperties : ITabMenuProperties
    {
        #region Serialized Private Variables
        [SerializeField] private RectTransform tabContainer = null;
        [SerializeField] private ToggleGroup tabToggleGroup = null;
        [SerializeField] private RectTransform tabPageContainer = null;
        #endregion

        #region Private Variables
        /// <summary>
        /// List of controlled tabs.
        /// </summary>
        private List<IToggleController> tabs = new List<IToggleController>();

        /// <summary>
        /// List of controlled tab pages.
        /// </summary>
        private List<IViewController> tabPages = new List<IViewController>();
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            foreach (var tabPage in tabPages)
                Destroy(tabPage.UIView.gameObject);
            tabPages.Clear();

            foreach (var tab in tabs)
                Destroy(tab.UIToggle.gameObject);
            tabs.Clear();

            foreach (var tabPageProperties in Properties.TabPageProperties)
            {
                var tabPage = GetTabPage(tabPageProperties);
                if (tabPage == null)
                    throw new System.Exception($"No supported tab page implemented for the given tab properties of type {tabPageProperties.GetType()}");

                var tab = GetTab(tabPageProperties.TabProperties);
                if (tab == null)
                    throw new System.Exception($"No supported tab implemented for the given tab properties of type {tabPageProperties.GetType()}");

                tab.Toggled += (toggle, value) =>
                {
                    if (value)
                    {
                        tabPage.UIView.Show();
                        tabPage.UIView.transform.SetAsLastSibling();
                    }
                    else
                    {
                        tabPage.UIView.Hide();
                        tabPage.UIView.transform.SetAsFirstSibling();
                    }
                };
            }

            tabs[0].UIToggle.ToggleOn();
        }

        /// <summary>
        /// Gets and returns a tab page object based on the given <paramref name="tabProperties"/>.
        /// </summary>
        /// <param name="tabPageProperties">Properties of the tab page to get.</param>
        /// <returns>A tab page object.</returns>
        protected abstract IViewController GetTabPage(ITabPageProperties tabPageProperties);

        /// <summary>
        /// Gets and returns a tab object based on the given <paramref name="tabProperties"/>.
        /// </summary>
        /// <param name="tabProperties">Properties of the tab to get.</param>
        /// <returns>A tab object.</returns>
        protected abstract IToggleController GetTab(ITabProperties tabProperties);

        /// <summary>
        /// Spawns and returns a tab page object from the given <paramref name="tabPagePrefab"/>.
        /// </summary>
        /// <param name="tabPagePrefab">The prefab of the tab page to spawn.</param>
        /// <returns>A instance of the <paramref name="tabPagePrefab"/>.</returns>
        protected IViewController SpawnTabPage(GameObject tabPagePrefab)
        {
            IViewController tabPage = Instantiate(tabPagePrefab, tabPageContainer).GetComponent<IViewController>();
            tabPages.Add(tabPage);
            return tabPage;
        }

        /// <summary>
        /// Spawns and returns a tab object from the given <paramref name="tabPrefab"/>.
        /// </summary>
        /// <param name="tabPrefab">The prefab of the tab  to spawn.</param>
        /// <returns>A instance of the <paramref name="tabPrefab"/>.</returns>
        protected IToggleController SpawnTab(GameObject tabPrefab)
        {
            IToggleController tab = Instantiate(tabPrefab, tabContainer).GetComponent<IToggleController>();
            tab.UIToggle.Toggle.group = tabToggleGroup;
            tabs.Add(tab);
            return tab;
        }
        #endregion
    }
    #endregion
}
