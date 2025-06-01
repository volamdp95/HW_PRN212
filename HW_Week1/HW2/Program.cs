// A utility to analyze text files and provide statistics
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("File Analyzer - .NET Core");
            Console.WriteLine("This tool analyzes text files and provides statistics.");
            
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path as a command-line argument.");
                Console.WriteLine("Example: dotnet run myfile.txt");
                return;
            }
            
            string filePath = args[0];
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' does not exist.");
                return;
            }
            
            try
            {
                Console.WriteLine($"Analyzing file: {filePath}");
                
                // Read the file content
                string content = File.ReadAllText(filePath);
                
                // TODO: Implement analysis functionality
                // 1. Count words
                // 2. Count characters (with and without whitespace)
                // 3. Count sentences
                // 4. Identify most common words
                // 5. Average word length
                
                // Example implementation for counting lines:
                int lineCount = File.ReadAllLines(filePath).Length;
                Console.WriteLine($"Number of lines: {lineCount}");
                
                string[] words = Regex.Split(content.ToLower(), @"\W+").Where(w => w.Length > 0).ToArray();
                Console.WriteLine($"Words: {words.Length}");

                int charWithSpaces = content.Length;
                int charWithoutSpaces = content.Count(c => !char.IsWhiteSpace(c));
                Console.WriteLine($"Characters (with spaces): {charWithSpaces}");
                Console.WriteLine($"Characters (no spaces): {charWithoutSpaces}");

                int sentenceCount = Regex.Matches(content, @"[.!?]").Count;
                Console.WriteLine($"Sentences: {sentenceCount}");

                var wordGroups = words.GroupBy(w => w).OrderByDescending(g => g.Count());
                var mostCommonWord = wordGroups.First().Key;
                var mostCommonCount = wordGroups.First().Count();

                Console.WriteLine($"Most common word: '{mostCommonWord}' ({mostCommonCount} times)");
                Console.WriteLine($"Average word length: {words.Average(w => w.Length):0.00}");
                // TODO: Additional analysis to be implemented
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file analysis: {ex.Message}");
            }
        }
    }
}