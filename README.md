# Audune Audio for FMOD

This repository contains scripts to enhance the **FMOD for Unity** package, which is used in Audune's own projects.

The FMOD for Unity package provides a thin wrapper around the C/C++ FMOD studio code and confronts the developer with many C++ paradigms ported to C#. This package was made to make the use of that package easier from a C# perspective, using C# paradigms like properties and events out of the box.

See the [wiki](https://github.com/audunegames/unity-audio-for-fmod/wiki) of the repository to get started with the package.

## Installation

Audune Audio for FMOD can be best installed as a git package in the Unity Editor using the following steps:

* In the Unity editor, navigate to **Window > Package Manager**.
* Click the **+** icon and click **Add package from git URL...**
* Enter the following URL in the URL field and click **Add**:

```
https://github.com/audunegames/unity-audio-for-fmod.git
```

Make sure you have the following dependencies installed before installing this package. The package might work with lower versions of the dependencies, but those have not been tested:

* [FMOD for Unity](https://fmod.com/download#fmodforunity) >=2.00.19
* [Odin Inspector](https://odininspector.com/download) >=3.2.1.0

*Note: We are aware that not everyone who wants to use this package has access to Odin Inspector and we're working on making our editor code work with the native Unity Editor functionality.*

## License

This package is licensed under the GNU GPL 3.0 license. See `LICENSE.txt` for more information.
