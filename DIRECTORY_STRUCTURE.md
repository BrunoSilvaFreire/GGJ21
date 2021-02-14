# Directory Structure

An ASMDEF is an Module

If something requires module `GGJ.A` and `GGJ.B`, then it goes in a third module `GGJ.A.B`

There may be different "types" of modules (By "types" I don't mean that there is any different on the ASMDEFs, it's just a convention), for example:
* Basic: The most necessary bits of code that are required for an
* Common: Extra classes that are not **required** for the implementation
* UI: pretty obvious no?

A module only contains the most basic classes of a system. If you wanna include some other classes with more implementation, do so in a "Common" module.
e.g. `GGJ.Traits.Common`, `GGJ.UI`

* Scripts/
    * Master/
        * When something uses a lot of modules, and there is not a module were it fits, it goes here.
    * Game/
        * Information about game state, and classes crucial about gameplay (eg: Knowledge)
    * Traits/
        *
    *