# Controly

A collection of interfaces and classes to handle data-driven systems in Unity.

## Installation

1. Make sure your Unity project has the [Unity UI](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/index.html) and [TextMesh Pro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@2.0/manual/index.html) packages fully installed through the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html).
1. Get the latest version of [DoozyUI](https://assetstore.unity.com/packages/tools/gui/doozyui-complete-ui-management-system-138361) and [install it into your Unity project](https://www.youtube.com/watch?v=4eFPI4tHE_w).
    * [Set up assembly definitions for DoozyUI](https://www.youtube.com/watch?v=asoFklJ8kfk)
1. **(Optional)** Get the latest version of [EnhancedScroller](https://assetstore.unity.com/packages/tools/gui/enhancedscroller-36378) and import it into your Unity project.
    * Set up assembly definitions for EnhancedScroller (same as DoozyUI)
1. Either [download the latest released Unity Package](https://github.com/dcolina900lbs/com.900lbs.controly/releases) and import it into your Unity project or [download the zip](https://github.com/dcolina900lbs/com.900lbs.controly/archive/master.zip) of this repository and extract its contents into your Unity project.
1. For each of the listed Assembly Definitions below, go to their dependencies and drag in references to the assembly definition on the right they are paired with.
    * `Runtime\UI\900lbs.Controly.UI` > `Doozy.Engine.asmdef`
    * **(Optional if EnhancedScroller installed)** `Runtime\UI\View\Scroller\900lbs.Controly.UI.View.Scroller.asmdef` > `EnhancedScroller.asmdef`
1. Open the settings window under `Tools > Controly`. If you installed and set up assembly definitions correctly for each of the elements, you should be able to click `Disabled` to enable the different modules.

## Getting Started

I strongly recommend installing the [Controly Examples](https://github.com/dcolina900lbs/controly-examples) and browsing the sample scenes there.

Also, check out the [Wiki](https://github.com/dcolina900lbs/com.900lbs.controly/wiki) for breakdowns of the different interfaces and classes.
