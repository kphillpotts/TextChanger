using CommandLine;
using System;

namespace TextSetter
{
    class Program
    {
        public class Options
        {
            [Option('t', "text", Default="", Required = false, HelpText = "Text to output, or no value to clear")]
            public string Text { get; set; }

            [Option('o', "output", Default="", Required = false, HelpText = "Output filename")]
            public string OutputFile { get; set; }
        }

        static void Main(string[] args)
        {

            try
            {
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(o =>
                      {
                          string fileName;
                          if (string.IsNullOrWhiteSpace(o.OutputFile))
                              fileName = System.IO.Path.Combine(Environment.CurrentDirectory, "output.txt");
                          else
                              fileName = o.OutputFile;

                          System.IO.File.WriteAllText(fileName, o.Text);
                      });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            
        }
    }
}
