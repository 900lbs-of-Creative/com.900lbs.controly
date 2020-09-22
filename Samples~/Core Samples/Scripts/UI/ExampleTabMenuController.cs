using System.Collections.Generic;
using UnityEngine;

namespace NineHundredLbs.Controly.UI.Examples
{
    public class ExampleTabMenuProperties : ITabMenuProperties
    {
        public List<ITabPageProperties> TabPageProperties { get; }
        public ExampleTabMenuProperties(List<ITabPageProperties> tabPageProperties)
        {
            TabPageProperties = tabPageProperties;
        }
    }

    public class ExampleTabMenuController : ATabMenuController<ExampleTabMenuProperties>
    {
        [SerializeField] private ExampleTabController exampleTabPrefab = null;
        [SerializeField] private BooleanTabPageController booleanTabPagePrefab = null;
        [SerializeField] private StringTabPageController stringTabPagePrefab = null;
        [SerializeField] private IntegerTabPageController integerTabPagePrefab = null;

        public void SetRandomProperties()
        {
            var tabPageProperties = new List<ITabPageProperties>();
            int randomPageCount = Random.Range(2, 5);
            for (int i = 0; i < randomPageCount; i++)
            {
                float randomValue = Random.value;
                if (randomValue > 0.0f && randomValue <= 0.33f)
                    tabPageProperties.Add(new StringTabPageProperties("Hello"));
                else if (randomValue > 0.33f && randomValue <= 0.66f)
                    tabPageProperties.Add(new BooleanTabPageProperties(Random.value > 0.5f));
                else if (randomValue > 0.66f && randomValue <= 1.0f)
                    tabPageProperties.Add(new IntegerTabPageProperties(Random.Range(0, 11)));
            }
            SetProperties(new ExampleTabMenuProperties(tabPageProperties));
        }

        protected override IToggleController GetTab(object tabProperties)
        {
            if (tabProperties is ExampleTabProperties exampleTabProperties)
            {
                ExampleTabController exampleTab = InstantiateTab(exampleTabPrefab) as ExampleTabController;
                exampleTab.SetProperties(exampleTabProperties);
                exampleTab.name = $"[ExampleTab] {exampleTabProperties.label}";
                return exampleTab;
            }
            else
                return null;
        }

        protected override IViewController GetTabPage(ITabPageProperties tabPageProperties)
        {
            if (tabPageProperties is BooleanTabPageProperties booleanTabPageProperties)
            {
                BooleanTabPageController booleanTabPage = InstantiateTabPage(booleanTabPagePrefab) as BooleanTabPageController;
                booleanTabPage.SetProperties(booleanTabPageProperties);
                booleanTabPage.name = $"[BooleanTabPage] {booleanTabPageProperties.value}";
                booleanTabPage.UIView.ViewName = $"[BooleanTabPage] {booleanTabPageProperties.value}";
                return booleanTabPage;
            }
            else if (tabPageProperties is StringTabPageProperties stringTabPageProperties)
            {
                StringTabPageController stringTabPage = InstantiateTabPage(stringTabPagePrefab) as StringTabPageController;
                stringTabPage.SetProperties(stringTabPageProperties);
                stringTabPage.name = $"[StringTabPage] {stringTabPageProperties.value}";
                stringTabPage.UIView.ViewName = $"[StringTabPage] {stringTabPageProperties.value}";
                return stringTabPage;
            }
            else if (tabPageProperties is IntegerTabPageProperties integerTabPageProperties)
            {
                IntegerTabPageController integerTabPage = InstantiateTabPage(integerTabPagePrefab) as IntegerTabPageController;
                integerTabPage.SetProperties(integerTabPageProperties);
                integerTabPage.name = $"[IntegerTabPage] {integerTabPageProperties.value}";
                integerTabPage.UIView.ViewName = $"[IntegerTabPage] {integerTabPageProperties.value}";
                return integerTabPage;
            }
            else
                return null;
        }
    }
}
