# UI Framework

## Table of Contents

- [UI Framework](#ui-framework)
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

The UI Framework is a collection of abstract classes designed to help manage behaviour on data-driven user interfaces. Built to run parallel with DoozyUI components and serialized fully by the Odin Inspector, the framework allows for dynamic and property-driven entities.

This framework is designed to provide a foundation of back-end behaviours often repeated throughout UI code like data management, callbacks, and internal state tracking all while remaining decoupled from the front-end behaviour of the controlled entity.

## Controllers & Properties

The framework is composed of two primary elements:

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

  // Make sure your custom controller class gives its properties as the generic parameter T
  // when inheriting from AEntityController<T>.
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

### Button Group Controllers

ButtonGroup controllers manage a group of

### Scroller Controllers

### Tab Menu Controllers

Tab page controllers control a UIView component