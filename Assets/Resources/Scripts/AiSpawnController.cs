using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AiSpawnController : MonoBehaviour  {

    public List<Spawn> spawns;
    public List<string> spawningPool;

    public void Spawn() {
        foreach(var id in spawningPool) {
            SpawnById(id);
        }
    }

    public void SpawnById(string id) {
        var spawn = GetSpawnById(id);
        if(spawn != null) {
            spawn.Execute();
        } else {
            Debug.LogErrorFormat("Skipping unknown spawn with id '{0}'", id);
        }
    }

    //TODO does this make sense?
    public bool InterpretCommand(string directive, string argument) {
        if (directive.ToLower() == "spawn pool") {
            var matches = Regex.Matches(argument, @"([-+=])([a-z0-9_]*)\b", RegexOptions.IgnoreCase);
            foreach(Match match in matches) {
                var op = match.Groups[1].Value;
                var spawn = match.Groups[2].Value;

                if(op == "=") {
                    spawningPool.Clear();
                    if(spawn.Length > 0) {
                        spawningPool.Add(spawn);
                    }
                } else if(op == "+") {
                    spawningPool.Add(spawn);
                } else if(op == "-") {
                    spawningPool.Remove(spawn);
                }
            }

            return true;
        } else if(directive.ToLower() == "spawn") {
            var spawns = Regex.Split(argument, @"\s+");
            foreach(var spawn in spawns) {
                SpawnById(spawn);
            }

            return true;
        }

        return false;
    }

    private Spawn GetSpawnById(string id) {
        foreach(var spawn in spawns) {
            if (spawn.id == id)
                return spawn;
        }

        return null;
    }
}
