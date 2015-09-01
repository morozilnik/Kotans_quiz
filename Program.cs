using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleParser
{
    class Program
    {
        static void PrintCommandNotSupported(String command)
        {
            Console.Write("Command " + command + " is not supported, ");
            Console.WriteLine("use CommandParser.exe /? to see set of allowed commands");
        }

        static Boolean IsHelpOption(String option)
        {
            return (option == "/?" || option == "/help" || option == "-help");
        }

        static Boolean IsKeyOption(String option)
        {
            return option == "-k";
        }

        static Boolean IsPingOption(String option)
        {
            return option == "-ping";
        }

        static Boolean IsPrintOption(String option)
        {
            return option == "-print";
        }

        static void PrintHelp()
        {
            Console.WriteLine("Program Usage:");
            Console.WriteLine("CommandParser.exe [/?] [/help] [-help] [-k key value] [-ping] [-print <print a value>]");
            Console.WriteLine();
            Console.WriteLine("[/?] [/help] [-help] - Show this message");
            Console.WriteLine("[-k key value] - Show table key-value");
            Console.WriteLine("[-ping] - Make a sound, write 'Pinging ...'");
            Console.WriteLine("[-print <message>] - Prints <message>");
        }

        static void FinishKey(Boolean active, String key)
        {
            if (active)
            {
                active = false;
                if (key.Length > 0)
                {
                    Console.WriteLine(key + " - null");
                }
            }
        }

        static Boolean ScanForNonValidAndHelp(string[] args)
        {
            //Occurence variables for duplicate search

            Boolean keyActive = false;
            for (int i = 0; i < args.Length; i++)
            {
                //If help is found among variables it is printed
                if (IsHelpOption(args[i]))
                {
                    PrintHelp();
                    return true;
                }
                if (IsKeyOption(args[i]))
                {
                    keyActive = true;
                    continue;
                }
                if (IsPingOption(args[i]))
                {
                    keyActive = false;
                    continue;
                }
                if (IsPrintOption(args[i]))
                {
                    keyActive = false;
                    i++;
                    continue;
                }

                if (keyActive)
                {
                    continue;
                }
                else
                {
                    PrintCommandNotSupported(args[i]);
                    return true;
                }
            }
            return false;
        }
        static bool nonEmpty(String s)
        {
            return s != "";
        }

        static void Main(string[] args)
        {
            char[] separator = { ' ' };
            while (true)
            {
                int count = args.Length;
                if (count < 1) PrintHelp();
                else
                {
                    if (ScanForNonValidAndHelp(args))
                    {
                        String result = Console.ReadLine();
                        args = Array.FindAll(result.Split(separator), nonEmpty).ToArray();
                        continue;
                    }
                }
                Boolean isKeyActive = false;
                String Key = "";
                for (int i = 0; i < count; i++)
                {
                    if (IsPingOption(args[i]))
                    {
                        FinishKey(isKeyActive, Key);
                        System.Media.SystemSounds.Beep.Play();
                        Console.WriteLine("Pinging ...");
                        continue;
                    }
                    if (IsPrintOption(args[i]))
                    {
                        FinishKey(isKeyActive, Key);
                        if (i + 1 == count)
                        {
                            Console.WriteLine("Error! No message to print");
                        }
                        else
                        {
                            i++;
                            Console.WriteLine(args[i]);
                        }
                        continue;
                    }
                    if (IsKeyOption(args[i]))
                    {
                        isKeyActive = true;
                        continue;
                    }

                    if (isKeyActive)
                    {
                        if (Key.Length > 0)
                        {
                            Console.WriteLine(Key + " - " + args[i]);
                            Key = "";
                        }
                        else
                        {
                            Key = args[i];
                        }
                    }
                    else
                    {
                        PrintCommandNotSupported(args[i]);
                    }
                }
                if (isKeyActive) FinishKey(isKeyActive, Key);

                String newCommands = Console.ReadLine();
                args = Array.FindAll(newCommands.Split(separator), nonEmpty).ToArray();

            }
        }
    }
}
