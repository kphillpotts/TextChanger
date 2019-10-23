using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.IO;

namespace TextSetter
{
	class Program
	{
		public class Options
		{
			[Option('i', "input", Default = "", Required = false, HelpText = "If specified, app runs in interactive mode")]
			public string InputFilename { get; set; }

			[Option('t', "text", Default = "", Required = true, HelpText = "Text to output, or no value to clear")]
			public string Text { get; set; }

			[Option('o', "output", Default = "output.txt", Required = false, HelpText = "Output filename. Defaults to 'output.txt' if not specified.")]
			public string OutputFilename { get; set; }

			[Usage(ApplicationAlias = "textsetter")]
			public static IEnumerable<Example> Examples
			{
				get
				{
					return new List<Example>() {
						new Example("Write a single line of text to the output file", new Options { OutputFilename = "outputfile.txt", Text = "This will end up in the file" }),
						new Example("Run the app interactively to write one of the lines of text from the input into the output file", new Options { OutputFilename = "outputfile.txt", InputFilename = "multiplelinesoftext.txt" }),
						new Example("Write an initial line of text to the output file and then continue in interactive mode", new Options { OutputFilename = "outputfile.txt", InputFilename = "multiplelinesoftext.txt", Text = "Initial text" })
					};
				}
			}
		}

		static void Main(string[] args)
		{
			Console.WriteLine("TextSetter 4000 - text setting will never be the same again");
			Console.WriteLine();

			Parser.Default.ParseArguments<Options>(args)
				.WithParsed<Options>(RunApp)
				.WithNotParsed<Options>(HandleParserErrors);
		}

		private static void HandleParserErrors(IEnumerable<Error> errors)
		{
			foreach (var error in errors)
			{
				Console.WriteLine(error);
			}
		}

		private static void RunApp(Options options)
		{
			Console.WriteLine($"* Writing '{options.Text}' to file '{options.OutputFilename}'");

			File.WriteAllText(options.OutputFilename, options.Text);
			if (!string.IsNullOrWhiteSpace(options.InputFilename))
			{
				Console.WriteLine("* Interactive mode - use CTRL+C to exit.");
				Console.WriteLine();

				var textLines = File.ReadAllLines(options.InputFilename);
				while (true)
				{
					var selector = '1';
					foreach (var textLine in textLines)
					{
						Console.WriteLine($"{selector} - {textLine}");
						selector++;
					}

					Console.WriteLine();
					Console.WriteLine("Make a selection...");
					var selectedKey = Console.ReadKey().KeyChar;
					Console.Clear();
					var text = textLines[selectedKey - '1'];
					Console.WriteLine($"* Selected '{selectedKey}'. Writing '{text}' to '{options.OutputFilename}'");
					Console.WriteLine();
					File.WriteAllText(options.OutputFilename, text);
				}
			}
		}
	}
}
