/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/Evolve) 
    Dual-Licensed under the Educational Community License, Version 2.0 and
    the binpress license you may not use this file except in compliance with 
    the License. You may obtain a copy of the Licenses at
	
	http://www.osedu.org/licenses/ECL-2.0
    http://www.binpress.com/license/view/l/6cfa4c36602b0f90ab898dc9fbd77b84
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
 
    The binpress license states that:
     - May only be used for personal use (cannot be resold or distributed)
     - Non-commerical use only
     - Cannot modify source code for any purpose (cannot create derivative works
    The implications of not following the license may lead to legal action, depending
    on the circumstances.
*/
using System;
using System.Collections.Generic;

namespace MCLawl
{
    public class CmdTempBan : Command
    {
        public override string name { get { return "tempban"; } }
        public override string shortcut { get { return "tb"; } }
        public override string type { get { return "moderation"; } }
        public override bool museumUsable { get { return true; } }
        //public override bool consoleUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdTempBan() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (message.IndexOf(' ') == -1) message = message + " 30";

            Player who = Player.Find(message.Split(' ')[0]);
            if (who == null) { Player.SendMessage(p, "Could not find player"); return; }
            if (who.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot ban someone of the same rank"); return; }

            int minutes;
            try
            {
                minutes = int.Parse(message.Split(' ')[1]);
            } catch { Player.SendMessage(p, "Invalid minutes"); return; }
            if (minutes > 60) { Player.SendMessage(p, "Cannot ban for more than an hour"); return; }
            if (minutes < 1) { Player.SendMessage(p, "Cannot ban someone for less than a minute"); return; }
            
            Server.TempBan tBan;
            tBan.name = who.name;
            tBan.allowedJoin = DateTime.Now.AddMinutes(minutes);
            Server.tempBans.Add(tBan);
            who.Kick("Banned for " + minutes + " minutes!");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tempban <name> <minutes> - Bans <name> for <minutes>");
            Player.SendMessage(p, "Max time is 60. Default is 30");
            Player.SendMessage(p, "Temp bans will reset on server restart");
        }
    }
}