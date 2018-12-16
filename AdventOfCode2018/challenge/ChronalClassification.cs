using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class ChronalClassification : Challenge
    {
        public static int GetCountThreeOrMorePossibilities()
        {
            (List<Sample> instructions, List<int[]> program) samples = GetSamples();
            List<Sample> instructions = ProcessSamples(samples.instructions);

            return instructions.Count(s => s.possibleOpcodes.Count >= 3);
        }

        public static int GetProgramOutcome()
        {
            (List<Sample> instructions, List<int[]> program) samples = GetSamples();
            Dictionary<int, string> dictionary = GetOpcodeDictionary(samples.instructions);

            return ExecuteProgram(samples.program, dictionary)[0];
        }

        private static int[] ExecuteProgram(List<int[]> program, Dictionary<int, string> dictionary)
        {
            int[] registers = new int[4];
            foreach (int[] instruction in program)
            {
                switch (dictionary[instruction[0]])
                {
                    // addr (add register) stores into register C the result of adding register A and register B.
                    case ("addr"):
                        registers[instruction[3]] = registers[instruction[1]] + registers[instruction[2]];
                        break;
                    // addi (add immediate) stores into register C the result of adding register A and value B.
                    case ("addi"):
                        registers[instruction[3]] = registers[instruction[1]] + instruction[2];
                        break;
                    // mulr (multiply register) stores into register C the result of multiplying register A and register B.
                    case ("mulr"):
                        registers[instruction[3]] = registers[instruction[1]] * registers[instruction[2]];
                        break;
                    // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
                    case ("muli"):
                        registers[instruction[3]] = registers[instruction[1]] * instruction[2];
                        break;
                    // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
                    case ("banr"):
                        registers[instruction[3]] = (registers[instruction[1]] & registers[instruction[2]]);
                        break;
                    // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
                    case ("bani"):
                        registers[instruction[3]] = (registers[instruction[1]] & instruction[2]);
                        break;
                    // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
                    case ("borr"):
                        registers[instruction[3]] = (registers[instruction[1]] | registers[instruction[2]]);
                        break;
                    // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
                    case ("bori"):
                        registers[instruction[3]] = (registers[instruction[1]] | instruction[2]);
                        break;
                    // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
                    case ("setr"):
                        registers[instruction[3]] = registers[instruction[1]];
                        break;
                    // seti (set immediate) stores value A into register C. (Input B is ignored.)
                    case ("seti"):
                        registers[instruction[3]] = instruction[1];
                        break;
                    // gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
                    case ("gtir"):
                        registers[instruction[3]] = ((instruction[1] > registers[instruction[2]]) ? 1 : 0);
                        break;
                    // gtri (greater - than register / immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
                    case ("gtri"):
                        registers[instruction[3]] = ((registers[instruction[1]] > instruction[2]) ? 1 : 0);
                        break;
                    // gtrr (greater - than register / register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
                    case ("gtrr"):
                        registers[instruction[3]] = ((registers[instruction[1]] > registers[instruction[2]]) ? 1 : 0);
                        break;
                    // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
                    case ("eqir"):
                        registers[instruction[3]] = ((instruction[1] == registers[instruction[2]]) ? 1 : 0);
                        break;
                    // eqri (equal register / immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
                    case ("eqri"):
                        registers[instruction[3]] = ((registers[instruction[1]] == instruction[2]) ? 1 : 0);
                        break;
                    // eqrr (equal register / register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
                    case ("eqrr"):
                        registers[instruction[3]] = ((registers[instruction[1]] == registers[instruction[2]]) ? 1 : 0);
                        break;
                }
            }

            return registers;
        }

        private static Dictionary<int, string> GetOpcodeDictionary(List<Sample> samples)
        {
            Dictionary<int, string> opcodeDictionary = new Dictionary<int, string>();
            samples = ProcessSamples(samples);

            while (opcodeDictionary.Keys.Count != 16)
            {
                foreach (string opcode in samples.SelectMany(c => c.possibleOpcodes).Distinct())
                {
                    int[] possibleOpcodes = samples.GroupBy(s => s.instruction[0]).Where(g => g.ToList().TrueForAll(s => s.possibleOpcodes.Contains(opcode))).SelectMany(g => g).Select(s => s.instruction[0]).Distinct().Except(opcodeDictionary.Keys).ToArray();

                    if (possibleOpcodes.Length == 1)
                    {
                        opcodeDictionary.Add(possibleOpcodes[0], opcode);
                    }
                }
            }

            return opcodeDictionary;
        }

        private static List<Sample> ProcessSamples(List<Sample> samples)
        {
            foreach (Sample sample in samples)
            {
                // addr (add register) stores into register C the result of adding register A and register B.
                if (sample.registersAfter[sample.instruction[3]] == sample.registersBefore[sample.instruction[1]] + sample.registersBefore[sample.instruction[2]])
                    sample.possibleOpcodes.Add("addr");

                // addi (add immediate) stores into register C the result of adding register A and value B.
                if (sample.registersAfter[sample.instruction[3]] == sample.registersBefore[sample.instruction[1]] + sample.instruction[2])
                    sample.possibleOpcodes.Add("addi");

                // mulr (multiply register) stores into register C the result of multiplying register A and register B.
                if (sample.registersAfter[sample.instruction[3]] == sample.registersBefore[sample.instruction[1]] * sample.registersBefore[sample.instruction[2]])
                    sample.possibleOpcodes.Add("mulr");

                // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
                if (sample.registersAfter[sample.instruction[3]] == sample.registersBefore[sample.instruction[1]] * sample.instruction[2])
                    sample.possibleOpcodes.Add("muli");

                // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] & sample.registersBefore[sample.instruction[2]]))
                    sample.possibleOpcodes.Add("banr");

                // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] & sample.instruction[2]))
                    sample.possibleOpcodes.Add("bani");

                // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] | sample.registersBefore[sample.instruction[2]]))
                    sample.possibleOpcodes.Add("borr");

                // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] | sample.instruction[2]))
                    sample.possibleOpcodes.Add("bori");

                // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
                if (sample.registersAfter[sample.instruction[3]] == sample.registersBefore[sample.instruction[1]])
                    sample.possibleOpcodes.Add("setr");

                // seti (set immediate) stores value A into register C. (Input B is ignored.)
                if (sample.registersAfter[sample.instruction[3]] == sample.instruction[1])
                    sample.possibleOpcodes.Add("seti");

                // gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
                if (sample.registersAfter[sample.instruction[3]] == (sample.instruction[1] > sample.registersBefore[sample.instruction[2]] ? 1 : 0))
                    sample.possibleOpcodes.Add("gtir");

                // gtri (greater - than register / immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] > sample.instruction[2] ? 1 : 0))
                    sample.possibleOpcodes.Add("gtri");

                // gtrr (greater - than register / register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] > sample.registersBefore[sample.instruction[2]] ? 1 : 0))
                    sample.possibleOpcodes.Add("gtrr");

                // eqir (equal immediate / register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
                if (sample.registersAfter[sample.instruction[3]] == (sample.instruction[1] == sample.registersBefore[sample.instruction[2]] ? 1 : 0))
                    sample.possibleOpcodes.Add("eqir");

                // eqri (equal register / immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] == sample.instruction[2] ? 1 : 0))
                    sample.possibleOpcodes.Add("eqri");

                // eqrr (equal register / register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
                if (sample.registersAfter[sample.instruction[3]] == (sample.registersBefore[sample.instruction[1]] == sample.registersBefore[sample.instruction[2]] ? 1 : 0))
                    sample.possibleOpcodes.Add("eqrr");
            }

            return samples;
        }

        private static (List<Sample>, List<int[]>) GetSamples()
        {
            List<Sample> sampleInstructions = new List<Sample>();
            List<int[]> sampleProgram = new List<int[]>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(16)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("Before:"))
                        {
                            sampleInstructions.Add(Sample.Parse(line, sr.ReadLine(), sr.ReadLine()));
                            sr.ReadLine();
                        }
                        else if (line == string.Empty)
                        {
                            // Pass empty lines between instruction samples and program
                        }
                        else
                        {
                            sampleProgram.Add(line.Split(' ').Select(c => int.Parse(c)).ToArray());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return (sampleInstructions, sampleProgram);
        }

        public class Sample
        {
            public List<string> possibleOpcodes = new List<string>();

            public int[] registersBefore;
            public int[] instruction;
            public int[] registersAfter;

            public static Sample Parse(string beforeLine, string instructionLine, string afterLine)
            {
                return new Sample()
                {
                    registersBefore = beforeLine.Substring(beforeLine.IndexOf('[') + 1, beforeLine.LastIndexOf(']') - beforeLine.IndexOf('[') - 1).Split(',').Select(c => int.Parse(c.Trim(' '))).ToArray(),
                    instruction = instructionLine.Split(' ').Select(c => int.Parse(c)).ToArray(),
                    registersAfter = afterLine.Substring(afterLine.IndexOf('[') + 1, afterLine.LastIndexOf(']') - afterLine.IndexOf('[') - 1).Split(',').Select(c => int.Parse(c.Trim(' '))).ToArray()
                };
            }
        }
    }
}
