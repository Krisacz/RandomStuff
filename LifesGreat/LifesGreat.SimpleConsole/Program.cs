using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LifesGreat.SimpleConsole
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            #region MAIN TEST
            //var start = DateTime.Now;
            //Test("", "");
            //Test("A", "A");
            //Test("ABC", "ABC");
            //Test("AB>CC", "ACB");
            //Test("AB>CC>FD>AE>BF", "AFCBDE");
            //Test("ABC>C", null);
            //Test("AB>CC>FD>AEF>B", null);
            //var stop = DateTime.Now;
            //var timeTaken = stop - start;
            //Console.WriteLine("------------------------------");
            //Console.WriteLine($"Total duration: {timeTaken}");
            //Console.ReadKey();
            #endregion

            #region SIMPLE TEST - NO CONSOLE FORMAT
            //Simple test
            var simpleStart = DateTime.Now;
            SimpleTest("");
            SimpleTest("A");
            SimpleTest("ABC");
            SimpleTest("AB>CC");
            SimpleTest("AB>CC>FD>AE>BF");
            SimpleTest("ABC>C");
            SimpleTest("AB>CC>FD>AEF>B");
            var simpleEnd = DateTime.Now - simpleStart;
            Console.WriteLine($"Total Duration: {simpleEnd}");
            Console.ReadKey();
            #endregion

            #region STRESS TEST WITH RANDOMLY GENERATED INPUT
            ////Stress test
            //for (var x = 0; x < 10; x++)
            //{
            //    for (var i = 0; i <= 10; i++)
            //    {
            //        var inputString = GenerateRandomInputString(i, 50);
            //        Console.WriteLine($"Input string [Count: {i}]: \t {inputString}");
            //        var resultString = Process(inputString, true);
            //        Console.WriteLine($"Result string: {resultString}");
            //        Console.WriteLine();
            //        Console.WriteLine("------------------------------------------------------------------------------------------");
            //    }
            //}
            //Console.ReadKey();
            #endregion
        }

        #region PROCESS JOBS
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
                    return "ERROR: Incorrect input string. Missing dependent job. Input string ends with \">\".";

                #endregion


                //Once we've validated input string lets "tokenize" our string in to usable "jobs" and their dependencies (if any)
                #region TOKENS

                //Tokens - 1st elemnt is a "job", 2nd is dependent job (optional)            
                var tokens = new List<Tuple<string, string>>();
                var skipChar = 0;

                //Loop through all character in the input string 
                for (var i = 0; i < inputStr.Length; i++)
                {
                    //Skip character if we've added dependent job in previous loop cycle(s)
                    if (skipChar > 0)
                    {
                        skipChar--;
                        continue;
                    }

                    //Add new token - job and it's dependent job (if any)
                    var job = inputStr[i].ToString();
                    var dependentJob = string.Empty;

                    //Check if further down the string there is current's job dependency                
                    if (i + 1 < inputStr.Length - 1)
                    {
                        if (inputStr[i + 1] == '>')
                        {
                            dependentJob = inputStr[i + 2].ToString();
                            skipChar = 2;
                        }
                    }

                    //Create new job+dependent job
                    var token = new Tuple<string, string>(job, dependentJob);
                    tokens.Add(token);
                }

                #endregion


                //Now we have have "converted" input string into usable list - let's validate all tokens
                #region TOKENS VALIDATION

                //Main loop will go through all tokens - all internal loops will perform individual validations
                for (var i1 = 0; i1 < tokens.Count; i1++)
                {
                    var job1 = tokens[i1].Item1;
                    var dependentJob1 = tokens[i1].Item2;

                    #region JOB DEFINED MULTIPLE TIMES                

                    if (tokens.Select(t => t.Item1).Where((job2, i2) => string.Equals(job1, job2) && i1 != i2).Any())
                    {
                        return $"ERROR: Job {job1} defined multiple times.";
                    }

                    #endregion

                    //Further validations are relying on dependent job being present so there is no point to carry on if there is none
                    if (string.IsNullOrWhiteSpace(dependentJob1)) continue;

                    #region JOB DEPENDS ON IT SELF

                    if (string.Equals(job1, dependentJob1))
                        return $"ERROR: Job can not depends on itself. [{job1} > {dependentJob1}]";

                    #endregion

                    #region DEPENDENT JOB NOT DEFINED                

                    var foundJob = tokens.Any(t2 => string.Equals(dependentJob1, t2.Item1));
                    if (foundJob == false) return $"ERROR: Job {job1} depends on undefined {dependentJob1} job.";

                    #endregion

                    #region CIRCULAR DEPENDENCIES             

                    var foundDependentJobAsJob = false;
                    var searchingFor = dependentJob1;
                    var exit = false;
                    var visitedJobs = new List<string>();
                    while (!foundDependentJobAsJob && !string.IsNullOrWhiteSpace(searchingFor) && !exit)
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
                        if (searchingFor == job1) foundDependentJobAsJob = true;
                    }
                    if (foundDependentJobAsJob) return "$ERROR: Jobs can not have circular dependencies";

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
                        var dependentJob = t.Item2;
                        var jobIndex = sortedJobs.IndexOf(t);

                        //If job does't have dependent job check if any other jobs depends on it and place it before it (lowest possible index)
                        if (string.IsNullOrWhiteSpace(dependentJob))
                        {
                            //Initialize with maximum index
                            var minimumIndex = tokens.Count - 1;
                            for (var i = 0; i < sortedJobs.Count; i++)
                            {
                                var dependentJob1 = sortedJobs[i].Item2;
                                if (string.Equals(job, dependentJob1)) minimumIndex = Math.Min(minimumIndex, i);
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
                                if (string.Equals(dependentJob, job2)) maximumIndex = Math.Max(maximumIndex, i);
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

            return null;
        }
        #endregion

        #region SIMPLE TEST
        private static void SimpleTest(string inputString, bool silent = false)
        {
            if (silent)
            {
                Process(inputString);
            }
            else
            {
                Console.WriteLine($"Input string:\t{inputString}");
                Console.WriteLine($"Result string:\t{Process(inputString)}");
                Console.WriteLine();
            }
        }
        #endregion

        #region TEST
        private static void Test(string inputString, string expectedResult, bool additionalDebugOutput = false)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var start = DateTime.Now;

            Console.Write("Input string: \t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(string.IsNullOrWhiteSpace(inputString) ? "(empty)" : inputString);
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Expected: \t");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(expectedResult == string.Empty ? "(empty)" : expectedResult ?? "ERROR");
            Console.ForegroundColor = ConsoleColor.White;

            var resultString = Process(inputString, additionalDebugOutput);
            Console.Write("Result string: \t");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.IsNullOrWhiteSpace(resultString) ? "(empty)" : resultString);
            Console.ForegroundColor = ConsoleColor.White;

            var testResult = string.Equals(resultString, expectedResult);
            Console.Write("TEST RESULT: \t");
            Console.ForegroundColor = testResult ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(testResult ? "PASSED" : "FAILED");
            Console.ForegroundColor = ConsoleColor.White;

            var end = DateTime.Now - start;
            Console.Write("Duration: \t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(end);
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();
        }
        #endregion

        #region RANDOM INPUT STRING GENERATOR
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
        #endregion
    }
}
