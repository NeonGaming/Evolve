/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/Evolve) 
    Dual-Licensed under the Educational Community License, Version 2.0 and
    the binpress license you mayy not use this file except in compliance with 
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
using System.IO;
using System.Collections.Generic;

namespace MCLawl
{
    public sealed class PlayerList
    {
        //public string name;
        public Group group;
        List<string> players = new List<string>();
        public PlayerList() { }
        public void Add(string p) { players.Add(p.ToLower()); }
        public bool Remove(string p)
        {
            return players.Remove(p.ToLower());
        }
        public bool Contains(string p) { return players.Contains(p.ToLower()); }
        public List<string> All() { return new List<string>(players); }
        public void Save(string path) { Save(path, true); }
        public void Save() {
            Save(group.fileName); 
        }
        public void Save(string path, bool console)
        {
            StreamWriter file = File.CreateText("ranks/" + path);
            players.ForEach(delegate(string p) { file.WriteLine(p); });
            file.Close(); if (console) { Server.s.Log("SAVED: " + path); }
        }
        public static PlayerList Load(string path, Group groupName)
        {
            if (!Directory.Exists("ranks")) { Directory.CreateDirectory("ranks"); }
            path = "ranks/" + path;
            PlayerList list = new PlayerList();
            list.group = groupName;
            if (File.Exists(path))
            {
                foreach (string line in File.ReadAllLines(path)) { list.Add(line); }
            }
            else
            {
                File.Create(path).Close();
                Server.s.Log("CREATED NEW: " + path);
            } return list;
        }
    }
}