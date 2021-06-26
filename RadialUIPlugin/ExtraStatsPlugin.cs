using BepInEx;
using Bounce.Unmanaged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LordAshes;
using Newtonsoft.Json;
using RadialUI;
using UnityEngine;
using Object = System.Object;

namespace ExtraStats
{

    [BepInPlugin(Guid, "ExtraStatsPlugin", Version)]
    public class ExtraStatsPlugin : BaseUnityPlugin
    {
        // constants
        public const string Guid = "org.hollofox.plugins.ExtraStatsPlugin";
        private const string Version = "1.0.0.0";

        /// <summary>
        /// Awake plugin
        /// </summary>
        void Awake()
        {
            Logger.LogInfo("In Awake for Extra Stats");

            Debug.Log("ExtraStats Plug-in loaded");

            RadialUIPlugin.AddOnStatMenu(Guid + "1", new StatItemArgs
            {
                Callback = callback,
                Color = Color.cyan,
                Current = 5,
                Max = 10,
                Title = "New Cyan Stat"
            });

            RadialUIPlugin.AddOnStatMenu(Guid + "2", new StatItemArgs
            {
                Callback = callback,
                Color = Color.magenta,
                Current = 5,
                Max = 10,
                Title = "New Magenta Stat"
            });
        }

        private void callback(MapMenuStatItem i, NGuid g)
        {
            Debug.Log(g);
            Debug.Log(i);
        }




        /// <summary>
        /// Looping method run by plugin
        /// </summary>
        void Update()
        {
        }

        private const BindingFlags bindFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    }
}
