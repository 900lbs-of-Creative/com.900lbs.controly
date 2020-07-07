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

## Overview

Controly is a collection of abstract classes designed to help manage behaviour on data-driven user interfaces. Built to run parallel with DoozyUI components, the framework allows for dynamic and property-driven entities.

This framework is designed to provide a foundation of back-end behaviours often repeated throughout UI code like data management, callbacks, and internal state tracking all while remaining decoupled from the front-end behaviour of the controlled entity.

## Controllers & Properties

Controly is composed of two primary elements:

- **Controllers**: These are classes that control specific things, like a Toggle, Button, or View.
- **Properties**: These are classes usually declared alongside the controller they are intended for and describe how the controller will behave or what it will cause its controlled entity to display.

The classes themselves are designed so that custom controllers and properties can inherit from them to allow for any type of desired behaviour.

## Entity Controllers

Entity controllers serve as the foundation for the entire framework. At their core, they are specialized MonoBehaviours that hold properties.

The default implementation of an entity controller is the `AEntityController<TEntityProperties>` class, which exposes a few protected methods that subclasses can easily override, listed below:

- `AddListeners()`: Handler for registering to delegate methods and events. By default called by `OnEnable()`.
- `RemoveListeners()`: Handler for un-registering to delegate methods and events. Be default called by `OnDisable()`.
- `OnPropertiesSet()`: Handler for initializing controller based on given properties. By default called when the properties of this entity have been set.
  - **IMPORTANT**: for some controllers like [View Controllers](#view-controllers), it is common that this method may be called before the GameObject is actually turned on, which can prevent Coroutines from executing. It is recommended to avoid starting Coroutines from this method and instead just initialize component values.
  
In addition, `AEntityController<TEntityProperties>` exposes a public method `SetProperties(TEntityProperties properties)` which can be called to set the properties of the entity, triggering the `OnPropertiesSet()` method in turn.

Because of their high level of abstraction, entity controllers can be declared that inherit from the base `AEntityController<TEntityProperties>` class to control any entity that needs to contain properties, from actual UI elements like [buttons](#button-controllers) and [toggles](#toggle-controllers) to camera controllers, to any entity in need of data.

For an example, see the `ExampleEntityController` below:

~~~C#
// A simple delegate that passes an integer.
public static Action<int> ExampleEvent;

// Make sure your custom properties class implements IEntityProperties.
// If you want your properties to be viewable in editor,
// make sure to mark it with the [System.Serializable] attribute.
[System.Serializable]
public class ExampleEntityProperties : IEntityProperties
{
    public int value;
    public ExampleEntityProperties(int value)
    {
    this.value = value;
    }
}

public class ExampleEntityController : AEntityController<ExampleEntityProperties>
{
    protected override void AddListeners()
    {
    // Add event listeners here.
    base.AddListeners();
    ExampleEvent += ExampleEventHandler;
    }

    protected override void RemoveListeners()
    {
    // Remove event listeners here.
    base.RemoveListeners();
    ExampleEvent -= ExampleEventHandler;
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
    Debug.Log($"My value is: {Properties.data}");
    }
}
~~~

## Button Controllers

Button controllers are specialized entity controllers that control a [UIButton](http://doozyui.com/uibutton/) component.

The default implementation of a button controller is the `AButtonController<TButtonProperties>` class. This class auto-registers callbacks to its controlled [UIButton](http://doozyui.com/uibutton/) component and exposes a few methods that subclasses can easily override (including the ones mentioned above in [entity controllers](#entity_controllers)):

- `HandleOnClick()`: Handler for backend behaviour (passing data, sending out events, etc) when clicked.
- `AnimateOnClick()`: Handler for frontend behaviour when clicked.
- `HandleInteractabilityChanged(bool value)`: Handler for backend behaviour when interactability has been changed.
- `AnimateInteractabilityChanged(bool value)`: Handler for frontend behaviour when interactability has been changed.
  
In addition, `AButtonController<TButtonProperties>` exposes a few public methods:

- `ToggleInteractability(bool value)`: Toggles interactability on controlled [UIButton](http://doozyui.com/uibutton/) component to the given value.

In the case of a button controller that does not need any specialized properties, simply inherit from the `AButtonController` class.

For an example, see the `ExampleButtonController` below:

~~~C#
// Make sure your custom properties class implements IButtonProperties.
[System.Serializable]
public class ExampleButtonProperties : IButtonProperties
{
    public string key;
    public ExampleButtonProperties(string key)
    {
        this.key = key;
    }
}

public class ExampleButtonController : AButtonController<ExampleButtonProperties>
{
    [SerializeField] private Gesture gesture;
    [SerializeField] private Animator anim;

    protected override void HandleOnClick()
    {
        // Handle backend behaviour like dispatching a global
        // message to other scripts here.
        base.HandleOnClick();
        Debug.Log($"My key is {Properties.key}, and I was clicked!");
    }

    protected override void AnimateOnClick()
    {
        // In most cases, DoozyUI can manage visual behaviour,
        // but in the case that a button controller needs to change
        // visually when clicked in a format that is not supported
        // by DoozyUI, do it here.
        base.AnimateOnClick();
        anim.SetTrigger("Clicked");
    }

    protected override void HandleInteractabilityChanged(bool value)
    {
        // Use cases for this method are slim, but handle specific
        // non-visual behaviour like disabling controlling gestures here.
        base.HandleInteractabilityChanged();
        gesture.Cancel();
    }

    protected override void AnimateInteractabilityChanged(bool value)
    {
        // As with AnimateOnClick, DoozyUI can usually manage visual
        // behaviour, but in the case that a button controller needs to
        // change visually when interactability is enabled or disabled
        // that is not supported by DoozyUI, do it here.
        base.AnimateInteractabilityChanged();
        anim.SetBool("Interactable", value);
    }
}
~~~

## Toggle Controllers

Toggle controllers are specialized entity controllers that control a [UIToggle](http://doozyui.com/uitoggle/) component.

The default implementation of a toggle controller is the `AToggleController<TToggleProperties>` class. This class auto-registers callbacks to its controlled [UIToggle](http://doozyui.com/uitoggle/) component and exposes a few methods that subclasses can easily override:

- `HandleValueChanged(bool value)`: Handler for backend behaviour (passing data, sending out events, etc) when toggled.
- `AnimateValueChanged(bool value)`: Handler for frontend behaviour (setting animator values) when toggled.
- `HandleInteractabilityChanged(bool value)`: Handler for backend behaviour when interactability has been changed.
- `AnimateInteractabilityChanged(bool value)`: Handler for frontend behaviour when interactability has been changed.
  
In addition, `AToggleController<TToggleProperties>` exposes a few public methods, listed below:

- `ToggleInteractability(bool value)`: Toggles interactability on controlled [UIToggle](http://doozyui.com/uitoggle/) component to the given value.
- `ToggleInternal(bool value)`: Toggles controlled [UIToggle](http://doozyui.com/uitoggle/) to the given value, but prevents backend callbacks from being dispatched.

In the case of a toggle controller that does not need any specialized properties, simply inherit from the `AToggleController` class.

For an example, see the `ExampleToggleController` below:

~~~C#
// Make sure your custom properties class implements IToggleProperties.
[System.Serializable]
public class ExampleToggleProperties : IToggleProperties
{
    public string key;
    public ExampleToggleProperties(string key)
    {
        this.key = key;
    }
}

public class ExampleToggleController : AToggleController<ExampleToggleProperties>
{
    [SerializeField] private Animator anim;

    protected override void HandleValueChanged(bool value)
    {
        // Handle backend behaviour like dispatching a global
        // message to other scripts here.
        base.HandleValueChanged(value);
        Debug.Log($"My key is {Properties.key}, and I was changed to {value}!");
    }

    protected override void AnimateValueChanged(bool value)
    {
        // In most cases, DoozyUI can manage visual behaviour,
        // but in the case that a toggle controller needs to change
        // visually when toggled in a format that is not supported
        // by DoozyUI, do it here.
        base.AnimateOnClick();
        anim.SetBool("Toggled", value);
    }

    protected override void AnimateInteractabilityChanged(bool value)
    {
        // As with AnimateValueChanged, DoozyUI can usually manage visual
        // behaviour, but in the case that a toggle controller needs to
        // change visually when interactability is enabled or disabled
        // that is not supported by DoozyUI, do it here.
        base.AnimateInteractabilityChanged();
        anim.SetBool("Interactable", value);
    }
}
~~~

## View Controllers

View controllers are specialized entity controllers that control a [UIView](http://doozyui.com/uiview/) component.

The default implementation of a view controller is the `AViewController<TViewProperties>` class. The controller auto-registers callbacks to its controlled [UIView](http://doozyui.com/uiview/) component and exposes a few methods that subclasses can easily override:

- `OnShowStarted()`: Handler for behaviour when controlled UIView has started its show animation.
- `OnShowFinished()`: Handler for behaviour when controlled UIView has finished its show animation.
- `OnHideStarted()`: Handler for behaviour when controlled UIView has started its hide animation.
- `OnHideFinished()`: Handler for behaviour when controlled UIView has finished its hide animation.

**NOTE**: These methods are called whenever the controlled [UIView](http://doozyui.com/uiview/) component's animations DOTween start and finish, which can cause unexpected behaviour when using the [Progressor](http://doozyui.com/progressor/), [Progress Target Animator](http://doozyui.com/progress-target-animator/), and [Unity animator](https://docs.unity3d.com/Manual/class-Animator.html) to do more elaborate UI animations.

In the case of a view controller that does not need any specialized properties, simply inherit from the `AViewController` class.

For an example, see the `ExampleViewController` below:

~~~C#
// Make sure your custom properties class implements IViewProperties.
[System.Serializable]
public class ExampleViewProperties : IViewProperties
{
    public string title;
    public string description;
    public ExampleViewProperties(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}

public class ExampleViewController : AViewController<ExampleViewProperties>
{
    [SerializeField] private TextMeshProUGUI titleLabel;
    [SerializeField] private TextMeshProUGUI descriptionLabel;

    protected override void OnPropertiesSet()
    {
        // Initialize components based on properties here.
        base.OnPropertiesSet();
        titleLabel.text = Properties.title;
        descriptionLabel.text = Properties.description;
    }

    protected override void OnShowStarted()
    {
        base.OnShowStarted();
        Debug.Log($"My name is {gameObject.name} and I started showing!");
    }

    protected override void OnShowFinished()
    {
        base.OnShowFinished();
        Debug.Log($"My name is {gameObject.name} and I finished showing!");
    }

    protected override void OnHideStarted()
    {
        base.OnHideStarted();
        Debug.Log($"My name is {gameObject.name} and I started hiding!");
    }

    protected override void OnHideFinished()
    {
        base.OnHideFinished();
        Debug.Log($"My name is {gameObject.name} and I finished hiding!");
    }
}
~~~

Because UIViews encapsulate all UI that are not specifically a toggle, button, or a simple UI element that needs no controller, the UIView class is designed to be extended to achieve any custom desired view.

Some unique repeated implementations are available below like [toggle groups](#toggle-group-controllers), [button groups](#button-group-controllers), [scrollers](#scroller-controllers), and [tab menus](#tab-menu-controllers).

### Toggle Group Controllers

Toggle group controllers are specialized view controllers that control a [ToggleGroup](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/script-ToggleGroup.html) component and a group of [toggle controllers](#toggle-controllers).

The default implementation of a toggle group controller is the `AToggleGroupController<TToggleGroupProperties, TToggleController, TToggleProperties>` class. The controller auto-registers callbacks of the toggle controllers it controls and exposes a few methods that subclasses can easily override, listed below:

- `InitializeToggle(TToggleController toggle)`: Handler to initialize the given toggle. Usually, this can be used to set the name of the toggle or any other custom behaviour as the toggle is instantiated.
- `Toggle_ValueChanged(TToggleController toggle, bool value)`: Handler for when a controlled toggle's value is changed.

Toggle group controllers are only able to control a group of toggle controllers of a specific type. This is by design; in essence they serve as a proxy for callbacks from a group of toggle controllers in addition to providing more control over a group.

For an example, see the `ExampleToggleGroupController` that controls a group of `ExampleToggleController` below:

~~~C#
#region ExampleToggleController
// This is an simple toggle controller that stores an integer
// in its properties.
[System.Serializable]
public class ExampleToggleProperties
{
    public int value;
    public ExampleToggleProperties(int value)
    {
        this.value = value;
    }
}
public class ExampleToggleController : AToggleController<ExampleToggleProperties> {}
#endregion


#region ExampleToggleGroupController
// Make sure your custom properties implements IToggleGroupProperties<TToggleGroupProperties>,
// passing the type of properties of the toggle controller this group will control to it.
[System.Serializable]
public class ExampleToggleGroupProperties : IToggleGroupProperties<ExampleToggleProperties>
{
    public List<int> values;

    public List<ExampleToggleProperties> GetToggleProperties()
    {
        // Because this method is part of the IToggleGroupProperties<TToggleProperties>
        // interface, each custom toggle group properties must implement it.
        // Essentially, this method creates and returns the properties of the
        // toggles this toggle group will control.
        List<ExampleToggleProperties> exampleToggleProperties = new List<ExampleToggleProperties>();
        foreach (var value in values)
            exampleToggleProperties.Add(new ExampleToggleProperties(value));
        return exampleToggleProperties;
    }
}

public class ExampleToggleGroupController : AToggleGroupController<ExampleToggleGroupProperties, ExampleToggleController, ExampleToggleProperties>
{
    protected override InitializeToggle(ExampleToggleController toggle)
    {
        // Handle initializing the given toggle here. This can include
        // naming its GameObject, coupling specific components on it
        // to this toggle group, etc.
        toggle.name = $"[ExampleToggle] {toggle.Properties.value}";
    }

    protected override void Toggle_ValueChanged(ExampleToggleController toggle, bool value)
    {
        // Handle backend behaviour that occur when a controlled
        // toggle is toggled on or off here. You can think of this
        // as a conglomerate of toggles; if a value is changed on
        // the group, and all toggles are off, then no toggles are
        // on; and vice versa.
        if (ToggleGroup.AnyTogglesOn())
            Debug.Log("One toggle is on!");
        else
            Debug.Log("All toggles are off!");
    }
}
#endregion
~~~

### Button Group Controllers

Button group controllers are specialized view controllers that control a group of [button controllers](#button-controllers).

The default implementation of a button group controller is the `AButtonGroupController<TButtonGroupProperties, TButtonController, TButtonProperties>` class. The controller auto-registers callbacks of the button controllers it controls and exposes a few methods that subclasses can easily override, listed below:

- `InitializeButton(TButtonController button)`: Handler to initialize the given button. Usually, this can be used to set the name of the button or any other custom behaviour as the button is instantiated.
- `Button_Clicked(TButtonController button)`: Handler for when a controlled button is clicked.

Button group controllers are only able to control a group of button controllers of a specific type. This is by design; in essence they serve as a proxy for callbacks from a group of button controllers in addition to providing more control over a group.

For an example, see the `ExampleButtonGroupController` that controls a group of `ExampleButtonController` below:

~~~C#
#region ExampleButtonController
// This is an simple button controller that stores an integer
// in its properties.
[System.Serializable]
public class ExampleButtonProperties
{
    public int value;
    public ExampleButtonProperties(int value)
    {
        this.value = value;
    }
}
public class ExampleButtonController : AButtonController<ExampleButtonProperties> {}
#endregion


#region ExampleButtonGroupController
// Make sure your custom properties implements IButtonGroupProperties<TButtonGroupProperties>,
// passing the type of properties of the button controller this group will control to it.
[System.Serializable]
public class ExampleButtonGroupProperties : IButtonGroupProperties<ExampleButtonProperties>
{
    public List<int> values;

    public List<ExampleButtonProperties> GetButtonProperties()
    {
        // Because this method is part of the IButtonGroupProperties<TButtonProperties>
        // interface, each custom button group properties must implement it.
        // Essentially, this method creates and returns the properties of the
        // buttons this button group will control.
        List<ExampleButtonProperties> exampleButtonProperties = new List<ExampleButtonProperties>();
        foreach (var value in values)
            exampleButtonProperties.Add(new ExampleButtonProperties(value));
        return exampleButtonProperties;
    }
}

public class ExampleButtonGroupController : AButtonGroupController<ExampleButtonGroupProperties, ExampleButtonController, ExampleButtonProperties>
{
    protected override InitializeButton(ExampleButtonController button)
    {
        // Handle initializing the given button here. This can include
        // naming its GameObject, coupling specific components on it
        // to this button group, etc.
        button.name = $"[ExampleButton] {button.Properties.value}";
    }

    protected override void Button_Clicked(ExampleButtonController button)
    {
        // Handle backend behaviour that occur when a controlled
        // button is clicked here.
        Debug.Log($"Controlled button {button} was clicked");
    }
}
#endregion
~~~

### Scroller Controllers

Scroller controllers are specialized view controllers that control an [EnhancedScroller](https://assetstore.unity.com/packages/tools/gui/enhancedscroller-36378) component and by proxy, a group of cell view controllers.

**IMPORTANT:** the majority of the implementation of scroller controllers is built on the [EnhancedScroller](https://assetstore.unity.com/packages/tools/gui/enhancedscroller-36378) plugin. If parts of the implementation don't make sense, reference the linked documentation and examples.

> **Cell View Controllers**
>
> Cell view controllers are specialized objects used and controlled by scroller controllers that control individual cell views in a scroller.
>
> The default implementation of cell view controllers `ACellViewController<TCellViewProperties>` extends the EnhancedScroller plugin's class `EnhancedScrollerCellView` because the EnhancedScroller plugin requires it.
>
>In addition, it also implements the interface that drive entity controllers, allowing for the property behaviour detailed [above](#entity-controllers).

The default implementation of a scroller controller is the `AScrollerController<TScrollerProperties>` class. The controller serves as an `IEnhancedScrollerDelegate` for the `EnhancedScroller` class, and as such handles the majority of behaviour internally, but exposes one abstract method that must be implemented, listed below:

- `GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)`: gets and returns the prefab for the cell view to be spawned based on the given data/cell index. Called by the controlled `EnhancedScroller` through its `IEnhancedScrollerDelegate` interface.

The key to what makes scroller controllers work is in their properties. By default, their properties store a `SmallList<ICellViewProperties>`, which the controlled `EnhancedScroller` uses to drive population of cell views.

For an example, see the `ExampleScrollerController` that controls a group of `ExampleCellViewController` below:

~~~C#
#region ExampleCellViewController
// This is just an simple cell view controller that stores an integer in its properties.
[System.Serializable]
public class ExampleCellViewProperties
{
    public int value;
    public ExampleCellViewProperties(int value)
    {
        this.value = value;
    }
}
public class ExampleCellViewController : ACellViewController<ExampleCellViewProperties> {}
#endregion

#region ExampleScrollerController
public class ExampleScrollerProperties : IScrollerProperties
{
    public List<int> values;

    public SmallList<ICellViewProperties> ScrollerData { get; private set; } = new SmallList<ICellViewProperties>();

    // Populate the SmallList with a list of cell view data.
    public ExampleScrollerProperties(List<int> values)
    {
        this.values = values;
        foreach (var value in values)
            ScrollerData.Add(new ExampleCellViewProperties(value));
    }
}

public class ExampleScrollerController : AScrollerController<ExampleScrollerProperties>
{
    // Make sure you store the prefabs for the cell view types.
    [SerializeField] private ExampleCellViewController exampleCellViewPrefab;

    public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // This syntax is pulled straight from the EnhancedScroller examples.
        if (Properties.ScrollerData[dataIndex] is ExampleCellViewProperties exampleCellViewProperties)
        {
            ExampleCellViewController exampleCellView = scroller.GetCellView(exampleCellViewPrefab) as ExampleCellViewPrefab;
            exampleCellView.SetPropertes(exampleCellViewProperties);
            return cellView;
        }
        else
            return null;
    }
}
#endregion
~~~
