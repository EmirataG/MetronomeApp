using NAudio.Wave.SampleProviders;
using NAudio.Wave;

namespace MetronomeApp
{
    class IrregularMetronome : Metronome, IMetronomeable
    {
        public int[] SecondaryStrong { get; set; }
        public bool IgnoreWeak { get; set; } = false;
        private int currentIndex = 0;
        private readonly WaveOutEvent SecondaryStrongBeep = new();
        private readonly SignalGenerator signalGeneratorSecondaryStrong = new(44100, 1)
        {
            Type = SignalGeneratorType.Sin,
            Frequency = 440,
            Gain = 0.6
        };

        public IrregularMetronome(int beats, int[] secondary) : base(beats)
        {
            SecondaryStrong = secondary;
        }

        public IrregularMetronome() { }

        public override void StartPause()
        {
            if (isStarted)
            {
                base.Pause();
            }
            else
            {
                Start();
            }
        }

        public override void Start()
        {
            isStarted = true;
            int interval = (int)((60.0 / BPM) * 1000);
            strongBeep.Init(signalGeneratorStrong);
            SecondaryStrongBeep.Init(signalGeneratorSecondaryStrong);

            if (IgnoreWeak)
            {
                timer = new Timer(IgnoreWeakTick, null, 0, interval);
            }
            else
            {
                weakBeep.Init(signalGeneratorWeak);
                timer = new Timer(Tick, null, 0, interval);
            }
        }
        public override void Reset()
        {
            base.Reset();
        }

        public void Tick(object state)
        {
            if (currentBeat % Beats == 0)
            {
                strongBeep.Play();
                Visuals.UpdateStrong();
                strongBeep.Stop();
            }
            else if (currentBeat % Beats == SecondaryStrong[currentIndex] - 1)
            {
                SecondaryStrongBeep.Play();
                if (currentIndex < SecondaryStrong.Length - 1)
                {
                    currentIndex++;
                }
                else
                {
                    currentIndex = 0;
                }
                Visuals.UpdateSecondaryStrong(currentBeat);
                SecondaryStrongBeep.Stop();
            }
            else
            {
                weakBeep.Play();
                Visuals.UpdateWeak(currentBeat);
                weakBeep.Stop();
            }
            currentBeat++;
        }

        public void IgnoreWeakTick(object state)
        {
            if (currentBeat % Beats == 0)
            {
                strongBeep.Play();
                Visuals.UpdateStrong();
                strongBeep.Stop();
            }
            else if (currentBeat % Beats == SecondaryStrong[currentIndex] - 1)
            {
                SecondaryStrongBeep.Play();
                if (currentIndex < SecondaryStrong.Length - 1)
                {
                    currentIndex++;
                }
                else
                {
                    currentIndex = 0;
                }
                Visuals.UpdateSecondaryStrong(currentBeat);
                SecondaryStrongBeep.Stop();
            }
            currentBeat++;
        }

        public void ChooseTimeSignature()
        {
            Dictionary<int, Tuple<int, int[]>> timeSignatures = new()
            {
                {1, new(5, new int[]{3}) },
                {2, new(5, new int[]{4}) },
                {3, new(7, new int[]{3, 5}) },
                {4, new(7, new int[]{4, 6}) },
                {5, new(8, new int[]{4, 6})},
                {6, new(9, new int[]{3, 5, 7}) },
                {7, new(10, new int[]{4, 6, 8}) },
                {8, new(11, new int[]{3, 5, 8, 10})},
                {9, new(12, new int[]{4, 6, 8, 10}) },
                {10, new(13, new int[]{3, 5, 7, 9, 11}) },
            };

            Console.WriteLine("1: 5/8a\n2: 5/8b\n3: 7/8a\n4: 7/8b\n5: 8/8\n6: 9/8\n7: 10/8\n8: 11/16\n9: 12/16\n10: 13/16");
            Console.WriteLine("Your choice: ");
            string choice = Console.ReadLine();
            while (true)
            {
                if (int.TryParse(choice, out int intChoice) && timeSignatures.ContainsKey(intChoice))
                {
                    Beats = timeSignatures[intChoice].Item1;
                    SecondaryStrong = timeSignatures[intChoice].Item2;
                    Visuals = new(Beats);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice");
                    choice = Console.ReadLine();
                }
            }
        }
        public override void Init()
        {
            Console.Clear();
            Visuals.DisplayTitle();
            Console.WriteLine("Irregular it is!\nWould you like to choose one of the time signatures in the list?\nY for yes; N to create a custom pattern");
            ConsoleKeyInfo choice = Console.ReadKey();
            while (true)
            {
                if (choice.Key == ConsoleKey.Y)
                {
                    Console.Write("\b");
                    ChooseTimeSignature();
                    base.SetBPM();
                    break;
                }
                else if (choice.Key == ConsoleKey.N)
                {
                    Console.Write("\b");
                    base.Init();
                    Console.WriteLine("Enter the number of secondary strong beats");
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out int num) && num > 0 && num < Beats)
                        {
                            int[] secondaryStrong = new int[num];
                            for (int i = 0; i < num; i++)
                            {
                                while (true)
                                {
                                    Console.WriteLine($"Enter secondary strong beat number {i + 1}");
                                    if (int.TryParse(Console.ReadLine(), out int beat) && beat <= Beats && !secondaryStrong.Contains(beat))
                                    {
                                        secondaryStrong[i] = beat;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("This beat can't exist in the specified time signature or has already been included");
                                    }
                                }
                            }
                            SecondaryStrong = secondaryStrong;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid number of secondary strong beats! Enter again:");
                        }
                    }
                    break;
                }
                else
                {
                    Console.WriteLine(" is not a valid choice! Enter again: ");
                    choice = Console.ReadKey();
                }
            }
            Console.WriteLine("Would you like to ignore weak beats? (Y for yes and N for no)");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Y)
                {
                    IgnoreWeak = true;
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.N)
                {
                    IgnoreWeak = false;
                    break;
                }
                else
                {
                    Console.WriteLine(" is not a valid choice");
                }
            }
        }
    }
}