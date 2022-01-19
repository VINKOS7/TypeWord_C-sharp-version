using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TypeWord
{
    public static class FuncСommon
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
    }
    class TypeWord
    {
        const char SymbolStartPartWord = '?';
        const string PathPriority = "../LibraryMorphemes/Priority.txt";
        const string PathLibraryMorphemes = "../LibraryMorphemes/";
        public class Word
        {
            public string Symbols {get;}
            public string Type {get;}

            public Word(string _Symbols)
            {
                Symbols = _Symbols;

                Type = IsType(Symbols);
            }

            private string IsType(string word)
            {
                List<string[]> Morphemes = new();

                string[] MorphemesNamesFiles;
                string[] MorphemesNamesFilesPriority;

                if (Directory.Exists(PathLibraryMorphemes))
                {
                    MorphemesNamesFiles = Directory.GetFiles(PathLibraryMorphemes);

                    if (File.Exists(PathPriority))
                    {
                        MorphemesNamesFilesPriority = File.ReadAllLines(PathPriority);

                        for (int i = 0; i < MorphemesNamesFiles.Length; i++)
                            for (int j = 0; j < MorphemesNamesFilesPriority.Length; j++)
                                if (MorphemesNamesFiles[i].Contains(MorphemesNamesFilesPriority[j]))
                                    FuncСommon.Swap(ref MorphemesNamesFiles[i], ref MorphemesNamesFiles[j]);
                    }

                    for (int i = 0; i < MorphemesNamesFiles.Length; i++)
                        Morphemes.Add(File.ReadAllLines(MorphemesNamesFiles[i]));

                    for (int i = 0; i < Morphemes.Count; i++)
                    {
                        if (SearchMorpheme(word, Morphemes[i]) != null)
                        {
                            Regex NameFile = new Regex(@"\/\w{1,}\.");

                            return NameFile.Match(MorphemesNamesFiles[i]).Value.Trim('/','.');
                        }
                    }
                }

                return "Noun";
            }
            private string SearchMorpheme(string word, string[] Morphemes)
            {
                for (int i = 0; i < Morphemes.Length; i++)
                {
                    if (!Morphemes[i].Contains(SymbolStartPartWord))
                    {
                        if (word.Length - Morphemes[i].Length >= 0)
                            if (word.Substring(word.Length - Morphemes[i].Length) == Morphemes[i])
                                return Morphemes[i];
                    }
                    else
                    {
                        int bufIndex = i;
                        i++;

                        if (word.Substring(word.Length - (Morphemes[bufIndex].Length - 1), Morphemes[bufIndex].Length - 1)
                            ==
                            Morphemes[bufIndex].Substring(1, Morphemes[bufIndex].Length - 1))
                        {
                            while (i < Morphemes.Length)
                            {
                                if (!Morphemes[i].Contains(SymbolStartPartWord))
                                {
                                    if (word.Length - Morphemes[i].Length >= 0)
                                        if (word.Substring(0, Morphemes[i].Length) == Morphemes[i])
                                            return Morphemes[i];
                                }
                                else break;

                                i++;
                            }
                        }
                        else
                        {
                            while (!Morphemes[i].Contains(SymbolStartPartWord))
                                if (i < Morphemes.Length - 1) i++;
                                else break;

                            i--;
                        }
                    }
                }
                return null;
            }
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TypeWord.Word WordTXT = new("Прыгающий");
        }
    }
}
