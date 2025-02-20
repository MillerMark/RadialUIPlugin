﻿using BepInEx;
using Bounce.Unmanaged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace RadialUI
{

	[BepInPlugin(Guid, "RadialUIPlugin", Version)]
	public class RadialUIPlugin : BaseUnityPlugin
	{
		// constants
		public const string Guid = "org.hollofox.plugins.RadialUIPlugin";
		public const string Version = "1.5.0.0";

		/// <summary>
		/// Awake plugin
		/// </summary>
		void Awake()
		{
			Logger.LogInfo("In Awake for RadialUI");

			Debug.Log("RadialUI Plug-in loaded");
		}


		// Character Related Add
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onCharacterCallback = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onCanAttack = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onCantAttack = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuEmotes = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuKill = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuGm = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuAttacks = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)> _onSubmenuSize = new Dictionary<string, (MapMenu.ItemArgs, Func<NGuid, NGuid, bool>)>();

		// Character Related Remove
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnCharacter = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnCanAttack = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnCantAttack = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnSubmenuEmotes = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnSubmenuKill = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnSubmenuGm = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnSubmenuAttacks = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnSubmenuSize = new Dictionary<string, List<RadialCheckRemove>>();
		private static readonly Dictionary<string, List<RadialCheckRemove>> _removeOnHideVolume = new Dictionary<string, List<RadialCheckRemove>>();

		// Hide Volumes
		private static readonly Dictionary<string, (MapMenu.ItemArgs, Func<HideVolumeItem, bool>)> _onHideVolumeCallback = new Dictionary<string, (MapMenu.ItemArgs, Func<HideVolumeItem, bool>)>();

		// Add On Character
		[Obsolete("Use RegisterAddCharacter instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnCharacter(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onCharacterCallback.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddCanAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnCanAttack(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onCanAttack.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddCantAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnCantAttack(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onCantAttack.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddSubmenuEmotes instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnSubmenuEmotes(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuEmotes.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddSubmenuKill instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnSubmenuKill(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuKill.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddSubmenuGm instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnSubmenuGm(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuGm.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddSubmenuAttacks instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnSubmenuAttacks(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuAttacks.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddSubmenuSize instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnSubmenuSize(string key, MapMenu.ItemArgs value, Func<NGuid, NGuid, bool> externalCheck = null) => _onSubmenuSize.Add(key, (value, externalCheck));
		[Obsolete("Use RegisterAddHideVolume instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnHideVolume(string key, MapMenu.ItemArgs value, Func<HideVolumeItem, bool> externalCheck = null) => _onHideVolumeCallback.Add(key, (value, externalCheck));

		// Remove On Character
		[Obsolete("Use UnregisterAddCharacter instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnCharacter(string key) => _onCharacterCallback.Remove(key);
		[Obsolete("Use UnregisterAddCanAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnCanAttack(string key) => _onCanAttack.Remove(key);
		[Obsolete("Use UnregisterAddCantAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnCantAttack(string key) => _onCantAttack.Remove(key);
		[Obsolete("Use UnregisterAddSubmenuEmotes instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnSubmenuEmotes(string key) => _onSubmenuEmotes.Remove(key);
		[Obsolete("Use UnregisterAddSubmenuKill instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnSubmenuKill(string key) => _onSubmenuKill.Remove(key);
		[Obsolete("Use UnregisterAddSubmenuGm instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnSubmenuGm(string key) => _onSubmenuGm.Remove(key);
		[Obsolete("Use UnregisterAddSubmenuAttacks instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnSubmenuAttacks(string key) => _onSubmenuAttacks.Remove(key);
		[Obsolete("Use UnregisterAddSubmenuSize instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool RemoveOnSubmenuSize(string key) => _onSubmenuSize.Remove(key);

		/// <summary>
		/// Registers a new menu item to add to the Character menu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddCharacter(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onCharacterCallback.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the CanAttack menu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddCanAttack(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onCanAttack.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the CantAttack menu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddCantAttack(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onCantAttack.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the Emotes submenu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddSubmenuEmotes(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onSubmenuEmotes.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the Kill submenu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddSubmenuKill(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onSubmenuKill.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the GM Tools submenu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddSubmenuGm(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onSubmenuGm.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the Attacks submenu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddSubmenuAttacks(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onSubmenuAttacks.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the Size submenu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddSubmenuSize(MapMenu.ItemArgs itemArgs, Func<NGuid, NGuid, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onSubmenuSize.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Registers a new menu item to add to the HideVolume menu.
		/// </summary>
		/// <param name="itemArgs">The MapMenu.ItemArgs of the menu to add (includes a callback for when this new menu item is selected).</param>
		/// <param name="externalCheck">An optional callback to dynamically determine when you want to add the menu.</param>
		/// <returns>Returns the commandId for this menu item add command. Useful if you later want to unregister this command.</returns>
		public static string RegisterAddHideVolume(MapMenu.ItemArgs itemArgs, Func<HideVolumeItem, bool> externalCheck = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			_onHideVolumeCallback.Add(commandId, (itemArgs, externalCheck));
			return commandId;
		}

		/// <summary>
		/// Unregisters the specified Add command from the Character menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddCharacter(string commandId) => _onCharacterCallback.Remove(commandId);

		/// <summary>
		/// Unregisters the specified Add command from the CanAttack menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddCanAttack(string commandId) => _onCanAttack.Remove(commandId);

		/// <summary>
		/// Unregisters the specified Add command from the CantAttack menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddCantAttack(string commandId) => _onCantAttack.Remove(commandId);

		/// <summary>
		/// Unregisters the specified Add command from the Emotes submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddSubmenuEmotes(string commandId) => _onSubmenuEmotes.Remove(commandId);

		/// <summary>
		/// Unregisters the specified Add command from the Kill submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddSubmenuKill(string commandId) => _onSubmenuKill.Remove(commandId);

		/// <summary>
		/// Unregisters the specified Add command from the GM Tools submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddSubmenuGm(string commandId) => _onSubmenuGm.Remove(commandId);

		/// <summary>
		/// Unregisters the specified Add command from the Attacks submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddSubmenuAttacks(string commandId) => _onSubmenuAttacks.Remove(commandId);

		/// <summary>
		/// Unregisters the specified Add command from the Size menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		public static bool UnregisterAddSubmenuSize(string commandId) => _onSubmenuSize.Remove(commandId);



		// Add RemoveOn
		[Obsolete("Use RegisterRemoveSubmenuAttacks instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveSubmenuAttacks(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnSubmenuAttacks, key, value, callback);
		[Obsolete("Use RegisterRemoveCanAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveCanAttack(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnCanAttack, key, value, callback);
		[Obsolete("Use RegisterRemoveCantAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveCantAttack(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnCantAttack, key, value, callback);
		[Obsolete("Use RegisterRemoveCharacter instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveCharacter(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnCharacter, key, value, callback);
		[Obsolete("Use RegisterRemoveHideVolume instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveHideVolume(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnHideVolume, key, value, callback);
		[Obsolete("Use RegisterRemoveSubmenuEmotes instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveSubmenuEmotes(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnSubmenuEmotes, key, value, callback);
		[Obsolete("Use RegisterRemoveSubmenuGm instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveSubmenuGm(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnSubmenuGm, key, value, callback);
		[Obsolete("Use RegisterRemoveSubmenuKill instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveSubmenuKill(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnSubmenuKill, key, value, callback);
		[Obsolete("Use RegisterRemoveSubmenuSize instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void AddOnRemoveSubmenuSize(string key, string value, ShouldShowMenu callback = null) => AddRemoveOn(_removeOnSubmenuSize, key, value, callback);


		/// <summary>
		/// Registers a menu item for removal from the Attacks submenu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveSubmenuAttacks(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnSubmenuAttacks, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the CanAttack menu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveCanAttack(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnCanAttack, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the CantAttack menu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveCantAttack(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnCantAttack, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the Character menu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveCharacter(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnCharacter, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the HideVolume menu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveHideVolume(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnHideVolume, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the Emotes submenu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveSubmenuEmotes(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnSubmenuEmotes, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the GM Tools submenu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveSubmenuGm(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnSubmenuGm, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the Kill submenu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveSubmenuKill(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnSubmenuKill, commandId, menuTitle, callback);
			return commandId;
		}

		/// <summary>
		/// Registers a menu item for removal from the Size submenu.
		/// </summary>
		/// <param name="menuTitle">The title of the menu to remove.</param>
		/// <param name="callback">An optional callback to dynamically determine when you want to remove the menu.</param>
		/// <returns>Returns the commandId for this menu item removal command. Useful if you later want to unregister this command.</returns>
		public static string RegisterRemoveSubmenuSize(string menuTitle, ShouldShowMenu callback = null)
		{
			string commandId = System.Guid.NewGuid().ToString();
			AddRemoveOn(_removeOnSubmenuSize, commandId, menuTitle, callback);
			return commandId;
		}


		// Remove RemoveOn
		[Obsolete("Use UnregisterRemoveSubmenuAttacks instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveSubmenuAttacks(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnSubmenuAttacks, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveCanAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveCanAttack(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnCanAttack, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveCantAttack instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveCantAttack(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnCantAttack, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveCharacter instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveCharacter(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnCharacter, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveHideVolume instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveHideVolume(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnHideVolume, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveSubmenuEmotes instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveSubmenuEmotes(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnSubmenuEmotes, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveSubmenuGm instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveSubmenuGm(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnSubmenuGm, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveSubmenuKill instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveSubmenuKill(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnSubmenuKill, commandId, menuTitle);
		[Obsolete("Use UnregisterRemoveSubmenuSize instead.")] 
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RemoveOnRemoveSubmenuSize(string commandId, string menuTitle) => RemoveRemoveOn(_removeOnSubmenuSize, commandId, menuTitle);


		/// <summary>
		/// Unregisters the specified Remove command from the Attacks submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveSubmenuAttacks(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnSubmenuAttacks, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the CanAttack menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveCanAttack(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnCanAttack, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the CantAttack menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveCantAttack(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnCantAttack, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the Character menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveCharacter(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnCharacter, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the HideVolume menu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveHideVolume(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnHideVolume, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the Emotes submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveSubmenuEmotes(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnSubmenuEmotes, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the GM Tools submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveSubmenuGm(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnSubmenuGm, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the Kill submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveSubmenuKill(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnSubmenuKill, commandId, menuTitle);
		}

		/// <summary>
		/// Unregisters the specified Remove command from the Size submenu.
		/// </summary>
		/// <param name="commandId">The commandId to remove.</param>
		/// <param name="menuTitle">An optional title of the menu to remove.</param>
		public static void UnregisterRemoveSubmenuSize(string commandId, string menuTitle = null)
		{
			RemoveRemoveOn(_removeOnSubmenuSize, commandId, menuTitle);
		}


		private static void AddRemoveOn(Dictionary<string, List<RadialCheckRemove>> data, string key, string value, ShouldShowMenu shouldRemoveCallback)
		{
			if (!data.ContainsKey(key))
				data.Add(key, new List<RadialCheckRemove>());
			data[key].Add(new RadialCheckRemove(value, shouldRemoveCallback));
		}

		private static bool RemoveRemoveOn(Dictionary<string, List<RadialCheckRemove>> data, string key, string value = null)
		{
			if (!data.ContainsKey(key))
				return false;
			List<RadialCheckRemove> radialCheckRemoves = data[key];
			int countBefore = radialCheckRemoves.Count;
			radialCheckRemoves.RemoveAll(x => value == null || x.TitleToRemove == value);  // Simply remove everthing if value is null.
			bool successfullyRemoved = radialCheckRemoves.Count != countBefore;
			if (radialCheckRemoves.Count == 0)
				data.Remove(key);
			return successfullyRemoved;
		}

		// Remove On HideVolume
		public static bool RemoveOnHideVolume(string key) => _onHideVolumeCallback.Remove(key);

		// Check to see if map menu is new
		private static int lastMenuCount = 0;
		private static string lastTitle = "";
		private static bool lastSuccess = true;
		private static DateTime menuExecutionTime;

		// Last Creature/HideVolume
		private static NGuid target;
		private static HideVolumeItem lastHideVolumeItem;

		private (Action<MapMenuItem, Object>, MapMenuItem, Object) pendingMenuAction = (null, null, null);

		/// <summary>
		/// Looping method run by plugin
		/// </summary>
		void Update()
		{
			ExecutePendingMenuActionIfNecessary();

			if (!MapMenuManager.HasInstance || !MapMenuManager.IsOpen)
			{
				if (needToClear)
				{
					needToClear = false;
					lastMenuCount = 0;
					lastTitle = "";
					lastSuccess = true;
					ClearPreviouslyModded();
				}
				return;
			}

			needToClear = true;
			List<MapMenu> menus = Talespire.RadialMenus.GetAllOpen();

			if (menus.Count >= 1 && menus.Count >= lastMenuCount)
			{
				try
				{
					string title = menus[menus.Count - 1].GetTitle();

					if (menus.Count == lastMenuCount && lastTitle != title)
						lastMenuCount -= 1;

					ModifyMenus();
					lastTitle = title;
					lastSuccess = true;
				}
				catch (Exception e)
				{
					// Probably Stat open
					if (lastSuccess)
					{
						Debug.Log($"Error: {e}, most likely stat submenu being opened");
						lastSuccess = false;
					}
				}
			}
			lastMenuCount = menus.Count;
		}

		private void ExecutePendingMenuActionIfNecessary()
		{
			if (pendingMenuAction.Item1 == null || menuExecutionTime == DateTime.MinValue || DateTime.Now <= menuExecutionTime)
				return;

			pendingMenuAction.Item1(pendingMenuAction.Item2, pendingMenuAction.Item3);
			pendingMenuAction = (null, null, null);
			menuExecutionTime = DateTime.MinValue;
		}

		static Vector3 RemoveY(Vector3 position)
		{
			return new Vector3(position.x, 0, position.z);
		}

		static float GetDistanceToMenuPosition(Vector3 position, CreatureBoardAsset creatureBoardAsset)
		{
			Vector3 flatPosition = RemoveY(position);
			float distanceToPlacedPosition = (flatPosition - RemoveY(creatureBoardAsset.PlacedPosition)).magnitude;
			if (creatureBoardAsset.IsFlying)
			{
				Vector3 flyingPosition = creatureBoardAsset.PlacedPosition - new Vector3(0, creatureBoardAsset.FlyingIndicator.ElevationAmount, 0);
				float distanceToFlyingPosition = (RemoveY(flyingPosition) - flatPosition).magnitude;
				return Mathf.Min(distanceToFlyingPosition, distanceToPlacedPosition);
			}

			return distanceToPlacedPosition;
		}

		/// <summary>
		/// Gets the creature closest to the specified menu point.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="maxRadiusTiles"></param>
		/// <returns></returns>
		public static CreatureBoardAsset GetCreatureClosestTo(Vector3 position, float maxRadiusTiles = 0.5f)
		{
			CreatureBoardAsset closestCreature = null;
			float shortestDistanceSoFar = float.MaxValue;
			CreatureBoardAsset[] allCreatureAssets = CreaturePresenter.AllCreatureAssets.ToArray();
			if (allCreatureAssets == null)
				return null;
			foreach (CreatureBoardAsset creatureBoardAsset in allCreatureAssets)
			{
				float distance = GetDistanceToMenuPosition(position, creatureBoardAsset);
				if (distance < shortestDistanceSoFar)
				{
					shortestDistanceSoFar = distance;
					closestCreature = creatureBoardAsset;
				}
			}
			if (shortestDistanceSoFar > maxRadiusTiles)
				return null;

			return closestCreature;
		}


		Vector3 GetMenuPosition()
		{
			FieldInfo positionField = typeof(MapMenuManager).GetField("_position", BindingFlags.NonPublic | BindingFlags.Instance);
			if (positionField != null)
			{
				return (Vector3)positionField.GetValue(MapMenuManager.Instance);
			}
			return Vector3.zero;
		}

		private void ModifyMenus()
		{
			List<MapMenu> menus = Talespire.RadialMenus.GetAllOpen();

			//NGuid selectedCreatureId = LocalClient.SelectedCreatureId.Value;

			//Debug.LogWarning($"selectedCreatureId = {selectedCreatureId}");

			Vector3 menuPosition = GetMenuPosition();
			//DebugVector("Menu position", menuPosition);
			CreatureAtMenu = GetCreatureClosestTo(menuPosition, 2f);
			NGuid rightClickCreatureId = NGuid.Empty;
			if (CreatureAtMenu != null)
				rightClickCreatureId = CreatureAtMenu.CreatureId.Value;

			for (var i = lastMenuCount; i < menus.Count; i++)
			{
				//Debug.LogWarning($"selectedCreatureId = {rightClickCreatureId}");
				MapMenu mapMenu = menus[i];

				string title = mapMenu.GetTitle();
				Debug.Log(title);

				// Minis Related
				if (IsMini(title)) AddMenuItem(_onCharacterCallback, rightClickCreatureId, mapMenu);
				if (CanAttack(title)) AddMenuItem(_onCanAttack, rightClickCreatureId, mapMenu);
				if (CanNotAttack(title)) AddMenuItem(_onCantAttack, rightClickCreatureId, mapMenu);

				if (IsMini(title)) RemoveMenuItem(_removeOnCharacter, rightClickCreatureId, mapMenu);
				if (CanAttack(title)) RemoveMenuItem(_removeOnCanAttack, rightClickCreatureId, mapMenu);
				if (CanNotAttack(title)) RemoveMenuItem(_removeOnCantAttack, rightClickCreatureId, mapMenu);

				// Minis Submenu
				if (IsEmotes(title)) AddMenuItem(_onSubmenuEmotes, rightClickCreatureId, mapMenu);
				if (IsKill(title)) AddMenuItem(_onSubmenuKill, rightClickCreatureId, mapMenu);
				if (IsGmMenu(title)) AddMenuItem(_onSubmenuGm, rightClickCreatureId, mapMenu);
				if (IsAttacksMenu(title)) AddMenuItem(_onSubmenuAttacks, rightClickCreatureId, mapMenu);
				if (IsSizeMenu(title)) AddMenuItem(_onSubmenuSize, rightClickCreatureId, mapMenu);

				if (IsEmotes(title)) RemoveMenuItem(_removeOnSubmenuEmotes, rightClickCreatureId, mapMenu);
				if (IsKill(title)) RemoveMenuItem(_removeOnSubmenuKill, rightClickCreatureId, mapMenu);
				if (IsGmMenu(title)) RemoveMenuItem(_removeOnSubmenuGm, rightClickCreatureId, mapMenu);
				if (IsAttacksMenu(title)) RemoveMenuItem(_removeOnSubmenuAttacks, rightClickCreatureId, mapMenu);
				if (IsSizeMenu(title)) RemoveMenuItem(_removeOnSubmenuSize, rightClickCreatureId, mapMenu);

				// Hide Volumes
				if (IsHideVolume(title)) RemoveMenuItem(_removeOnHideVolume, rightClickCreatureId, mapMenu);
				if (IsHideVolume(title)) AddHideVolumeEvent(_onHideVolumeCallback, mapMenu);
			}
		}

		private static void DebugVector(string label, Vector3 menuPosition)
		{
			Debug.LogWarning($"{label}: {menuPosition.x}, {menuPosition.y}, {menuPosition.z}");
		}

		private static void RemoveMenuItem(Dictionary<string, List<RadialCheckRemove>> removeOnCharacter, NGuid miniAtMenu, MapMenu map)
		{
			target = Talespire.RadialMenus.GetTargetCreatureId();
			IEnumerable<string> indexes = removeOnCharacter.SelectMany(i => i.Value.Select(x => x.TitleToRemove)).Distinct();
			Debug.Log("We expect to be removing these titles (perhaps conditionally):");
			foreach (string index in indexes)
				Debug.Log("  " + index);

			Transform Map = map.transform.GetChild(0);
			//Debug.Log($"Map.childCount = {Map.childCount}");
			try
			{
				for (int i = Map.childCount - 1; i >= 0; i--)
				{
					Transform transform = Map.GetChild(i);
					string menuTitle = GetMenuTitle(transform);
					//Debug.Log($"Map.GetChild({i}) = \"{menuTitle}\"");
					//Debug.Log($"removeOnCharacter.Keys.Count = {removeOnCharacter.Keys.Count}");

					bool found = false;
					foreach (string key in removeOnCharacter.Keys)
					{
						foreach (RadialCheckRemove radialCheckRemove in removeOnCharacter[key])
						{
							if (radialCheckRemove.TitleToRemove == menuTitle)
							{
								//Debug.LogWarning($"Found a match: ({radialCheckRemove.TitleToRemove})");

								//if (radialCheckRemove.ShouldRemoveCallback == null)
								//	Debug.LogWarning("radialCheckRemove.ShouldRemoveCallback == null");

								if (radialCheckRemove.ShouldRemoveCallback == null || TriggerShouldRemoveCallback(miniAtMenu, menuTitle, radialCheckRemove))
								{
									//Debug.Log($"RemoveRadialComponent - (setting active to false): {menuTitle}");
									transform.gameObject.SetActive(false);
									found = true;
									break;
								}
							}
						}
						if (found)
							break;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		private static bool TriggerShouldRemoveCallback(NGuid miniAtMenu, string menuTitle, RadialCheckRemove radialCheckRemove)
		{
			Debug.LogWarning("Calling radialCheckRemove.ShouldRemoveCallback(menuTitle, miniAtMenu.ToString(), target.ToString())...");
			return radialCheckRemove.ShouldRemoveCallback(menuTitle, miniAtMenu.ToString(), target.ToString());
		}

		private static string GetMenuTitle(Transform transform)
		{
			MapMenuItem mapMenuItem = transform.GetComponent<MapMenuItem>();
			if (mapMenuItem != null)
				return mapMenuItem.GetTitle();
			MapMenuStatItem mapMenuStatItem = transform.GetComponent<MapMenuStatItem>();
			if (mapMenuStatItem != null)
				return mapMenuStatItem.GetTitle();
			return null;
		}

		List<string> previouslyModded = new List<string>();
		bool needToClear;

		/// <summary>
		/// The creature at the menu (not necessarily the selected creature - that can be different!)
		/// </summary>
		public static CreatureBoardAsset CreatureAtMenu { get; private set; }

		public void ClearPreviouslyModded()
		{
			previouslyModded.Clear();
		}

		private void AddMenuItem(Dictionary<string, (MapMenu.ItemArgs mapArgs, Func<NGuid, NGuid, bool> shouldAdd)> dic, NGuid myCreature, MapMenu mapMenu)
		{
			target = Talespire.RadialMenus.GetTargetCreatureId();

			var validMenuMods = new List<(MapMenu.ItemArgs mapArgs, Func<NGuid, NGuid, bool> shouldAdd)>();
			foreach (string key in dic.Keys)
			{
				var menuMod = dic[key];
				if (menuMod.shouldAdd == null || menuMod.shouldAdd(myCreature, target))
					if (!previouslyModded.Contains(key))
					{
						previouslyModded.Add(key);
						validMenuMods.Add(menuMod);
					}
			}

			foreach (var handler in validMenuMods)
			{
				mapMenu.AddItem(CreateMenu(handler));
				int count = mapMenu.transform.childCount;
				Transform lastTransform = mapMenu.transform.GetChild(count - 1);
				RectTransform rectTransform = lastTransform.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
				rectTransform.sizeDelta = new Vector2(48f, 48f);
				//Debug.Log("Set Size 48");
			}
		}

		private MapMenu.ItemArgs CreateMenu((MapMenu.ItemArgs mapArgs, Func<NGuid, NGuid, bool> shouldAdd) handler)
		{
			MapMenu.ItemArgs clonedMenu = handler.mapArgs.Clone();
			clonedMenu.Action = (mmi, obj) =>
			{
				pendingMenuAction = (handler.mapArgs.Action, mmi, obj);
				menuExecutionTime = DateTime.Now.AddMilliseconds(200);
			};
			return clonedMenu;
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
			lastHideVolumeItem = Talespire.RadialMenus.GetSelectedHideVolumeItem();
			foreach (var handlers
					in dic.Values
							.Where(handlers => handlers.Item2 == null
										 || handlers.Item2(lastHideVolumeItem))) map.AddItem(handlers.Item1);
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
