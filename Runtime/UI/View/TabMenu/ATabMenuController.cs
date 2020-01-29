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
        private List<dynamic> tabs = new List<dynamic>();

        /// <summary>
        /// List of controlled tab pages.
        /// </summary>
        private List<dynamic> tabPages = new List<dynamic>();
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            foreach (var tabPage in tabPages)
                Destroy(tabPage.gameObject);
            tabPages.Clear();

            foreach (var tab in tabs)
                Destroy(tab.gameObject);
            tabs.Clear();

            foreach (var tabPageProperties in Properties.TabPageProperties)
            {
                var tabPage = GetTabPage(tabPageProperties);
                if (tabPage == null)
                    throw new System.Exception($"No supported tab page implemented for the given tab properties of type {tabPageProperties.GetType()}");

                var tab = GetTab(tabPageProperties.TabProperties);
                if (tab == null)
                    throw new System.Exception($"No supported tab implemented for the given tab properties of type {tabPageProperties.GetType()}");

                IToggleController tabToggle = tab as IToggleController;
                IViewController tabPageView = tabPage as IViewController;
                tabToggle.Toggled += (toggle, value) =>
                {
                    if (value)
                    {
                        tabPageView.UIView.Show();
                        tabPageView.UIView.transform.SetAsLastSibling();
                    }
                    else
                    {
                        tabPageView.UIView.Hide();
                        tabPageView.UIView.transform.SetAsFirstSibling();
                    }
                };
            }

            if (tabs.Count > 0)
            {
                StartCoroutine(IE_Initialize());
                IEnumerator IE_Initialize()
                {
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    tabs[0].UIToggle.IsOn = true;
                }
            }
        }

        /// <summary>
        /// Gets and returns a tab page object based on the given <paramref name="tabProperties"/>.
        /// </summary>
        /// <param name="tabPageProperties">Properties of the tab page to get.</param>
        /// <returns>A tab page object.</returns>
        protected abstract dynamic GetTabPage(ITabPageProperties tabPageProperties);

        /// <summary>
        /// Gets and returns a tab object based on the given <paramref name="tabProperties"/>.
        /// </summary>
        /// <param name="tabProperties">Properties of the tab to get.</param>
        /// <returns>A tab object.</returns>
        protected abstract dynamic GetTab(ITabProperties tabProperties);

        /// <summary>
        /// Spawns and returns a tab page object from the given <paramref name="tabPagePrefab"/>.
        /// </summary>
        /// <param name="tabPagePrefab">The prefab of the tab page to spawn.</param>
        /// <returns>A instance of the <paramref name="tabPagePrefab"/>.</returns>
        protected dynamic SpawnTabPage(dynamic tabPagePrefab)
        {
            dynamic tabPage = Instantiate(tabPagePrefab, tabPageContainer);
            tabPages.Add(tabPage);
            return tabPage;
        }

        /// <summary>
        /// Spawns and returns a tab object from the given <paramref name="tabPrefab"/>.
        /// </summary>
        /// <param name="tabPrefab">The prefab of the tab  to spawn.</param>
        /// <returns>A instance of the <paramref name="tabPrefab"/>.</returns>
        protected dynamic SpawnTab(dynamic tabPrefab)
        {
            dynamic tab = Instantiate(tabPrefab, tabContainer);
            tab.UIToggle.Toggle.group = tabToggleGroup;
            tabs.Add(tab);
            return tab;
        }
        #endregion
    }
    #endregion
}
