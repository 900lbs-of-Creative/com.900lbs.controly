# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2019-10-30

### Added

- EntityControllers that hold properties for data-driven objects.
- UI Controllers that run parallel to [DoozyUI](https://www.doozyui.com) components (UIButton, UIToggle, UIView).
- Specialized ScrollerController for the [EnhancedScroller](https://assetstore.unity.com/packages/tools/gui/enhancedscroller-36378) plugin.
- Import pipeline management of package, which automates creating assembly definitions and referencing other assemblies in the project as needed.

## [1.0.1] - 2019-12-5

### Changed

- Removed default interactability changed behaviour for toggle controllers and button controllers (previously changed CanvasGroup alpha immediately) to allow for developers to override behaviour as desired through AnimateInteractabilityChanged.
- Updated properties for controllers that don't need special properties to be serialized by Unity rather than Odin, forcing the default drawer of nothing rather than Odin's special property drawer.
- Attempted fix to the import automation pipeline.
- Added a few comments and fixed a few typos and misnomers.
