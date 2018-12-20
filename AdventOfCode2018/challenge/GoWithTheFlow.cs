using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class GoWithTheFlow : Challenge
    {
        public static int[] GetOutcome()
        {
            int[] registers = new int[6];
            registers[0] = 1;
            (int ip, Instruction[] instructions) program = GetProgram();
            
            while (registers[program.ip] < program.instructions.Length)
            {
                Console.WriteLine(registers[program.ip]);

                registers = ExecuteInstruction(program.instructions[registers[program.ip]], registers);
                registers[program.ip]++;
            }

            return registers;
        }

        private static (int, Instruction[]) GetProgram()
        {
            List<Instruction> list = new List<Instruction>();
            int position;

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(19)))
                {
                    position = int.Parse(sr.ReadLine().Last().ToString());

                    while (!sr.EndOfStream)
                    {
                        list.Add(Instruction.Parse(sr.ReadLine()));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return (position, list.ToArray());
        }

        private static int[] ExecuteInstruction(Instruction instruction, int[] registers)
        {
            switch (instruction.opcode)
            {
                // addr (add register) stores into register C the result of adding register A and register B.
                case ("addr"):
                    registers[instruction.C] = registers[instruction.A] + registers[instruction.B];
                    break;
                // addi (add immediate) stores into register C the result of adding register A and value B.
                case ("addi"):
                    registers[instruction.C] = registers[instruction.A] + instruction.B;
                    break;
                // mulr (multiply register) stores into register C the result of multiplying register A and register B.
                case ("mulr"):
                    registers[instruction.C] = registers[instruction.A] * registers[instruction.B];
                    break;
                // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
                case ("muli"):
                    registers[instruction.C] = registers[instruction.A] * instruction.B;
                    break;
                // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
                case ("banr"):
                    registers[instruction.C] = (registers[instruction.A] & registers[instruction.B]);
                    break;
                // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
                case ("bani"):
                    registers[instruction.C] = (registers[instruction.A] & instruction.B);
                    break;
                // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
                case ("borr"):
                    registers[instruction.C] = (registers[instruction.A] | registers[instruction.B]);
                    break;
                // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
                case ("bori"):
                    registers[instruction.C] = (registers[instruction.A] | instruction.B);
                    break;
                // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
                case ("setr"):
                    registers[instruction.C] = registers[instruction.A];
                    break;
                // seti (set immediate) stores value A into register C. (Input B is ignored.)
                case ("seti"):
                    registers[instruction.C] = instruction.A;
                    break;
                // gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
                case ("gtir"):
                    registers[instruction.C] = ((instruction.A > registers[instruction.B]) ? 1 : 0);
                    break;
                // gtri (greater - than register / immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
                case ("gtri"):
                    registers[instruction.C] = ((registers[instruction.A] > instruction.B) ? 1 : 0);
                    break;
                // gtrr (greater - than register / register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
                case ("gtrr"):
                    registers[instruction.C] = ((registers[instruction.A] > registers[instruction.B]) ? 1 : 0);
                    break;
                // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
                case ("eqir"):
                    registers[instruction.C] = ((instruction.A == registers[instruction.B]) ? 1 : 0);
                    break;
                // eqri (equal register / immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
                case ("eqri"):
                    registers[instruction.C] = ((registers[instruction.A] == instruction.B) ? 1 : 0);
                    break;
                // eqrr (equal register / register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
                case ("eqrr"):
                    registers[instruction.C] = ((registers[instruction.A] == registers[instruction.B]) ? 1 : 0);
                    break;
            }

            return registers;
        }

        public class Instruction
        {
            public string opcode;
            public int A;
            public int B;
            public int C;

            public static Instruction Parse(string line)
            {
                string[] words = line.Split(' ');
                return new Instruction()
                {
                    opcode = words[0],
                    A = int.Parse(words[1]),
                    B = int.Parse(words[2]),
                    C = int.Parse(words[3])
                };
            }
        }
    }
}
