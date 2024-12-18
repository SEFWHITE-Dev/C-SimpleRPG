﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Quest
    {
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }

        // items required to complete a quest
        public List<ItemQuantity> ItemsToComplete { get; }

        public int RewardExp { get; }
        public int RewardGold { get; }
        public List<ItemQuantity> RewardItems { get; }

        public string ToolTipContents =>
            Description + Environment.NewLine + Environment.NewLine +
            "Items to complete the quest" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine, ItemsToComplete.Select(i => i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Rewards\r\n" +
            "===========================" + Environment.NewLine +
            $"{RewardExp} EXP" + Environment.NewLine +
            $"{RewardGold} Gold" + Environment.NewLine +
            string.Join(Environment.NewLine, RewardItems.Select(i => i.QuantityItemDescription));


        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete,
            int rewardExp, int rewardGold, List<ItemQuantity> rewardItems) 
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExp = rewardExp;
            RewardGold = rewardGold;
            RewardItems = rewardItems;

        }
    }
}
