# UI Framework

The UI Framework is a collection of abstract classes and interfaces designed to help manage behaviour on data-driven user interfaces. Built to run parallel with DoozyUI components and serialized fully by the Odin Inspector, the framework allows for dynamic and property-driven entities.

This framework is designed to provide a foundation of back-end behaviours often repeated throughout UI code like data management, callbacks, and internal state tracking all while remaining decoupled from the front-end behaviour of the controlled UI.

The framework is composed of two primary elements: **Controllers** and **Properties**. Controllers manage behaviour on objects, while Properties drive how they behave and/or what data they display.

## Overview

+ **Entity** - controls any arbitrary entity that holds properties. All other controllers inherit from this.
  + **Button** - controls a DoozyUI **UIButton**. Manages callbacks for when clicked and toggling interactability.
  + **Toggle** - controls a DoozyUI **UIToggle**. Manages callbacks for when value is changed, toggling interactability, and toggling 
    + **Tab** - special type of toggle used by a **TabMenu** to open and close **TabPages**.
  + **View** - controls a DoozyUI **UIView**. Manages callbacks for visibility changes.
    + **Toggle Group** - controls a **ToggleGroup** component as well as a group of Toggle Controllers.