namespace MetronomeApp
{
    public interface IMetronomeable
    {
        int BPM {get; set;}
        int Beats { get; set; }

        void Start();
        void Pause();
        void Reset();
    }
}