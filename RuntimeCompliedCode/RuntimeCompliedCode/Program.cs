using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RuntimeCompliedCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileLines = File.ReadAllLines("code.txt");

            StringBuilder code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("namespace First");
            code.AppendLine("{");
            code.AppendLine("public class Program");
            code.AppendLine("{");
            code.AppendLine("public static void Main()");
            code.AppendLine("{");
            foreach (var l in fileLines) code.AppendLine(l);            
            code.AppendLine("}");
            code.AppendLine("}");
            code.AppendLine("}");
                        
            //Create the provider and parameters of the compiler
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            //Define parameters of the compiler (optional) – at this point, we can add a reference to external libraries.
            //We can also define whether our compiled code will be generated only in the memory or into the DLL or EXE file
            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = false;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = true;
            

            //Compile assembly
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code.ToString());

            //Check errors
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                Console.WriteLine(sb.ToString());                
            }

            //Get assembly, type and the Main method
            Assembly assembly = results.CompiledAssembly;
            Type program = assembly.GetType("First.Program");
            MethodInfo main = program.GetMethod("Main");

            //Run it
            main.Invoke(null, null);


            Console.WriteLine();
            Console.WriteLine("===========================");
            Console.WriteLine("Done!");
            Console.ReadKey();            
        }
    }
}
