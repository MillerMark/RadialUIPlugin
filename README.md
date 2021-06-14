# Radial UI Plugin

This is a plugin for TaleSpire using BepInEx.

## Install

Currently you need to either follow the build guide down below or use the R2ModMan. 

## Usage
This plugin is specifically for developers to easily implement extra Radial buttons based on a entity properties.
Developers should reference the DLL for their own projects. This does not provide anything out of the box.

## Example Usage
```csharp
	void Awake()
        {

	    // Adds Callbacks to append new mapmenu item
            AddOnCharacter(Guid, new MapMenu.ItemArgs
            {
                Action = Action,
                Title = "On Character",
                CloseMenuOnActivate = true
            }); // Callback comparator is optional

            AddOnCanAttack(Guid, new MapMenu.ItemArgs
            {
                Action = Action,
                Title = "On Can Attack",
                CloseMenuOnActivate = true
            }, Check);

            AddOnCantAttack(Guid, new MapMenu.ItemArgs
            {
                Action = Action,
                Title = "On Cant Attack",
                CloseMenuOnActivate = true
            }, Check);

            AddOnHideVolume(Guid, new MapMenu.ItemArgs
            {
                Action = Action,
                Title = "On HideVolume",
                CloseMenuOnActivate = true
            }, Check2);
        }

        private Boolean Check(NGuid args, NGuid args2)
        {
            Debug.Log($"{args},{args2}");
            return true;
        }

        private Boolean Check2(HideVolumeItem args2)
        {
            Debug.Log($"{args2}");
            return true;
        }

        private void Action(MapMenuItem args, object args2) => Debug.Log($"{args},{args2}");

```


## How to Compile / Modify

Open ```RadialUIPlugin.sln``` in Visual Studio.

You will need to add references to:

```
* BepInEx.dll  (Download from the BepInEx project.)
* Bouncyrock.TaleSpire.Runtime (found in Steam\steamapps\common\TaleSpire\TaleSpire_Data\Managed)
* UnityEngine.dll
* UnityEngine.CoreModule.dll
* UnityEngine.InputLegacyModule.dll 
* UnityEngine.UI
* Unity.TextMeshPro
```

Build the project.

Browse to the newly created ```bin/Debug``` or ```bin/Release``` folders and copy the ```RadialUIPlugin.dll``` to ```Steam\steamapps\common\TaleSpire\BepInEx\plugins```
