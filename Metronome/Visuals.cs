namespace MetronomeApp
{
    class Visuals
    {
        private readonly int beats;
        private readonly List<ConsoleColor> sectionColors;

        public Visuals(int beats)
        {
            this.beats = beats;
            sectionColors = Enumerable.Repeat(ConsoleColor.White, beats).ToList();
        }

        public void UpdateStrong()
        {
            sectionColors[0] = ConsoleColor.Green;
            DisplayVisuals();
            Thread.Sleep(100);
            sectionColors[0] = ConsoleColor.White;
        }

        public void UpdateWeak(int currentBeat)
        {
            sectionColors[currentBeat % beats] = ConsoleColor.Yellow;
            DisplayVisuals();
            Thread.Sleep(100);
            sectionColors[currentBeat % beats] = ConsoleColor.White;
        }

        public void UpdateSecondaryStrong(int currentBeat)
        {
            sectionColors[currentBeat % beats] = ConsoleColor.Blue;
            DisplayVisuals();
            Thread.Sleep(100);
            sectionColors[currentBeat % beats] = ConsoleColor.White;
        }

        private void DisplayVisuals()
        {
            int sectionWidth = (Console.WindowWidth - 4 - (beats - 1)) / beats;

            Console.SetCursorPosition(0, 9);

            Console.Write(new string(' ', 2));

            for (int i = 0; i < beats; i++)
            {
                Console.SetCursorPosition(2 + i * (sectionWidth + 1), Console.CursorTop);
                Console.ForegroundColor = sectionColors[i];
                Console.Write(new string('█', sectionWidth));
            }
        }

        public static void DisplayTitle()
        {
            Console.SetCursorPosition(31, 0);
            Console.WriteLine("  __  __      _                                        ");
            Console.SetCursorPosition(31, Console.CursorTop);
            Console.WriteLine(" |  \\/  |    | |                                       ");
            Console.SetCursorPosition(31, Console.CursorTop);
            Console.WriteLine(" | \\  / | ___| |_ _ __ ___  _ __   ___  _ __ ___   ___ ");
            Console.SetCursorPosition(31, Console.CursorTop);
            Console.WriteLine(" | |\\/| |/ _ \\ __| '__/ _ \\| '_ \\ / _ \\| '_ ` _ \\ / _ \\");
            Console.SetCursorPosition(31, Console.CursorTop);
            Console.WriteLine(" | |  | |  __/ |_| | | (_) | | | | (_) | | | | | |  __/");
            Console.SetCursorPosition(31, Console.CursorTop);
            Console.WriteLine(" |_|  |_|\\___|\\__|_|  \\___/|_| |_|\\___/|_| |_| |_|\\___|");
            Console.SetCursorPosition(31, Console.CursorTop);
            Console.WriteLine("                                                       ");
        }

        public static void DisplayInfo(Metronome metronome)
        {
            string info = $"Beats per measure: {metronome.Beats} BPM: {metronome.BPM}";
            Console.SetCursorPosition((Console.WindowWidth - info.Length) / 2, Console.CursorTop);
            Console.WriteLine(info);
            string instructions = "Commands: Space to start / pause; R to reset; Q to quit";
            Console.SetCursorPosition((Console.WindowWidth - instructions.Length) / 2, Console.CursorTop);
            Console.WriteLine(instructions);
        }

        public static void DisplayBye()
        {
            Console.WriteLine("\n");
            Console.WriteLine(" _                  _                \r\n | |__  _   _  ___  | |__  _   _  ___ \r\n | '_ \\| | | |/ _ \\ | '_ \\| | | |/ _ \\\r\n | |_) | |_| |  __/ | |_) | |_| |  __/\r\n |_.__/ \\__, |\\___| |_.__/ \\__, |\\___|\r\n        |___/              |___/      ");
        }
    }
}