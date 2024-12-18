﻿using Engine.Models;
using Engine.Shared;
using System.Diagnostics;
using System.Xml;

namespace Engine.Factories
{
    // static class, all methods must be static as well
    // an instanced class (like Player class) CAN have static functions. These functions can be called without creating an instance of the class.
    // because static classes are not instanced, they have no constructor
    // it is possiblet to run a function whenever anything uses something from a static class
    // a static funciton can only use the objects it creates inside the functions, other static functions, or static class-level variables.
    // WorldFactory can be safely made into a static class because we are not 'maintaining state' within it
    //  we are not holding any variable values inside the static class
    // Because static classes do not need to be instantiated and can be referenced from anywhere in the program,
    // creating static variables where the value is changed in multiple locations can lead to hard to track down bugs
    internal static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Locations.xml";

        internal static World CreateWorld()
        {
            World newWorld = new World();

            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath = data.SelectSingleNode("/Locations").AttributeAsString("RootImagePath");

                LoadLocationsFromNodes(newWorld, rootImagePath, data.SelectNodes("/Locations/Location")); 
            }
            else
            {
                throw new FileNotFoundException($"Missing data file {GAME_DATA_FILENAME}");
            }
            return newWorld;
            
            /* OLD CONSTRUCTOR
            World newWorld = new World();

            // Add Map Locations
            newWorld.AddLocation(0, -1, "Home", "This is your home.", "Home.png");
            newWorld.AddLocation(-1, -1, "Farm House", "A farm.", "FarmHouse.png");
            newWorld.AddLocation(-2, -1, "Farm Field", "A field.", "FarmFields.png");
            newWorld.AddLocation(-1, 0, "Trading Shop","The shop of Susan, the trader.","Trader.png");
            newWorld.AddLocation(0, 0, "Town square","You see a fountain here.","TownSquare.png");
            newWorld.AddLocation(1, 0, "Town Gate","There is a gate here, protecting the town from giant spiders.","TownGate.png");
            newWorld.AddLocation(2, 0, "Skeleton Forest","The trees in this forest are covered with spider webs.","SpiderForest.png");
            newWorld.AddLocation(0, 1, "Herbalist's hut","You see a small hut, with plants drying from the roof.","HerbalistsHut.png");
            newWorld.AddLocation(0, 2, "Herbalist's garden","There are many plants here, with snakes hiding behind them.", "HerbalistsGarden.png");

            // Add quests to existing locations
            newWorld.LocationAt(0,1).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));


            // Add Monster spawns to existing locations
            newWorld.LocationAt(-2, -1).AddMonster(1, 100);
            newWorld.LocationAt(2, 0).AddMonster(2, 100);
            newWorld.LocationAt(0, 2).AddMonster(3, 100);

            // Add Traders to existing locaitons
            newWorld.LocationAt(-1, -1).TraderHere = TraderFactory.GetTraderByName("Woman");
            newWorld.LocationAt(-1, 0).TraderHere = TraderFactory.GetTraderByName("Man");
            newWorld.LocationAt(0, 1).TraderHere = TraderFactory.GetTraderByName("Herbalist");


            return newWorld;
            */
        }

        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                Location location = new Location(node.AttributeAsInt("X"),
                    node.AttributeAsInt("Y"),
                    node.AttributeAsString("Name"),
                    node.SelectSingleNode("./Description")?.InnerText ?? "", // InnerText is not an attribute but the content between the tags
                    $".{rootImagePath}{node.AttributeAsString("ImageName")}" );

                AddMonsters(location, node.SelectNodes("./Monsters/Monster"));
                AddQuests(location, node.SelectNodes("./Quests/Quest"));
                AddTrader(location, node.SelectSingleNode("./Trader"));

                world.AddLocation(location);
            }
        }

        private static void AddMonsters(Location location, XmlNodeList monsters)
        {
            if(monsters == null)
            {
                Debug.Print("no monsters");
                return;
                
            }

            foreach (XmlNode monsterNode in monsters)
            {
                Debug.Print("monsters adddedd");
                location.AddMonster(monsterNode.AttributeAsInt("ID"), monsterNode.AttributeAsInt("Percent"));
            }
        }

        private static void AddQuests(Location location, XmlNodeList quests)
        {
            if (quests == null)
            {
                return;
            }

            foreach (XmlNode questNode in quests)
            {
                location.QuestsAvailableHere.Add(QuestFactory.GetQuestByID(questNode.AttributeAsInt("ID")));        
            }
        }

        private static void AddTrader(Location location, XmlNode traders)
        {
            if (traders == null)
            {
                return;
            }
            location.TraderHere = TraderFactory.GetTraderByID(traders.AttributeAsInt("ID"));
    
        }
    }
}
