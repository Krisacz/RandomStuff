using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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


            var inputStr = GenerateRandomInputString(26, 50);
            Console.WriteLine($"Input string: {inputStr}");
            var resultString = Process(inputStr, true);
            Console.WriteLine($"Result string: {resultString}");
            


            //Stress test
            /*
            for (var x = 0; x < 100; x++)
            {
                for (var i = 0; i <= 10; i++)
                {
                    var inputString = GenerateRandomInputString(i, 50);
                    Console.WriteLine($"Input string [Count: {i}]: \t {inputString}");
                    var resultString = Process(inputString, true);
                    Console.WriteLine($"Result string: {resultString}");
                    Console.WriteLine();
                    Console.WriteLine("------------------------------------------------------------------------------------------");
                }
            }
            */

            Console.WriteLine("################################################");
            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        private static string Process(string inputStr, bool additionalDebugOutput = false)
        {
            try
            {
                //First let's make sure that passed string will pass initial validation           
                #region INPUT STRING VALIDATION

                //Accept only A-Z letters ("jobs") and dependency character ">"
                var validCharacters = new Regex("[A-Z>]+");
                if (!string.Equals(validCharacters.Match(inputStr).Value, inputStr))
                    return "ERROR: Incorrect input string. Valid character are: A-Z and \">\".";

                //Fail if there is double (or more) dependency characters in a row (">")
                var multiDependencyCharacter = new Regex(">>");
                if (multiDependencyCharacter.IsMatch(inputStr))
                    return "ERROR: Incorrect input string. Dependency character used multiple times in a row.";

                //First character is ">"
                if (inputStr.Length > 0 && inputStr[0] == '>')
                    return "ERROR: Incorrect input string. Missing job. Input string starts with \">\".";

                //Last character is ">"
                if (inputStr.Length > 0 && inputStr[inputStr.Length - 1] == '>')
                    return "ERROR: Incorrect input string. Missing dependency job. Input string ends with \">\".";

                #endregion


                //Once we've validated input string lets "tokenize" our string in to usable "jobs" and their dependencies (if any)
                #region TOKENS

                //Tokens - 1st elemnt is a "job", 2nd is it's dependency (optional)            
                var tokens = new List<Tuple<string, string>>();
                var skipChar = 0;

                //Loop through all character in the input string 
                for (var i = 0; i < inputStr.Length; i++)
                {
                    //Skip character if we've added dependency job in previous loop cycle(s)
                    if (skipChar > 0)
                    {
                        skipChar--;
                        continue;
                    }

                    //Add new token - job and it's dependency job (if any)
                    var job = inputStr[i].ToString();
                    var dependencyJob = string.Empty;

                    //Check if further down the string there is current's job dependency                
                    if (i + 1 < inputStr.Length - 1)
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
                }

                #endregion


                //Now we have have "converted" input string into usable list - let's validate all tokens
                #region TOKENS VALIDATION

                //Main loop will go through all tokens - all internal loops will perform individual validations
                for (var i1 = 0; i1 < tokens.Count; i1++)
                {
                    var job1 = tokens[i1].Item1;
                    var dependencyJob1 = tokens[i1].Item2;

                    #region JOB DEFINED MULTIPLE TIMES                

                    if (tokens.Select(t => t.Item1).Where((job2, i2) => string.Equals(job1, job2) && i1 != i2).Any())
                    {
                        return $"ERROR: Job {job1} defined multiple times.";
                    }

                    #endregion

                    //Further validations are relying on dependency job being present so there is no point to carry on if there is none
                    if (string.IsNullOrWhiteSpace(dependencyJob1)) continue;

                    #region JOB DEPENDS ON IT SELF

                    if (string.Equals(job1, dependencyJob1))
                        return $"ERROR: Job can not depends on itself. [{job1} > {dependencyJob1}]";

                    #endregion

                    #region DEPENDENCY JOB NOT DEFINED                

                    var foundJob = tokens.Any(t2 => string.Equals(dependencyJob1, t2.Item1));
                    if (foundJob == false) return $"ERROR: Job {job1} depends on undefined {dependencyJob1} job.";

                    #endregion

                    #region CIRCULAR DEPENDENCIES             

                    var foundDependencyJobAsJob = false;
                    var searchingFor = dependencyJob1;
                    var exit = false;
                    var visitedJobs = new List<string>();
                    while (!foundDependencyJobAsJob && !string.IsNullOrWhiteSpace(searchingFor) && !exit)
                    {
                        var found = false;
                        foreach (var token in tokens)
                        {
                            if (!string.Equals(token.Item1, searchingFor)) continue;
                            visitedJobs.Add(searchingFor);
                            searchingFor = token.Item2;
                            found = true;
                            break;
                        }

                        if (visitedJobs.Any(x => x.Equals(searchingFor))) exit = true;  
                        if (!found) exit = true;
                        if (searchingFor == job1) foundDependencyJobAsJob = true;
                    }
                    if (foundDependencyJobAsJob) return "$ERROR: Jobs can not have circular dependencies";

                    #endregion
                }

                #endregion


                //Sort all jobs
                #region SORT JOBS

                //Mirror tokens list - this is the one we will be modifying as we can not modify an array while we are looping through it
                var sortedJobs = new List<Tuple<string, string>>(tokens);

                //We are processing all tokens untill all tokens are in correct place
                var correct = false;
                while (!correct)
                {
                    var changeMade = false;
                    //By moving "backwards" we are improving our performance and decreasing number of loop cycles
                    for (var a = tokens.Count - 1; a >= 0; a--)
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

                            if (minimumIndex >= jobIndex) continue;
                            sortedJobs.RemoveAt(jobIndex);
                            sortedJobs.Insert(minimumIndex, t);
                            changeMade = true;
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

                            if (maximumIndex <= jobIndex) continue;
                            sortedJobs.RemoveAt(jobIndex);
                            sortedJobs.Insert(maximumIndex, t);
                            changeMade = true;
                        }
                    }

                    correct = !changeMade;
                }

                #endregion


                //And finally - convert sorted list to output string
                #region CONVERT SORTED LIST

                var outputString = string.Join(string.Empty, sortedJobs.Select(x => x.Item1));

                #endregion


                //Debug info
                #region ADDITIONAL DEBUG OUTPUT

                if (additionalDebugOutput)
                {
                    Console.WriteLine();
                    Console.WriteLine("===== ADDITIONAL DEBUG OUTPUT =====");
                    Console.WriteLine();
                    Console.WriteLine("\tTOKENIZED INPUT STRING");
                    Console.WriteLine("\t----------------------");
                    foreach (var t in tokens) Console.WriteLine("\tJob: {0}{1}", t.Item1, string.IsNullOrWhiteSpace(t.Item2) ? " (no dependency)" : $" depends on {t.Item2}");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("\tSORTED JOBS");
                    Console.WriteLine("\t--------------");
                    foreach (var t in sortedJobs) Console.WriteLine("\tJob: {0}{1}", t.Item1, string.IsNullOrWhiteSpace(t.Item2) ? " (no dependency)" : $" depends on {t.Item2}");
                    Console.WriteLine();
                    Console.WriteLine("===================================");
                    Console.WriteLine();
                }

                #endregion


                return outputString;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return "EXCEPTION";
        }

        private static string GenerateRandomInputString(int count, int dependencyChance = 25)
        {
            try
            {
                if (count < 0 || count > 26) throw new Exception("Count parameter out of range - 0-26");
                if (dependencyChance < 0 || dependencyChance > 100) throw new Exception("DependencyChance parameter out of range - 0-100");


                //Init
                var allJobs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
                var random = new Random(Guid.NewGuid().GetHashCode()); //to make it even more random
                var finalString = string.Empty;

                //First generate list of jobs based on count
                var selectedJobs = new List<string>();
                for (var i = 1; i <= count; i++)
                {
                    var randomIndex = random.Next(allJobs.Count);
                    var randomJob = allJobs[randomIndex];
                    allJobs.RemoveAt(randomIndex);
                    selectedJobs.Add(randomJob.ToString());
                }

                //Now create final string while randomly adding dependency            
                foreach (var job in selectedJobs)
                {
                    var randomChance = random.Next(101);
                    if (count > 1 && randomChance <= dependencyChance)
                    {
                        var found = false;
                        var dependencyJob = string.Empty;
                        while (!found)
                        {
                            var randomIndex = random.Next(selectedJobs.Count);
                            var randomJob = selectedJobs[randomIndex];
                            if (randomJob == job) continue;
                            dependencyJob = randomJob;
                            found = true;
                        }
                        finalString += $"{job}>{dependencyJob}";
                    }
                    else
                    {
                        finalString += job;
                    }
                }

                //Output
                return finalString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return string.Empty;
        }
    }
}
