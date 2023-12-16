# Computer++

*Computer++* is a mod for the VR game Gorilla Tag. It enhances the functionality of the vanilla computer by adding a a few new features like the ability to enable/disable mods and even add a custom wallpaper.

## Links

* Discord: https://discord.com/invite/K79j2arExP
* YouTube: https://www.youtube.com/@kylethescientist

## Installation
To use *Bark*, you will need to have installed BepInEx and Utilla with [Monke Mod Manager](https://github.com/DeadlyKitten/MonkeModManager/releases)

Once you have installed the mod loader, simply drop the `ComputerPlusPlus-vX.X.X.dll` file into your plugins folder.

![Installation GIF](https://github.com/KyleTheScientist/Bark/blob/master/Marketing/HowToInstall.gif)

## Custom Wallpapers

To add a custom wallpaper, simply drop an image file called `wallpaper.png` into the `Gorilla Tag/BepInEx/plugins/ComputerPlusPlus` folder. The image must be a PNG file.

## Custom Screens

If you want to add your own screen to Computer++ you can do so by creating a BepInEx plugin with a class that implements the `ComputerPlusPlus.IScreen` interface. All classes that implement this interface will be automatically loaded and added to the list of screens. 

An example of a plugin that implements a custom screen can be seen below:

```csharp
using BepInEx;
using UnityEngine;
using ComputerPlusPlus;

[BepInPlugin("com.yourname.gorillatag.yourpluginname", "YourPluginName", "1.0.0")]
[BepInDependency("com.kylethescientist.gorillatag.computerplusplus")]
public class Plugin : BaseUnityPlugin { } // This is required so BepInEx knows to load the plugin

public class TestScreen : IScreen
{
    public string Title => "Test";

    public string Description => "Press [Option 1] for something to happen.";

    bool somethingHappened = false;

    // This is called every frame while the screen is active
    public string GetContent()
    {
        if (somethingHappened)
            return "Something happened!";
        else
            return "";
    }

    // This is called whenever a key is pressed while the screen is active
    public void OnKeyPressed(GorillaKeyboardButton button)
    {
        if (button.characterString == "option1")
            somethingHappened = true;
    }

    // This is called when the screen is registered
    public void Start() { }
}
```


## Important notes
While *Computer++* is designed to be safe to use in all lobbies, it is important to note that using mods in any form in public lobbies can result in a ban from the game. Use *Computer++* at your own risk.

## Bugs and issues
If you encounter any bugs or issues while using *Computer++*, please report them on the mod's GitHub page or in the discord server linked above. I will do my best to address them as soon as possible. 

## Contributing
If you would like to contribute to *Computer++*, feel free to submit a pull request on the mod's GitHub page. I welcome any and all contributions that can help make the mod better for everyone.

## Credits
*Computer++* was created by KyleTheScientist. 
