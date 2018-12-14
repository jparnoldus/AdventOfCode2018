using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class ChocolateCharts : Challenge
    {
        public static string GetLastTen(int input)
        {
            (List<Recipe> recipes, List<Elf> elves) state = GetStartingState();

            int addedCounter = 0;
            while (addedCounter < input + 10)
            {
                // First add the new recipes
                foreach (char number in state.elves.Sum(e => e.currentRecipe.value).ToString())
                {
                    Recipe newRecipe = new Recipe(int.Parse(number.ToString()));
                    state.recipes.Last().next = newRecipe;
                    newRecipe.next = state.recipes.First();

                    state.recipes.Add(newRecipe);
                    addedCounter++;
                }

                // Then give the elfs a new current
                foreach (Elf elf in state.elves)
                {
                    int change = elf.currentRecipe.value + 1;
                    for (int i = 0; i < change; i++)
                    {
                        elf.currentRecipe = elf.currentRecipe.next;
                    }
                }
            }

            // Last get the scores of the last 10
            string scores = "";
            for (int i = 0; i < 10; i++)
            {
                scores += state.recipes.ElementAt(input + i).value.ToString();
            }

            return scores;
        }

        public static int GetLeftOf(string input)
        {
            (List<Recipe> recipes, List<Elf> elves) state = GetStartingState();

            int addedCounter = 0;
            bool found = false;
            while (!found)
            {
                if (addedCounter % 100000 == 0)
                    Console.WriteLine(addedCounter);

                // First add the new recipes
                foreach (char number in state.elves.Sum(e => e.currentRecipe.value).ToString())
                {
                    Recipe newRecipe = new Recipe(int.Parse(number.ToString()));
                    newRecipe.previous = state.recipes.Last();
                    newRecipe.next = state.recipes.First();

                    state.recipes.Last().next = newRecipe;
                    state.recipes.First().previous = newRecipe;

                    state.recipes.Add(newRecipe);
                    addedCounter++;
                    
                    // While adding new recipes, a new string is made with last values
                    Recipe head = state.recipes.Last();
                    string last = head.value.ToString();
                    for (int i = 0; i < input.Length - 1; i++)
                    {
                        last = string.Concat(head.previous.value, last);
                        head = head.previous;
                    }
                    
                    // If this string matches input, it's time to return
                    if (input == last)
                    {
                        found = true;
                        break;
                    }
                }

                // Then give the elfs a new current
                foreach (Elf elf in state.elves)
                {
                    int change = elf.currentRecipe.value + 1;
                    for (int i = 0; i < change; i++)
                    {
                        elf.currentRecipe = elf.currentRecipe.next;
                    }
                }
            }

            return state.recipes.Count - input.Length;
        }

        public static (List<Recipe> recipes, List<Elf> elves) GetStartingState()
        {
            //First add all starting recipes as linked list
            Recipe recipe1 = new Recipe(3);
            Recipe recipe2 = new Recipe(7);
            recipe1.next = recipe2;
            recipe1.previous = recipe2;
            recipe2.next = recipe1;
            recipe2.previous = recipe1;

            List<Recipe> recipes = new List<Recipe>();
            recipes.Add(recipe1);
            recipes.Add(recipe2);

            //Then add all elves to loop over
            List<Elf> elves = new List<Elf>();
            elves.Add(new Elf(recipe1));
            elves.Add(new Elf(recipe2));

            return (recipes, elves);
        }

        public class Recipe
        {
            public int value;
            public Recipe next;
            public Recipe previous;

            public Recipe(int value)
            {
                this.value = value;
            }
        }

        public class Elf
        {
            public Recipe currentRecipe;

            public Elf(Recipe startingRecipe)
            {
                this.currentRecipe = startingRecipe;
            }
        }
    }
}
