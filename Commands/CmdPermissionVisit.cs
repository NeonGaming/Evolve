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

namespace MCLawl
{
    public class CmdPermissionVisit : Command
    {
        public override string name { get { return "pervisit"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        //public override bool consoleUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdPermissionVisit() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 2 || number < 1) { Help(p); return; }
            if (number == 1)
            {
                LevelPermission Perm = Level.PermissionFromName(message);
                if (Perm == LevelPermission.Null) { Player.SendMessage(p, "Not a valid rank"); return; }
                p.level.permissionvisit = Perm;
                Server.s.Log(p.level.name + " visit permission changed to " + message + ".");
                Player.GlobalMessageLevel(p.level, "visit permission changed to " + message + ".");
            }
            else
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                LevelPermission Perm = Level.PermissionFromName(s);
                if (Perm == LevelPermission.Null) { Player.SendMessage(p, "Not a valid rank"); return; }

                Level level = Level.Find(t);
                if (level != null)
                {
                    level.permissionvisit = Perm;
                    Server.s.Log(level.name + " visit permission changed to " + s + ".");
                    Player.GlobalMessageLevel(level, "visit permission changed to " + s + ".");
                    if (p != null)
                        if (p.level != level) { Player.SendMessage(p, "visit permission changed to " + s + " on " + level.name + "."); }
                    return;
                }
                else
                    Player.SendMessage(p, "There is no level \"" + s + "\" loaded.");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/PerVisit <map> <rank> - Sets visiting permission for a map.");
        }
    }
}