using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputStr = "B>AA>CC";
            Console.WriteLine($"Input string: {inputStr}");            
            var resultString = Process(inputStr, true);
            Console.WriteLine($"Result string: {resultString}");
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
            if (inputStr[inputStr.Length - 1] == '>') return "ERROR: Incorrect input string. Missing dependency task. Input string ends with \">\".";
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


            #region RE-ORDER JOBS
            //Mirror tokens list - this is the one we will be modifying as we can not modify an array while we are looping through it
            var sortedJobs = new List<Tuple<string, string>>(tokens);

            foreach (var t in tokens)
            {
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

                    continue;
                }
            }
            #endregion

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
                Console.WriteLine("\tRE-ORDERED JOBS");
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

            return inputStr;
        }
    }
}
