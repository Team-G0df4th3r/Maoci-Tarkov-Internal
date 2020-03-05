using EFT;
using EFT.Interactive;
using System;
using System.Collections.Generic;

namespace UnhandledException
{
    public class Types
    {
        public static Type ObserverCorpse = new ObservedCorpse().GetType();
        public static Type Corpse = new Corpse().GetType();
        public static Type ObservedLootItem = new ObservedLootItem().GetType();
        public static Type LootItem = new LootItem().GetType();
        public static Type Player = new Player().GetType();
        //public static Type Door = new WorldInteractiveObject().GetType();
        public static Dictionary<string, int> GroupTable = new Dictionary<string, int>();
        // public static Type Throwable = new Throwable().GetType(); // no interface for this
    }
}
