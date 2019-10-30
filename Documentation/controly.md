# Controly

## Table of Contents

- [Controly](#controly)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Controllers & Properties](#controllers--properties)
  - [Entity Controllers](#entity-controllers)
  - [Button Controllers](#button-controllers)
  - [Toggle Controllers](#toggle-controllers)
  - [View Controllers](#view-controllers)
    - [Toggle Group Controllers](#toggle-group-controllers)
    - [Button Group Controllers](#button-group-controllers)
    - [Scroller Controllers](#scroller-controllers)
    - [Tab Menu Controllers](#tab-menu-controllers)

## Overview

Controly is a collection of abstract classes designed to help manage behaviour on data-driven user interfaces. Built to run parallel with DoozyUI components and serialized fully by the Odin Inspector, the framework allows for dynamic and property-driven entities.

This framework is designed to provide a foundation of back-end behaviours often repeated throughout UI code like data management, callbacks, and internal state tracking all while remaining decoupled from the front-end behaviour of the controlled entity.

## Controllers & Properties

Controly is composed of two primary elements:

- **Controllers**: These are classes that control specific things, like a Toggle, Button, or any arbitrary entity.
- **Properties**: These are classes usually declared alongside the controller they are intended for and describe how the controller will behave or what it will cause its controlled entity to display.

The classes themselves are designed so that custom controllers and properties can inherit from them to allow for any type of desired UI behaviour.

## Entity Controllers

Entity controllers serve as the foundation for the entire framework. At their core, they are specialized MonoBehaviours that hold properties.

The default implementation of an entity controller is the ***AEntityController\<T>*** class, which exposes a few protected methods that subclasses can easily override, listed below:

- ***AddListeners():*** Handler for registering to delegate methods and events. By default called by *OnEnable()*.
- ***RemoveListeners():*** Handler for un-registering to delegate methods and events. Be default called by *OnDisable()*.
- ***OnPropertiesSet():*** Handler for initializing controller based on given properties. By default called when the properties of this entity have been set.
  - **IMPORTANT**: for some controllers like [View Controllers](#view-controllers), it is common that this method may be called before the GameObject is actually turned on, which can prevent Coroutines from executing. It is recommended to avoid starting Coroutines from this method and instead just initialize component values.
  
In addition, ***AEntityController\<T>*** exposes a public method ***SetProperties(IEntityProperties properties)*** which can be called to set the properties of the entity, triggering the ***OnPropertiesSet()*** method in turn.

Because of their high level of abstraction, entity controllers can be declared that inherit from the base ***AEntityController\<T>*** class to control any entity that needs to contain properties, from actual UI elements like [buttons](#button-controllers) and [toggles](#toggle-controllers) to any data-driven system.

For an example, see the ExampleEntityController below:

~~~C#
{
  // Make sure your custom properties class inherits from AEntityProperties.
  // If you want your properties to be serialized and editable, make sure
  // to mark it with the [System.Serializable] attribute.
  [System.Serializable]
  public class ExampleEntityProperties : AEntityProperties
  {
    public int value;
  }

  public class ExampleEntityController : AEntityController<ExampleEntityProperties>
  {
    protected override void AddListeners()
    {
      // Add event listeners here.
      base.AddListeners();
      ExampleEvent.AddListener(ExampleEventHandler);
    }

    protected override void RemoveListeners()
    {
      // Remove event listeners here.
      base.RemoveListeners();
      ExampleEvent.RemoveListeners(ExampleEventHandler)
    }

    private void ExampleEventHandler(int value)
    {
      // Set new properties based on the passed value.
      SetProperties(new ExampleEntityProperties(value));
    }

    protected override void OnPropertiesSet()
    {
      // Handle initializing this entity given its properties.
      base.OnPropertiesSet();
      Debug.LogFormat("My data's properties are: {0}", Properties.data);
    }
  }
}
~~~

## Button Controllers

Button controllers are specialized entity controllers that control a UIButton component. Because they inherit from ***AEntityController\<T>,*** they retain all the property behaviours detailed [above](#entity-controllers).

The default implementation of a button controller is the ***AButtonController\<T>*** class. The controller auto-registers callbacks to its controlled UIButton component and exposes a few methods that subclasses can easily override, listed below:

- ***HandleOnClick():*** Handler for backend behaviour (passing data, sending out events, etc) when the controlled UIButton component was clicked.
- ***AnimateOnClick():*** Handler for frontend behaviour (setting animator values) when the controlled UIButton component was clicked.
- ***HandleInteractabilityChanged(bool value):*** Handler for backend behaviour when interactability has been changed.
- ***AnimateInteractabilityChanged(bool value):*** Handler for frontend behaviour when interactability has been changed.
  
In addition, ***AButtonController\<T>*** exposes a few public methods, listed below:

- ***ToggleInteractability(bool value):*** Toggles interactability on controlled UIButton component to the given value.

In the case of a button controller that does not need any specialized properties, simply inherit from the ***AButtonController*** class.

For an example, see the ExampleButtonController below:

~~~C#
{
  // Make sure your custom properties class inherits from AButtonProperties.
  [System.Serializable]
  public class ExampleButtonProperties : AButtonProperties
  {
    public string key;
  }

  public class ExampleButtonController : AButtonController<ExampleButtonProperties>
  {
    [SerializeField] private Gesture gesture;
    [SerializeField] private Animator anim;

    protected override void HandleOnClick()
    {
      // Handle backend behaviour like dispatching a global message to other scripts here.
      base.HandleOnClick();
      Debug.LogFormat("My name is {0}, my key is {1}, and I was clicked!", gameObject.name, Properties.key);
    }

    protected override void AnimateOnClick()
    {
      // In most cases, Doozy can manage frontend behaviour, but in the case that a button controller
      // needs to change visually when clicked in a format that is not supported by Doozy, do it here.
      base.AnimateOnClick();
      anim.SetTrigger("Clicked");
    }

    protected override void HandleInteractabilityChanged(bool value)
    {
      // Use cases for this method are slim, but manage specific behaviour like disabling controlling
      // gestures here.
      base.HandleInteractabilityChanged();
      gesture.Cancel();
    }

    protected override void AnimateInteractabilityChanged(bool value)
    {
      // By default, the base AButtonController<T> class will change alpha value on the UIButton
      // component's CanvasGroup, so any extra behaviour to indicate uninteractability can be done here.
      base.AnimateInteractabilityChanged();
      anim.SetBool("Interactable", value);
    }
  }
}
~~~

## Toggle Controllers

Toggle controllers are specialized entity controllers that control a UIToggle component. Because they inherit from ***AEntityController\<T>,*** they retain all the property behaviours detailed [above](#entity-controllers).

The default implementation of a toggle controller is the ***AToggleController\<T>*** class. The controller auto-registers callbacks to its controlled UIToggle component and exposes a few methods that subclasses can easily override, listed below:

- ***HandleValueChanged(bool value):*** Handler for backend behaviour (passing data, sending out events, etc) when the controlled UIToggle component's value was changed from on to off or vice versa.
- ***AnimateValueChanged(bool value):*** Handler for frontend behaviour (setting animator values) when the controlled UIToggle component's value was changed from on to off or vice versa.
- ***HandleInteractabilityChanged(bool value):*** Handler for backend behaviour when interactability has been changed.
- ***AnimateInteractabilityChanged(bool value):*** Handler for frontend behaviour when interactability has been changed.
  
In addition, ***AToggleController\<T>*** exposes a few public methods, listed below:

- ***ToggleInteractability(bool value):*** Toggles interactability on controlled UIButton component to the given value.
- ***ToggleInternal(bool value):*** Toggles value of controlled UIToggle to the given value, but prevents backend callbacks from being dispatched.

In the case of a toggle controller that does not need any specialized properties, simply inherit from the ***AToggleController*** class.

For an example, see the ExampleToggleController below:

~~~C#
{
  // Make sure your custom properties class inherits from AToggleProperties.
  [System.Serializable]
  public class ExampleToggleProperties : AToggleProperties
  {
    public string key;
  }

  public class ExampleToggleController : AToggleController<ExampleToggleProperties>
  {
    [SerializeField] private Animator anim;

    protected override void HandleValueChanged(bool value)
    {
      // Handle backend behaviour like dispatching a global message to other scripts here.
      base.HandleOnClick();
      Debug.LogFormat("My name is {0}, my key is {1}, and I was changed to {2}!", gameObject.name, Properties.key, value);
    }

    protected override void AnimateValueChanged(bool value)
    {
      // In most cases, Doozy can manage frontend behaviour, but in the case that a toggle controller
      // needs to change visually when value is changed in a format that is not supported by
      // Doozy, do it here.
      base.AnimateOnClick();
      anim.SetBool("Toggled", value);
    }

    protected override void AnimateInteractabilityChanged(bool value)
    {
      // By default, the base AToggleController<T> class will change alpha value on the UIToggle
      // component's CanvasGroup, so any extra behaviour to indicate uninteractability can be done here.
      base.AnimateInteractabilityChanged();
      anim.SetBool("Interactable", value);
    }
  }
}
~~~

## View Controllers

View controllers are specialized entity controllers that control a UIView component. Because they inherit from ***AEntityController\<T>,*** they retain all the property behaviours detailed [above](#entity-controllers).

The default implementation of a view controller is the ***AViewController\<T>*** class. The controller auto-registers callbacks to its controlled UIVIew component and exposes a few methods that subclasses can easily override, listed below:

- ***OnShowStarted():*** Handler for behaviour when controlled UIView has started its show animation.
- ***OnShowFinished():*** Handler for behaviour when controlled UIView has finished its show animation.
- ***OnHideStarted():*** Handler for behaviour when controlled UIView has started its hide animation.
- ***OnHideFinished():*** Handler for behaviour when controlled UIView has finished its hide animation.

In the case of a view controller that does not need any specialized properties, simply inherit from the ***AViewController*** class.

For an example, see the ExampleViewController below:

~~~C#
{
  // Make sure your custom properties class inherits from AViewProperties.
  [System.Serializable]
  public class ExampleViewProperties : AViewProperties
  {
    public string title;
    public string description;
  }

  public class ExampleViewController : AViewController<ExampleViewProperties>
  {
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;

    protected override void OnPropertiesSet()
    {
      // Generally, initialize components based on properties here.
      base.OnPropertiesSet();
      titleText.text = Properties.title;
      descriptionText.text = Properties.description;
    }

    protected override void OnShowStarted()
    {
      base.OnShowStarted();
      Debug.LogFormat("My name is {0} and I started showing!", gameObject.name);
    }

    protected override void OnShowFinished()
    {
      base.OnShowFinished();
      Debug.LogFormat("My name is {0} and I finished showing!", gameObject.name);
    }

    protected override void OnHideStarted()
    {
      base.OnHideStarted();
      Debug.LogFormat("My name is {0} and I started hiding!", gameObject.name);
    }

    protected override void OnHideFinished()
    {
      base.OnHideFinished();
      Debug.LogFormat("My name is {0} and I finished hiding!", gameObject.name);
    }
  }
}
~~~

Because UIViews encapsulate all UI that are not specifically a toggle, button, or a simple image that needs no controller, the UIView class is designed to be extended to achieve any custom view desired. Some unique repeated implementations are available below like [toggle groups](#toggle-group-controllers), [button groups](#button-group-controllers), [scroller controllers](#scroller-controllers), and [tab systems](#tab-menu-controllers).

### Toggle Group Controllers

Toggle group controllers are specialized view controllers that control a ToggleGroup component and a group of [toggle controllers](#toggle-controllers).

The default implementation of a toggle group controller is the ***AToggleGroupController\<T, U, V>*** class. The controller auto-registers callbacks of the toggle controllers it controls and exposes a few methods that subclasses can easily override, listed below:

- ***PopulateToggles():*** Handler to populate toggles. By default called when properties have been set. Delegates to contained properties to generate properties for controlled toggles.
- ***ClearToggles():*** Handler to clear toggles. By default called when properties have been set before ***PopulateToggles()*** is called.
- ***Toggle_Toggled():*** Handler for behaviour when a controlled toggle controller is toggled on or off.

Toggle group controllers are only able to control a group of toggle controllers of a specific type. This is by design. In essence, they serve as a proxy for callbacks from a group of toggle controllers in addition to providing more control over a group.

For an example, see the ExampleToggleGroupController that controls a group of ExampleToggleController below:

~~~C#
{
  #region ExampleToggleController
  // This is an empty toggle controller that stores an integer in its properties.
  [System.Serializable]
  public class ExampleToggleProperties
  {
    public int data;
    public ExampleToggleProperties(int data)
    {
      this.data = data;
    }
  }
  public class ExampleToggleController : AToggleController<ExampleToggleProperties> {}
  #endregion

  #region ExampleToggleGroupController
  // Make sure your custom properties inherits from AToggleGroupProperties, passing
  // the type of properties of the toggle controller this group will control to it.
  [System.Serializable]
  public class ExampleToggleGroupProperties : AToggleGroupProperties<ExampleToggleProperties>
  {
    public List<int> dataSet;

    public override List<ExampleToggleProperties> GetToggleProperties()
    {
      // Because this method is abstract in AToggleGroupProperties<T>, each custom toggle group properties
      // class must implement. Essentially, this method creates and returns the properties of the toggles
      // this group will control.
      List<ExampleToggleProperties> propertiesList = new List<ExampleToggleProperties>();
      foreach (int data in dataSet)
        propertiesList.Add(new ExampleToggleProperties(data));
      return propertiesList;
    }
  }

  public class ExampleToggleGroupController : AToggleGroupController<ExampleToggleGroupProperties, ExampleToggleController, ExampleToggleProperties>
  {
    protected override void PopulateToggles()
    {
      // By default, the base AToggleGroupController<T, U, V> will populate controlled
      // toggles by delegating to the properties on this object, so manage any behaviour
      // that needs to happen after toggles have spawned here.
      base.PopulateToggles();
      foreach (ExampleToggleController toggle in Toggles)
        Debug.LogFormat("Populated {0}!", toggle.name);
    }

    protected override void ClearToggles()
    {
      // By default, the base AToggleGroupController<T, U, V> will destroy the controlled
      // toggles in the base method, so manage any other behaviour that needs to happen here
      // BEFORE calling base.ClearToggles();
      foreach (ExampleToggleController toggle in Toggles)
        Debug.LogFormat("Destroying {0}!", toggle.name);
      base.ClearToggles();
    }

    protected override void Toggle_Toggled(ExampleToggleController toggle, bool value)
    {
      // This method is intended to manage behaviour when a controlled toggle has changed value.
      // From here, you can send out global messages, synchronize other toggle groups, etc.
      Debug.LogFormat("My controlled toggle {0} was toggled to {1} and his property's value is {2}!", toggle.name, value, toggle.Properties.data);
    }
  }
  #endregion
}
~~~

### Button Group Controllers

Button group controllers are specialized view controllers that control a group of [button controllers](#button-controllers).

The default implementation of a button group controller is the ***AButtonGroupController\<T, U, V>*** class. The controller auto-registers callbacks of the button controllers it controls and exposes a few methods that subclasses can easily override, listed below:

- ***PopulateButtons():*** Handler to populate buttons. By default called when properties have been set. Delegates to contained properties to generate properties for controlled buttons.
- ***ClearButtons():*** Handler to clear buttons. By default called when properties have been set before ***PopulateToggles()*** is called.
- ***Button_Clicked():*** Handler for behaviour when a controlled button controller clicked.

Button group controllers are only able to control a group of button controllers of a specific type. This is by design. In essence, they serve as a proxy for callbacks from a group of button controllers in addition to providing more control over a group.

For an example, see the ExampleButtonGroupController that controls a group of ExampleButtonController below:

~~~C#
{
  #region ExampleButtonController
  // This is just an empty button controller that stores an integer in its properties.
  [System.Serializable]
  public class ExampleButtonProperties
  {
    public int data;
    public ExampleButtonProperties(int data)
    {
      this.data = data;
    }
  }
  public class ExampleButtonController : AButtonController<ExampleButtonProperties> {}
  #endregion

  #region ExampleButtonGroupController
  // Make sure your custom properties inherits from AButtonGroupProperties, passing
  // the type of properties of the button controller this group will control to it.
  [System.Serializable]
  public class ExampleButtonGroupProperties : AButtonGroupProperties<ExampleButtonProperties>
  {
    public List<int> dataSet;

    public override List<ExampleButtonProperties> GetButtonProperties()
    {
      // Because this method is abstract in AButtonGroupProperties<T>, each custom button group properties
      // class must implement. Essentially, this method creates and returns the properties of the buttons
      // this group will control.
      List<ExampleButtonProperties> propertiesList = new List<ExampleButtonProperties>();
      foreach (int data in dataSet)
        propertiesList.Add(new ExampleButtonProperties(data));
      return propertiesList;
    }
  }

  public class ExampleButtonGroupController : AButtonGroupController<ExampleButtonGroupProperties, ExampleButtonController, ExampleButtonProperties>
  {
    protected override void PopulateButtons()
    {
      base.PopulateButtons();
      // By default, the base AButtonGroupController<T, U, V> will populate controlled
      // buttons by delegating to the properties on this object, so manage any behaviour
      // that needs to happen after buttons have spawned here.
      foreach (ExampleButtonController button in Buttons)
        Debug.LogFormat("Populated {0}!", button.name);
    }

    protected override void ClearButtons()
    {
      // By default, the base AButtonGroupController<T, U, V> will destroy the controlled
      // buttons in the base method, so manage any other behaviour that needs to happen here
      // BEFORE calling base.ClearButtons();
      foreach (ExampleButtonController button in Buttons)
        Debug.LogFormat("Destroying {0}!", button.name);
      base.ClearButtons();
    }

    protected override void Button_Clicked(ExampleButtonController button, bool value)
    {
      // This method is intended to manage behaviour when a controlled button has changed value.
      // From here, you can send out global messages, synchronize other button groups, etc.
      Debug.LogFormat("My controlled button {0} was clicked and his property's value is {1}!", button.name, button.Properties.data);
    }
  }
  #endregion
}
~~~

### Scroller Controllers

Scroller controllers are specialized view controllers that control an EnhancedScroller component and by proxy, a group of cell view controllers.

**IMPORTANT:** the majority of the implementation of scroller controllers is built on the EnhancedScroller plugin. If parts of the implementation don't make sense, reference the EnhancedScroller documentation and examples.

> **Cell View Controllers**
>
> Cell view controllers are specialized objects used and controlled by scroller controllers that control individual cell views in a scroller.
>
> The default implementation of cell view controllers ***ACellViewController\<T>*** extends the EnhancedScroller plugin's class **EnhancedScrollerCellView** because the EnhancedScroller plugin requires it. In addition, it also implements the interface that drive entity controllers, allowing for the property behaviour detailed [above](#entity-controllers).

The default implementation of a scroller controller is the ***AScrollerController\<T>*** class. The controller serves as IEnhancedScrollerDelegate for the EnhancedScroller plugin, and as such handles the majority of behaviour internally, but exposes one abstract method that must be implemented, listed below:

- ***GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex):*** gets and returns the prefab for the cell view to be spawned based on the given data/cell index. Called by the controlled EnhancedScroller through its IEnhancedScrollerDelegate interface.

The key to what makes scroller controllers work is in their properties. By default, their properties store a SmallList\<ACellViewProperties>, which the controlled EnhancedScroller uses to drive population of cell views.

For an example, see the ExampleScrollerController that controls a group of ExampleCellViewController below:

~~~C#
{
  #region ExampleCellViewController
  // This is just an empty cell view controller that stores an integer in its properties.
  [System.Serializable]
  public class ExampleCellViewProperties
  {
    public int data;
    public ExampleCellViewProperties(int data)
    {
      this.data = data;
    }
  }
  public class ExampleCellViewController : ACellViewController<ExampleCellViewProperties> {}
  #endregion

  #region ExampleScrollerController
  // Make sure your custom properties inherits from AScrollerProperties.
  [System.Serializable]
  public class ExampleScrollerProperties : AScrollerProperties
  {
    public List<int> dataSet;
  }

  public class ExampleScrollerController : AScrollerController<ExampleScrollerProperties>
  {
    // Make sure you store the prefabs for the cell view types 
    [SerializeField] private ExampleCellViewController exampleCellViewPrefab;

    public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
      // This syntax is pulled straight from the EnhancedScroller examples.
      if (Properties.ScrollerData[dataIndex] is ExampleCellViewProperties)
      {
        ExampleCellViewController exampleCellView = scroller.GetCellView(exampleCellViewPrefab) as ExampleCellViewPrefab;
        exampleCellView.SetPropertes(Properties.ScrollerData[dataIndex]);
        return cellView
      }
      else
      {
        return null;
      }
    }
  }
  #endregion
}
~~~
