using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bounce.Unmanaged;
using UnityEngine;
using UnityEngine.UI;

namespace RadialUI
{
    public static class MapMenuExt
    {
        private const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public static MapMenuStatItem AddStatItem(this MapMenu map, StatItemArgs args, NGuid target, int index)
        {
            var factory = (SpawnFactory<MapMenuStatItem>)map.GetType().GetField("_statFactory", bindFlags).GetValue(map);
            var mapMenuStatItem = factory.HireItem();
            mapMenuStatItem.Setup(args.Title, new CreatureGuid(target), 0);
            mapMenuStatItem.transform.localPosition = Vector3.zero;

            var image = (Image)mapMenuStatItem.GetType().GetField("_color", bindFlags).GetValue(mapMenuStatItem);
            image.color = args.Color;

            mapMenuStatItem.GetType().GetField("_statIndex", bindFlags).SetValue(mapMenuStatItem,index);
            
            var c = (UINumberInput)mapMenuStatItem.GetType().GetField("current", bindFlags).GetValue(mapMenuStatItem);
            var m = (UINumberInput)mapMenuStatItem.GetType().GetField("maxNumber", bindFlags).GetValue(mapMenuStatItem);
            c.SetValue(args.Current);
            m.SetValue(args.Max);


            // Set Visible and return
            mapMenuStatItem.gameObject.SetActive(true);
            return mapMenuStatItem;
        }

        public static StatItemArgs GetStatItem( this MapMenuStatItem item)
        {
            var output = new StatItemArgs()
            {
                Current = ((UINumberInput)item.GetType().GetField("current", bindFlags).GetValue(item)).Value,
                Max = ((UINumberInput)item.GetType().GetField("maxNumber", bindFlags).GetValue(item)).Value,
                Color = ((Image)item.GetType().GetField("_color", bindFlags).GetValue(item)).color,
                // Title = ((string)item.GetType().GetField("_title", bindFlags).GetValue(item)),
            };

            return output;
        }
    }
}
