namespace MetronomeApp
{
    class Prorgam
    {
        static void Main()
        {
            Visuals.DisplayTitle();
            Console.WriteLine("Choose metronome type:");
            Console.WriteLine("For Regular: R");
            Console.WriteLine("For Irregular: I");

            ConsoleKeyInfo choice;

            while (true)
            {
                choice = Console.ReadKey();
                if (choice.Key == ConsoleKey.R)
                {
                    Metronome metronome = new();
                    metronome.Init();
                    Interact(metronome);
                    break;
                }
                else if (choice.Key == ConsoleKey.I)
                {
                    IrregularMetronome metronome = new();
                    metronome.Init();
                    Interact(metronome);
                    break;
                }
                else
                {
                    Console.WriteLine(" is not a valid choice");
                }
            }
        }

        static void Interact(Metronome metronome)
        {
            Console.Clear();
            Visuals.DisplayTitle();
            Visuals.DisplayInfo(metronome);
            ConsoleKeyInfo command;
            do
            {
                command = Console.ReadKey(true);

                switch (command.Key)
                {
                    case ConsoleKey.Spacebar:
                        metronome.StartPause();
                        break;
                    case ConsoleKey.R:
                        metronome.Reset();
                        break;
                    case ConsoleKey.UpArrow:
                        metronome.BPM += 1;
                        Console.WriteLine(metronome.BPM);
                        break;
                    case ConsoleKey.DownArrow:
                        metronome.BPM -= 1;
                        break;
                    default:
                        break;
                }
            } while (command.Key != ConsoleKey.Q);
            Visuals.DisplayBye();
        }
    }
}