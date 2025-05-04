using System.Diagnostics;
using NekoLib.Extra;

namespace Neko; 

public static class Time {
    private static Stopwatch _sw = new();
    
    public static double Step() { 
        if (!_sw.IsRunning) 
            _sw.Start();
        Delta = _sw.Elapsed.TotalSeconds;
        CurrentTime += Delta;
        _fixedTime += DeltaF;
        _sw.Restart();
        return Delta;
    }

    public static double CurrentTime { get; private set; }
    private static double _delta = 0f;

    public static double Delta {
        get => IsInFixed ? FixedDeltaF : _delta;
        private set => _delta = value;

    }

    public static float DeltaF => (float)Delta;
    public static float Fps => 1 / DeltaF;

    [ConVariable("host_timescale")] 
    [ConTags("cheat")]
    public static float Timescale { get; set; } = 1f;
    
    [ConVariable("r_drawfps")] 
    public static bool DrawFps { get; set; }

    [ConVariable("host_fixedtimestep")] 
    public static float FixedFps { get; set; } = 60f;

    internal static int TargetDelta = 0;
    
    [ConVariable("r_targetfps")]
    public static int TargetFps {
        get {
            if (TargetDelta == 0) return 0; 
            return 1/TargetDelta;
        }
        set {
            if (value <= 0f) TargetDelta = 0;
            else TargetDelta = 1 / value;
        }
    }
    
    public static double FixedDelta => 1d / FixedFps;
    public static float FixedDeltaF => (float) FixedDelta;

    internal static float _fixedTime = 0f;
    public static bool IsInFixed = false;
}