using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MetronomeApp
{
    class Metronome : IMetronomeable
    {
        protected int bpm;
        protected int beats;
        protected int currentBeat = 0;
        protected bool isStarted = false;
        protected Timer timer;
        protected readonly WaveOutEvent strongBeep = new();
        protected readonly WaveOutEvent weakBeep = new();
        protected readonly SignalGenerator signalGeneratorStrong = new(44100, 1)
        {
            Type = SignalGeneratorType.Sin,
            Frequency = 440,
            Gain = 0.8
        };

        protected readonly SignalGenerator signalGeneratorWeak = new(44100, 1)
        {
            Type = SignalGeneratorType.Sin,
            Frequency = 440,
            Gain = 0.2
        };
        public Visuals Visuals { get; set; }
        protected Stack<int> pastBPMs = new();

        public Metronome(int beats)
        {
            Beats = beats;
            Visuals = new(beats);
        }

        public Metronome() { }

        public int BPM
        {
            get { return bpm; }
            set
            {
                if(value >= 20 && value <= 400)
                {
                    bpm = value;
                }
                else
                {
                    bpm = 120;
                }
            }
        }

        public int Beats
        {
            get { return beats; }
            set
            {
                if (value >= 1 && value <= 16)
                {
                    beats = value;
                }
                else
                {
                    beats = 4;
                }
            }
        }

        public virtual void StartPause()
        {
            if (isStarted)
            {
                Pause();
            }
            else
            {
                Start();
            }
        }

        public virtual void Pause()
        {
            timer.Dispose();
            isStarted = false;
        }

        public virtual void Start()
        {
            isStarted = true;
            strongBeep.Init(signalGeneratorStrong);
            weakBeep.Init(signalGeneratorWeak);
            timer = new(_ =>
            {
                if (currentBeat % Beats == 0)
                {
                    strongBeep.Play();
                    Visuals.UpdateStrong();
                    strongBeep.Stop();
                }
                else
                {
                    weakBeep.Play();
                    Visuals.UpdateWeak(currentBeat);
                    weakBeep.Stop();
                }
                currentBeat++;
            }, null, 0, (int)((60.0 / BPM) * 1000));
        }

        public virtual void Reset()
        {
            timer.Dispose();
            isStarted = false;
            currentBeat = 0;
            int lastBPM = BPM;
            Console.ForegroundColor = ConsoleColor.White;
            if(pastBPMs.Count == 0)
            {
                Console.SetCursorPosition(0, 11);
                SetBPM();
            }
            else
            {
                Console.SetCursorPosition(0, 11);
                Console.WriteLine("Press B to change the BPM to the one from your last session. Press any other key to set new BPM");
                ConsoleKeyInfo choice = Console.ReadKey();
                if (choice.Key == ConsoleKey.B)
                {
                    BPM = pastBPMs.Peek();
                }
                else
                {
                    Console.SetCursorPosition(0, 12);
                    SetBPM();
                }
            }
            pastBPMs.Push(lastBPM);
            Console.Clear();
            Visuals.DisplayTitle();
            Visuals.DisplayInfo(this);
        }
        
        public virtual void Init()
        {
            Console.Clear();
            Visuals.DisplayTitle();
            Console.WriteLine("Regular it is");
            SetBPM();
            SetBeats();
        }

        protected virtual void SetBPM()
        {
            Console.WriteLine("Enter BPM: (integer between 20 - 400)");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int bpm) && bpm >= 20 && bpm <= 400)
                {
                    BPM = bpm;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid BPM! Enter again: ");
                }
            }
        }

        protected void SetBeats()
        {
            Console.WriteLine("Enter number of beats per measure: (integer between 1 and 16)");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int beats) && beats >= 1 && beats <= 16)
                {
                    Beats = beats;
                    Visuals = new(beats);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input! Enter again: ");
                }
            }
        }
    }
}