using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var inputStr = ""; //Result: empty set
            //var inputStr = "A"; //Result: A
            //var inputStr = "ABC"; //Result: ABC
            //var inputStr = "AB>CC"; //Result: ACB
            //var inputStr = "AB>CC>FD>AE>BF"; //Result: AFCBDE
            //var inputStr = "ABC>C"; //Result: error - job can not depend on itself
            //var inputStr = "AB>CC>FD>AEF>B"; //Result: eror - circular dependenties
            
            for (int i = 0; i <= 10; i++)
            {
                var inputString = GenerateRandomInputString(i);
                Console.WriteLine($"Input string [Count: {i}]: \t {inputString}");
                var resultString = Process(inputString, true);
                Console.WriteLine($"Result string: {resultString}");
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------------------------------------------------");
            }

            /*
            var inputStr = "AB>CC>FD>AE>BF";
            Console.WriteLine($"Input string: {inputStr}");            
            var resultString = Process(inputStr, true);
            Console.WriteLine($"Result string: {resultString}");
            */

            Console.ReadKey();
        }

        private static string Process(string inputStr, bool additionalDebugOutput = false)
        {
            //First let's make sure that passed string will pass initial validation            
            #region INPUT STRING VALIDATION
            //Accept only A-Z letters ("jobs") and dependency character ">"
            var validCharacters = new Regex("[A-Z>]+");
            if(!string.Equals(validCharacters.Match(inputStr).Value, inputStr)) return "ERROR: Incorrect input string. Valid character are: A-Z and \">\".";

            //Fail if there is double (or more) dependency characters in a row (">")
            var multiDependencyCharacter = new Regex(">>");
            if(multiDependencyCharacter.IsMatch(inputStr)) return "ERROR: Incorrect input string. Dependency character used multiple times in a row.";

            //Last character is ">"
            if (inputStr.Length > 0 && inputStr[inputStr.Length - 1] == '>') return "ERROR: Incorrect input string. Missing dependency task. Input string ends with \">\".";
            #endregion


            //Once we've validated input string lets "tokenize" our string in to usable "jobs" and their dependencies (if any)
            #region TOKENS
            //Tokens - 1st elemnt is a "job", 2nd is it's dependency (optional)            
            var tokens = new List<Tuple<string,string>>();            
            var skipChar = 0;

            //Loop through all character in the input string 
            for(var i = 0; i < inputStr.Length; i++)
            {
                //Skip character if we've added dependency job in previous loop cycle(s)
                if(skipChar > 0)
                {
                    skipChar--;
                    continue;
                }

                //Add new token - job and it's dependency job (if any)
                var job = inputStr[i].ToString();
                var dependencyJob = string.Empty; 
                
                //Check if further down the string there is current's job dependency                
                if(i + 1 < inputStr.Length - 1)
                {
                    if (inputStr[i + 1] == '>')
                    {
                        dependencyJob = inputStr[i + 2].ToString();
                        skipChar = 2;
                    }
                }

                //Create new job+dependency job
                var token = new Tuple<string, string>(job, dependencyJob);
                tokens.Add(token);
            };
            #endregion


            //Now we have have "converted" input string into usable list - let's validate all tokens
            #region TOKENS VALIDATION
            //Main loop will go through all tokens - all internal loops will perform individual validations
            for(var i1 = 0; i1 < tokens.Count; i1++)
            {
                var job1 = tokens[i1].Item1;
                var dependencyJob1 = tokens[i1].Item2;

                #region JOB DEFINED MULTIPLE TIMES                
                for (var i2 = 0; i2 < tokens.Count; i2++)
                {
                    var job2 = tokens[i2].Item1;
                    //When same job with different indices is defined - throw an error                     
                    if (string.Equals(job1, job2) && i1 != i2) return $"ERROR: Job {job1} defined multiple times.";
                }
                #endregion

                //Further validations are relying on dependency job being present so there is no point to carry on if there is none
                if (string.IsNullOrWhiteSpace(dependencyJob1)) continue;

                #region DEPENDENCY JOB NOT DEFINED                
                var foundTask = false;
                foreach (var t2 in tokens)
                {
                    if (string.Equals(dependencyJob1, t2.Item1))
                    {
                        foundTask = true;
                        break;
                    }
                }
                if (foundTask == false) return $"ERROR: Task {job1} depends on undefined {dependencyJob1} task.";
                #endregion

                #region CIRCULAR DEPENDENCIES                                            
                foreach (var t2 in tokens)
                {
                    //TODO MAKE PROPER HEAD TO TAIL TO HEAD CHECK
                    /*
                    var task2 = t2.Item1;
                    var dependencyTask2 = t2.Item2;
                    if (string.Equals(job1, dependencyTask2) && string.Equals(dependencyJob1, task2))
                        return $"ERROR: Two different tasks can not depend on each other ({job1}>{dependencyJob1} and {task2}>{dependencyTask2}).";
                    */
                }
                #endregion
            }
            #endregion


            //Sort all jobs
            #region SORT JOBS
            //Mirror tokens list - this is the one we will be modifying as we can not modify an array while we are looping through it
            var sortedJobs = new List<Tuple<string, string>>(tokens);

            //We could fully sort it by either "running" through all tokens twice or one decremental "run"
            for(var a = tokens.Count - 1; a >= 0; a--)
            {
                var t = tokens[a];
                var job = t.Item1;
                var dependencyJob = t.Item2;
                var jobIndex = sortedJobs.IndexOf(t);

                //If job does't have dependency job check if any other jobs depends on it and place it before it (lowest possible index)
                if (string.IsNullOrWhiteSpace(dependencyJob))
                {
                    //Initialize with maximum index
                    var minimumIndex = tokens.Count - 1;
                    for (var i = 0; i < sortedJobs.Count; i++)
                    {
                        var dependencyJob1 = sortedJobs[i].Item2;
                        if (string.Equals(job, dependencyJob1)) minimumIndex = Math.Min(minimumIndex, i);
                    }

                    var newJobIndex = Math.Min(jobIndex, minimumIndex);
                    sortedJobs.RemoveAt(jobIndex);
                    sortedJobs.Insert(newJobIndex, t);
                }
                //...otherwise place current job after job it depends on
                else
                {
                    //Initialize with maximum index
                    var maximumIndex = 0;
                    for (var i = 0; i < sortedJobs.Count; i++)
                    {
                        var job2 = sortedJobs[i].Item1;
                        if (string.Equals(dependencyJob, job2)) maximumIndex = Math.Max(maximumIndex, i);
                    }

                    var newJobIndex = Math.Max(jobIndex, maximumIndex);
                    sortedJobs.RemoveAt(jobIndex);
                    sortedJobs.Insert(newJobIndex, t);
                }
            }
            #endregion


            //And finally - convert sorted list to output string
            #region CONVERT SORTED LIST
            var outputString = string.Join(string.Empty, sortedJobs.Select(x=>x.Item1));            
            #endregion


            //Debug info
            #region ADDITIONAL DEBUG OUTPUT
            if(additionalDebugOutput)
            {
                Console.WriteLine();
                Console.WriteLine("===== ADDITIONAL DEBUG OUTPUT =====");
                Console.WriteLine();
                Console.WriteLine("\tTOKENIZED INPUT STRING");
                Console.WriteLine("\t----------------------");
                foreach (var t in tokens)
                {                
                    Console.WriteLine(string.Format("\tJob: {0}{1}", t.Item1, string.IsNullOrWhiteSpace(t.Item2) ? " (no dependency)" : $" depends on {t.Item2}"));
                };
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\tSORTED JOBS");
                Console.WriteLine("\t--------------");
                foreach (var t in sortedJobs)
                {
                    Console.WriteLine(string.Format("\tJob: {0}{1}", t.Item1, string.IsNullOrWhiteSpace(t.Item2) ? " (no dependency)" : $" depends on {t.Item2}"));
                };
                Console.WriteLine();
                Console.WriteLine("===================================");
                Console.WriteLine();
            }
            #endregion

            return outputString;
        }

        private static string GenerateRandomInputString(int count, int dependencyChance = 25)
        {
            //Init
            var allJobs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
            var random = new Random(Guid.NewGuid().GetHashCode()); //to make it even more random
            var finalString = string.Empty;

            //First generate list of jobs based on count
            var selectedJobs = new List<string>();
            for(var i = 1; i <= count; i++)
            {
                var randomIndex = random.Next(allJobs.Count);
                var randomJob = allJobs[randomIndex];
                allJobs.RemoveAt(randomIndex);
                selectedJobs.Add(randomJob.ToString());
            }

            //Now create final string while randomly adding dependency            
            foreach(var job in selectedJobs)
            {
                var randomChance = random.Next(101);
                if(count > 1 && randomChance <= dependencyChance)
                {
                    var found = false;
                    var dependencyJob = string.Empty;
                    while(!found)
                    {
                        var randomIndex = random.Next(selectedJobs.Count);
                        var randomJob = selectedJobs[randomIndex];
                        if(randomJob != job)
                        {
                            dependencyJob = randomJob;
                            found = true;
                        }
                    }
                    finalString += string.Format("{0}>{1}", job, dependencyJob);
                }
                else
                {
                    finalString += job;
                }
            }

            //Output
            return finalString;
        }
    }
}
