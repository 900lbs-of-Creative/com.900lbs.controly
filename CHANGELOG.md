# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.2.0] - 2020-7-7

### Changed

- Updated the [README.md](README.md) significantly to account for new and recent changes.
- Changed `AToggleGroupController` and `AButtonGroupController` to delegate to subclasses to initialize instantiated toggles and buttons.
- Changed `AToggleController`'s output `Toggled` event to `ValueChanged`.
- Changed `AScrollerController`'s `Start` method to `protected virtual` accessibility.
- Changed all assembly definitions to depend on names as opposed to GUIDs.

### Fixed

- Fixed a bug where `ATabMenuController` not correctly toggling on the first tab when spawning tabs.

## [2.1.1] - 2020-2-17

### Changed

- Removed component requirements from AViewController, AToggleController, and AButtonController to allow for controlled components to exist on different objects.

## [2.1.0] - 2020-2-5

### Changed

- Reworked the entire framework to no longer be a git-importable package. Instead, just download the most recent release from the [Releases]("https://github.com/900lbs-dcolina/com.900lbs.controly/releases") page, import
the Unity Package file into Unity, and edit the Assembly Definition files as needed (see Doozy's [video on setting up assembly definitions]("https://www.youtube.com/watch?v=asoFklJ8kfk")) for reference on this.

## [2.0.2] - 2020-2-5

### Fixed

- Attempted fix of ProcessorSettings bug.

## [2.0.1] - 2020-1-30

### Planned

- Once OdinInspector fully supports nested prefabs, a re-implementation or re-addressal of `SerializedMonoBehaviour`. See the [Odin roadmap]("https://odininspector.com/roadmap") for information and possible dates.

### Changed

- Reworked the implementation of `ATabMenuController` to depend on interfaces instead of dynamic compile-time type interpretation.
- Changed `AToggleController` to no longer block the first `OnValueChanged` event.

### Fixed

- Attempted fix of the `ControlyEditorUtils` bug.

## [2.0.0] - 2020-1-29

### Changed

- Reworked many base abstract classes to implement and depend on interfaces and rather than implementations.

### Fixed

- Fixed a few typos throughout documentation, classes, and comments.

### Removed

- Removed various base implementation classes, particularly Property classes.

## [1.1.0] - 2019-12-18

### Added

- Added a new class called SerializedDictionary<TKey, TValue> to allow for the now MonoBehaviour behaviours to have serialized Dictionaries.

### Changed

- Replaced base classes for all custom class implementations to MonoBehaviour as opposed to SerializedMonoBehaviour to prevent serialization loss issues.
- Removed dependencies on OdinInspector.

## [1.0.2] - 2019-12-11

### Changed

- Updated the `IEntityController` interface to `IEntityController<TEntityProperties>` to force a property type to be passed in.
- Updated the three base UI classes (`AViewController`, `AButtonController`, `AToggleController`) to inherit from `SerializedMonoBehaviour` and instead implement the new `IEntityController<TEntityProperties>` interface for their unique properties.
- Updated ACellViewController to implement the new `IEntityController<TEntityProperties>` interface.
- Added a few comments and fixed a few typos and misnomers.

### Removed

- Removed `ATabMenuController`, `ATabPageController`, and `ATabController`. Plan to reimplement at some point in the future.

## [1.0.1] - 2019-12-5

### Changed

- Removed default interactability changed behaviour for toggle controllers and button controllers (previously changed CanvasGroup alpha immediately) to allow for developers to override behaviour as desired through AnimateInteractabilityChanged.
- Updated properties for controllers that don't need special properties to be serialized by Unity rather than Odin, forcing the default drawer of nothing rather than Odin's special property drawer.
- Attempted fix to the import automation pipeline.
- Added a few comments and fixed a few typos and misnomers.

## [1.0.0] - 2019-10-30

### Added

- EntityControllers that hold properties for data-driven objects.
- UI Controllers that run parallel to [DoozyUI](https://www.doozyui.com) components (UIButton, UIToggle, UIView).
- Specialized ScrollerController for the [EnhancedScroller](https://assetstore.unity.com/packages/tools/gui/enhancedscroller-36378) plugin.
- Import pipeline management of package, which automates creating assembly definitions and referencing other assemblies in the project as needed.

## [Unreleased]
