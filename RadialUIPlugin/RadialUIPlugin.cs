﻿using BepInEx;
using Bounce.Unmanaged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LordAshes;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

namespace RadialUI
{

    [BepInPlugin(Guid, "RadialUIPlugin", Version)]
    public class RadialUIPlugin : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.hollofox.plugins.RadialUIPlugin";
        private const string Version = "1.4.1.0";

        /// <summary>
        /// Awake plugin
        /// </summary>
        void Awake()
        {
            Logger.LogInfo("In Awake for RadialUI");

            Debug.Log("RadialUI Plug-in loaded");
        }


        // Character Related Add
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid,NGuid, bool>)> _onCharacterCallback = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onCanAttack = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onCantAttack = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuEmotes = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuKill = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuGm = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuAttacks = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuSize = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();

        // Character Related Remove
        private static readonly Dictionary<string, List<string>> _removeOnCharacter = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnCanAttack = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnCantAttack = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnSubmenuEmotes = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnSubmenuKill = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnSubmenuGm = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnSubmenuAttacks = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnSubmenuSize = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, List<string>> _removeOnHideVolume = new Dictionary<string, List<string>>();

        // Hide Volumes
        private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<HideVolumeItem,bool>)> _onHideVolumeCallback = new Dictionary<string, (MapMenu.ItemArgs, Func<HideVolumeItem,bool>)>();

        // Add On Character
        public static void AddOnCharacter(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onCharacterCallback.Add(key,(value, externalCheck));
        public static void AddOnCanAttack(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onCanAttack.Add(key,(value, externalCheck));
        public static void AddOnCantAttack(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onCantAttack.Add(key,(value, externalCheck));
        public static void AddOnSubmenuEmotes(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuEmotes.Add(key,(value, externalCheck));
        public static void AddOnSubmenuKill(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuKill.Add(key,(value, externalCheck));
        public static void AddOnSubmenuGm(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuGm.Add(key,(value, externalCheck));
        public static void AddOnSubmenuAttacks(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuAttacks.Add(key,(value, externalCheck));
        public static void AddOnSubmenuSize(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuSize.Add(key,(value, externalCheck));
        public static void AddOnStatMenu(string key, StatItemArgs value, Func<NGuid, bool>  externalCheck = null) => _onCharStatMenu.Add(key,(value, externalCheck));

        // Add On HideVolume
        public static void AddOnHideVolume(string key, MapMenu.ItemArgs value, Func<HideVolumeItem,bool> externalCheck = null) => _onHideVolumeCallback.Add(key, (value, externalCheck));

        // Remove On Character
        public static bool RemoveOnCharacter(string key) => _onCharacterCallback.Remove(key);
        public static bool RemoveOnCanAttack(string key) => _onCanAttack.Remove(key);
        public static bool RemoveOnCantAttack(string key) => _onCantAttack.Remove(key);
        public static bool RemoveOnSubmenuEmotes(string key) => _onSubmenuEmotes.Remove(key);
        public static bool RemoveOnSubmenuKill(string key) => _onSubmenuKill.Remove(key);
        public static bool RemoveOnSubmenuGm(string key) => _onSubmenuGm.Remove(key);
        public static bool RemoveOnSubmenuAttacks(string key) => _onSubmenuAttacks.Remove(key);
        public static bool RemoveOnSubmenuSize(string key) => _onSubmenuSize.Remove(key);
        public static bool RemoveOnCharStatMenu(string key) => _onCharStatMenu.Remove(key);

        // Add RemoveOn
        public static void AddOnRemoveSubmenuAttacks(string key, string value) => AddRemoveOn(_removeOnSubmenuAttacks,key,value);
        public static void AddOnRemoveCanAttack(string key, string value) => AddRemoveOn(_removeOnCanAttack,key,value);
        public static void AddOnRemoveCantAttack(string key, string value) => AddRemoveOn(_removeOnCantAttack,key,value);
        public static void AddOnRemoveCharacter(string key, string value) => AddRemoveOn(_removeOnCharacter,key,value);
        public static void AddOnRemoveHideVolume(string key, string value) => AddRemoveOn(_removeOnHideVolume,key,value);
        public static void AddOnRemoveSubmenuEmotes(string key, string value) => AddRemoveOn(_removeOnSubmenuEmotes,key,value);
        public static void AddOnRemoveSubmenuGm(string key, string value) => AddRemoveOn(_removeOnSubmenuGm,key,value);
        public static void AddOnRemoveSubmenuKill(string key, string value) => AddRemoveOn(_removeOnSubmenuKill,key,value);
        public static void AddOnRemoveSubmenuSize(string key, string value) => AddRemoveOn(_removeOnSubmenuSize,key,value);

        // Remove RemoveOn
        public static void RemoveOnRemoveSubmenuAttacks(string key, string value) => RemoveRemoveOn(_removeOnSubmenuAttacks, key, value);
        public static void RemoveOnRemoveCanAttack(string key, string value) => RemoveRemoveOn(_removeOnCanAttack, key, value);
        public static void RemoveOnRemoveCantAttack(string key, string value) => RemoveRemoveOn(_removeOnCantAttack, key, value);
        public static void RemoveOnRemoveCharacter(string key, string value) => RemoveRemoveOn(_removeOnCharacter, key, value);
        public static void RemoveOnRemoveHideVolume(string key, string value) => RemoveRemoveOn(_removeOnHideVolume, key, value);
        public static void RemoveOnRemoveSubmenuEmotes(string key, string value) => RemoveRemoveOn(_removeOnSubmenuEmotes, key, value);
        public static void RemoveOnRemoveSubmenuGm(string key, string value) => RemoveRemoveOn(_removeOnSubmenuGm, key, value);
        public static void RemoveOnRemoveSubmenuKill(string key, string value) => RemoveRemoveOn(_removeOnSubmenuKill, key, value);
        public static void RemoveOnRemoveSubmenuSize(string key, string value) => RemoveRemoveOn(_removeOnSubmenuSize, key, value);

        private static void AddRemoveOn(Dictionary<string, List<string>> data, string key, string value)
        {
            if (!data.ContainsKey(key)) data.Add(key, new List<string>());
            data[key].Add(value);
        }

        private static bool RemoveRemoveOn(Dictionary<string, List<string>> data, string key, string value)
        {
            return data.ContainsKey(key) && data[key].Remove(value);
        }

        // Remove On HideVolume
        public static bool RemoveOnHideVolume(string key) => _onHideVolumeCallback.Remove(key);

        // Check to see if map menu is new
        private static int last = 0;
        private static string lastTitle = "";
        private static bool lastSuccess = true;
        private static DateTime Execute;

        // Last Creature/HideVolume
        private static NGuid target;
        private static HideVolumeItem lastHideVolumeItem;

        private (Action<MapMenuItem, Object>, MapMenuItem, Object) pending = (null,null,null);

        private bool ready = true;

        /// <summary>
        /// Looping method run by plugin
        /// </summary>
        void Update()
        {
            if (ready)
            {
                ready = false;
                if (MapMenuManager.HasInstance && MapMenuManager.IsOpen)
                {

                    var instance = MapMenuManager.Instance;
                    var type = instance.GetType();

                    var field = type.GetField("_allOpenMenus", bindFlags);
                    var menus = (List<MapMenu>) field.GetValue(instance);

                    var title = "";
                    if (menus.Count >= 1 && menus.Count >= last)
                    {
                        var map = menus[menus.Count - 1];
                        var Map = map.transform.GetChild(0).GetChild(0);

                        var foundComp = Map.TryGetComponent(out MapMenuItem mapComponents);
                        var foundStat = Map.TryGetComponent(out MapMenuStatItem mapStatComponents);

                        if (foundComp && !foundStat)
                        {
                            var mapComponent = mapComponents;
                            var mapField = mapComponent.GetType().GetField("_title", bindFlags);
                            title = (string) mapField.GetValue(mapComponent);
                        }

                        if (foundStat)
                        {
                            title = "Creature Stat";
                        }

                        if (menus.Count == last && lastTitle != title) last -= 1;
                        if (foundComp || foundStat) Probe();
                        lastTitle = title;
                        lastSuccess = true;
                    }

                    last = menus.Count;

                    OnStatChange(title);
                }
                else
                {
                    last = 0;
                    lastTitle = "";
                    lastSuccess = true;
                    ClearStatMenus();
                }

                if (pending.Item1 != null && Execute != DateTime.MinValue && DateTime.Now > Execute)
                {
                    pending.Item1(pending.Item2, pending.Item3);
                    pending = (null, null, null);
                    Execute = DateTime.MinValue;
                }
                ready = true;
            }
        }

        private const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        private void Probe()
        { 
            var instance = MapMenuManager.Instance;
            var type = instance.GetType();
            
            var field = type.GetField("_allOpenMenus", bindFlags);
            var menus = (List<MapMenu>) field.GetValue(instance);

            for(var i = last; i < menus.Count; i++)
            {
                var map = menus[i];
                var Map = map.transform.GetChild(0).GetChild(0);
                // Debug.Log("Found Map");

                var foundComp = Map.TryGetComponent(out MapMenuItem mapComponents);
                var foundStat = Map.TryGetComponent(out MapMenuStatItem mapStatComponents);
                // Debug.Log("Fetch Components");

                if (foundComp && !foundStat)
                {
                    var mapComponent = mapComponents;
                    var mapField = mapComponent.GetType().GetField("_title", bindFlags);
                    var title = (string) mapField.GetValue(mapComponent);

                    var id = LocalClient.SelectedCreatureId.Value;

                    Debug.Log(title);

                // Minis Related
                if (IsMini(title)) RemoveRadialComponent(_removeOnCharacter, map);
                if (CanAttack(title)) RemoveRadialComponent(_removeOnCanAttack, map);
                if (CanNotAttack(title)) RemoveRadialComponent(_removeOnCantAttack, map);
                
                if (IsMini(title)) AddCreatureEvent(_onCharacterCallback,id,map);
                if (CanAttack(title)) AddCreatureEvent(_onCanAttack, id, map);
                if (CanNotAttack(title)) AddCreatureEvent(_onCantAttack, id, map);

                // Minis Submenu
                if (IsEmotes(title)) RemoveRadialComponent(_removeOnSubmenuEmotes, map);
                if (IsKill(title)) RemoveRadialComponent(_removeOnSubmenuKill, map);
                if (IsGmMenu(title)) RemoveRadialComponent(_removeOnSubmenuGm, map);
                if (IsAttacksMenu(title)) RemoveRadialComponent(_removeOnSubmenuAttacks, map);
                if (IsSizeMenu(title)) RemoveRadialComponent(_removeOnSubmenuSize, map);

                if (IsEmotes(title)) AddCreatureEvent(_onSubmenuEmotes, id, map);
                if (IsKill(title)) AddCreatureEvent(_onSubmenuKill, id, map);
                if (IsGmMenu(title)) AddCreatureEvent(_onSubmenuGm, id, map);
                if (IsAttacksMenu(title)) AddCreatureEvent(_onSubmenuAttacks, id, map);
                if (IsSizeMenu(title)) AddCreatureEvent(_onSubmenuSize, id, map);

                // Hide Volumes
                if (IsHideVolume(title)) RemoveRadialComponent(_removeOnHideVolume, map);
                if (IsHideVolume(title)) AddHideVolumeEvent(_onHideVolumeCallback, map);
                    // Hide Volumes
                    if (IsHideVolume(title)) AddHideVolumeEvent(_onHideVolumeCallback, map);
                } else if (foundStat)
                {
                    // Debug.Log("Found Stat instead");
                    AddStatCreatureEvent(map);
                }
            }
        }

        private static void RemoveRadialComponent(Dictionary<string, List<string>> removeOnCharacter, MapMenu map)
        {
            var indexes = removeOnCharacter.SelectMany(i => i.Value).Distinct();
            foreach (var index in indexes) Debug.Log(index);
            foreach (var index in indexes)
            {
                var Map = map.transform.GetChild(0);
                for (var i = 0; i < Map.childCount; i++)
                {
                    var mapComponent = Map.GetChild(i).GetComponent<MapMenuItem>();
                    var mapField = mapComponent.GetType().GetField("_title", bindFlags);
                    var title = (string)mapField.GetValue(mapComponent);
                    if (title != index) continue;
                    Debug.Log($"found: {index}");
                    Map.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        private void AddCreatureEvent(Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> dic, NGuid myCreature, MapMenu map)
        {
            target = GetRadialTargetCreature();
            foreach (var handlers
                in dic.Values
                    .Where(handlers => handlers.Item2 == null
                                       || handlers.Item2(myCreature, target))) map.AddItem(
                new MapMenu.ItemArgs
                {
                    Title = handlers.Item1.Title,
                    Icon = handlers.Item1.Icon,
                    Action = (mmi, obj) =>
                    {
                        pending = (handlers.Item1.Action,mmi,obj);
                        Execute = DateTime.Now.AddMilliseconds(200);
                    },
                    CloseMenuOnActivate = handlers.Item1.CloseMenuOnActivate,
                    ValueText = handlers.Item1.ValueText,
                    SubValueText = handlers.Item1.SubValueText,
                    FadeName = handlers.Item1.FadeName,
                    Obj = handlers.Item1.Obj,
                }
            );
        }

        // Registered Value
        private static readonly Dictionary<string, (StatItemArgs, Func<NGuid, bool> check)> _onCharStatMenu = new Dictionary<string, (StatItemArgs, Func<NGuid, bool> check)>();
        
        // store current value
        private Dictionary<(NGuid,string), (StatItemArgs args, MapMenuStatItem item)> statItems = new Dictionary<(NGuid, string), (StatItemArgs args, MapMenuStatItem item)>();

        private void ClearStatMenus()
        {
            var keys = statItems.Keys.ToArray();
            foreach (var k in keys)
            {
                statItems[k] = (statItems[k].args,null);
            }
        }

        private void AddStatCreatureEvent(MapMenu map)
        {
            var target = GetRadialTargetCreature();
            var index = 4;
            
            foreach (var key in _onCharStatMenu.Keys)
            {
                var result = StatMessaging.ReadInfo(new CreatureGuid(target), key);
                
                if (result != "")
                {
                    StatMessaging.Subscribe(key,null);
                }
                else
                {

                }

                Debug.Log(key);
                if (_onCharStatMenu[key].check == null || _onCharStatMenu[key].check(target))
                {
                    var args = _onCharStatMenu[key].Item1;
                    if (!statItems.ContainsKey((target, key)))
                    {
                        statItems.Add((target, key), (args,null));
                    }

                    args = statItems[(target, key)].args;
                    Debug.Log(target);
                    
                    var item = map.AddStatItem(args, target, index);
                    statItems[(target, key)] = (args, item);
                    index++;
                }
            }
        }

        private void OnStatChange(string title)
        {
            if (!IsMini(title))
            {
                var keys = _onCharStatMenu.Keys.ToArray();

                foreach (var key in keys)
                {
                    Debug.Log(key);
                    var info = StatMessaging.ReadInfo(new CreatureGuid(GetRadialTargetCreature()), key);
                    
                    StatMessaging.SetInfo(new CreatureGuid(GetRadialTargetCreature()), key,);



                    if (statItems[key].item != null &&
                        statItems[key].args.Current != statItems[key].item.GetStatItem().Current)
                    {
                        var i = statItems[key].args;
                        Debug.Log(i.Current);
                        i.Current = statItems[key].item.GetStatItem().Current;
                        Debug.Log(i.Current);
                        statItems[key] = (i, statItems[key].item);
                        Debug.Log("On State Change");
                        _onCharStatMenu[key.Item2].Item1.Callback(statItems[key].item, key.Item1);
                    }
                }
            }
        }
        

        public static NGuid GetRadialTargetCreature()
        {
            var x = (CreatureMenuBoardTool)GameObject.FindObjectOfType(typeof(CreatureMenuBoardTool));

            FieldInfo mapField = x.GetType().GetField("_selectedCreature", bindFlags);
            var selectedCreature = (Creature)mapField.GetValue(x);
            return selectedCreature.CreatureId.Value;
        }

        /// <summary>
        /// Fetches the last creature the menu has been open on (menu open or closed)
        /// </summary>
        /// <returns>NGuid for the last creature that the Radial Menu has been opened on.</returns>
        public static NGuid GetLastRadialTargetCreature()
        {
            return target;
        }

        /// <summary>
        /// Fetches the last creature the menu has been open on (menu open or closed)
        /// </summary>
        /// <returns>NGuid for the last creature that the Radial Menu has been opened on.</returns>
        public static HideVolumeItem GetLastRadialHideVolume()
        {
            return lastHideVolumeItem;
        }

        private void AddHideVolumeEvent(Dictionary<string, (MapMenu.ItemArgs, Func<HideVolumeItem, bool>)> dic, MapMenu map)
        {
            lastHideVolumeItem = GetSelectedHideVolumeItem();
            foreach (var handlers
                in dic.Values
                    .Where(handlers => handlers.Item2 == null
                                       || handlers.Item2(lastHideVolumeItem))) map.AddItem(handlers.Item1);
        }

        private static HideVolumeItem GetSelectedHideVolumeItem()
        {
            var tool = (GMHideVolumeMenuBoardTool)GameObject.FindObjectOfType(typeof(GMHideVolumeMenuBoardTool));
            FieldInfo mapField = tool.GetType().GetField("_selectedVolume", bindFlags);
            return (HideVolumeItem) mapField.GetValue(tool);
        }


        // Current ShortHand to see if Mini
        private bool IsMini(string title) => title == "Emotes" || title == "Attacks";
        private bool CanAttack(string title) => title == "Attacks";
        private bool CanNotAttack(string title) => title == "Emotes";

        // Mini Submenus
        private bool IsEmotes(string title) => title == "Twirl";
        private bool IsKill(string title) => title == "Kill Creature";
        private bool IsGmMenu(string title) => title == "Player Permission";
        private bool IsAttacksMenu(string title) => title == "Attack";
        private bool IsSizeMenu(string title) => title == "0.5x0.5";

        // Current ShortHand to see if HideVolume
        private bool IsHideVolume(string title) => title == "Toggle Visibility";
    }
}
