using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class ImmuneSystemSimulator : Challenge
    {
        public static int GetOutcome()
        {
            List<Army> armies = GetStartingState();
            
            while (armies.Count(a => a.units.Count != 0) > 1)
            {
                armies.ForEach( army => army.units.OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).ToList().ForEach(unit => unit.target = armies.Find(a => a.type != army.type).units.Where(u => u.weakTo.Contains(unit.attackType) && !army.units.Select(uu => uu.target).Contains(u)).ToList().OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).DefaultIfEmpty(armies.Find(a => a.type != army.type).units.Where(u => !u.immuneTo.Contains(unit.attackType) && !army.units.Select(uu => uu.target).Contains(u)).ToList().OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).DefaultIfEmpty(armies.Find(a => a.type != army.type).units.Where(u => !army.units.Select(uu => uu.target).Contains(u)).OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).FirstOrDefault()).FirstOrDefault()).FirstOrDefault()));

                List<Unit> toRemove = new List<Unit>();
                foreach (Unit unit in armies.SelectMany(a => a.units).Where(u => u.target != null && u.count > 0).OrderByDescending(u => u.initiative))
                {
                    int multiplier = 1;
                    if (unit.target.weakTo.Contains(unit.attackType))
                        multiplier = 2;
                    else if (unit.target.immuneTo.Contains(unit.attackType))
                        multiplier = 0;

                    int damage = (multiplier * unit.attack * unit.count) / unit.target.hitpoints;
                    unit.target.count -= damage;
                    if (unit.target.count < 1) toRemove.Add(unit.target);
                    unit.target = null;
                }

                foreach (Unit unit in toRemove)
                {
                    foreach (Army army in armies)
                    {
                        army.units.Remove(unit);
                    }
                }
            }

            return armies.Find(a => a.units.Count != 0).units.Sum(u => u.count);
        }

        public static int GetBetterOutcome()
        {
            List<Army> armies;
            int boost = 1;
            do
            {
                armies = GetStartingState();
                armies.Find(a => a.type == Army.Type.IMMUNESYSTEM).units.ForEach(u => u.attack += boost);

                int haltCounter = 0;
                while (armies.Count(a => a.units.Count != 0) > 1 && haltCounter < 10000)
                {
                    armies.ForEach(army => army.units.OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).ToList().ForEach(unit => unit.target = armies.Find(a => a.type != army.type).units.Where(u => u.weakTo.Contains(unit.attackType) && !army.units.Select(uu => uu.target).Contains(u)).ToList().OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).DefaultIfEmpty(armies.Find(a => a.type != army.type).units.Where(u => !u.immuneTo.Contains(unit.attackType) && !army.units.Select(uu => uu.target).Contains(u)).ToList().OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).DefaultIfEmpty(armies.Find(a => a.type != army.type).units.Where(u => !army.units.Select(uu => uu.target).Contains(u)).OrderByDescending(u => u.attack * u.count).ThenByDescending(u => u.initiative).FirstOrDefault()).FirstOrDefault()).FirstOrDefault()));

                    List<Unit> toRemove = new List<Unit>();
                    foreach (Unit unit in armies.SelectMany(a => a.units).Where(u => u.target != null && u.count > 0).OrderByDescending(u => u.initiative))
                    {
                        int multiplier = 1;
                        if (unit.target.weakTo.Contains(unit.attackType))
                            multiplier = 2;
                        else if (unit.target.immuneTo.Contains(unit.attackType))
                            multiplier = 0;

                        int damage = (multiplier * unit.attack * unit.count) / unit.target.hitpoints;
                        if (damage > 0) unit.target.count -= damage;
                        if (unit.target.count < 1) toRemove.Add(unit.target);
                        unit.target = null;
                    }

                    foreach (Unit unit in toRemove)
                    {
                        foreach (Army army in armies)
                        {
                            army.units.Remove(unit);
                        }
                    }

                    haltCounter++;
                }

                armies.RemoveAll(a => a.units.Count == 0);

                boost++;
            }
            while (!(armies.Count == 1 && armies.First().type == Army.Type.IMMUNESYSTEM));

            return armies.Find(a => a.units.Count != 0).units.Sum(u => u.count);
        }

        private static List<Army> GetStartingState()
        {
            List<Army> armies = new List<Army>();
            armies.Add(new Army(Army.Type.IMMUNESYSTEM));
            armies.Add(new Army(Army.Type.INFECTION));

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(24)))
                {
                    Army.Type currentType = Army.Type.IMMUNESYSTEM;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line == "Immune System:")
                        {
                            currentType = Army.Type.IMMUNESYSTEM;
                        }
                        else if (line == "Infection:")
                        {
                            currentType = Army.Type.INFECTION;
                        }
                        else if (line != string.Empty)
                        {
                            armies.Find(a => a.type == currentType).units.Add(Unit.Parse(line));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return armies;
        }

        public class Army
        {
            public enum Type
            {
                IMMUNESYSTEM,
                INFECTION
            }

            public Type type;
            public List<Unit> units = new List<Unit>();

            public Army(Type type)
            {
                this.type = type;
            }
        }

        public enum DamageType
        {
            BLUDGEONING,
            SLASHING,
            COLD,
            FIRE,
            RADIATION,
            UNKNOWN
        }

        private static DamageType ParseDamageType(string word)
        {
            switch (word)
            {
                case "bludgeoning":
                    return DamageType.BLUDGEONING;
                case "slashing":
                    return DamageType.SLASHING;
                case "cold":
                    return DamageType.COLD;
                case "fire":
                    return DamageType.FIRE;
                case "radiation":
                    return DamageType.RADIATION;
            }

            return DamageType.UNKNOWN;
        }

        public class Unit
        {
            public int count;
            public int hitpoints;
            public int attack;
            public DamageType attackType;
            public int initiative;
            public List<DamageType> weakTo = new List<DamageType>();
            public List<DamageType> immuneTo = new List<DamageType>();

            public Unit target;

            public static Unit Parse(string line)
            {
                Unit unit = new Unit();
                string[] words = line.Split(' ');

                unit.count = int.Parse(words[0]);
                unit.hitpoints = int.Parse(words[4]);
                unit.attackType = ParseDamageType(words[words.Length - 5]);
                unit.attack = int.Parse(words[words.Length - 6]);
                unit.initiative = int.Parse(words[words.Length - 1]);

                if (line.Contains("(") && line.Contains(")"))
                {
                    List<string> weaknessesAndImmunities = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1).Split(';').Select(w => w.Trim(' ')).ToList();

                    string weaknessesString = weaknessesAndImmunities.Find(s => s.StartsWith("weak"));
                    if (weaknessesString != null)
                    {
                        foreach (string weakness in weaknessesString.Remove(0, "weak to ".Count()).Split(',').Select(i => i.Trim(' ')).ToList())
                        {
                            unit.weakTo.Add(ParseDamageType(weakness));
                        }
                    }

                    string immuninitiesString = weaknessesAndImmunities.Find(s => s.StartsWith("immune"));
                    if (immuninitiesString != null)
                    {
                        foreach (string immunity in immuninitiesString.Remove(0, "immune to ".Count()).Split(',').Select(i => i.Trim(' ')).ToList())
                        {
                            unit.immuneTo.Add(ParseDamageType(immunity));
                        }
                    }
                }

                return unit;
            }
        }
    }
}
