using SoLoud;

namespace NekoRay; 

public static class Audio {

    public static SoLoud.SoLoud SoLoud;
    public static SoLoudBackend Backend = SoLoudBackend.Auto;
    public static void Init() {
        if (SoLoud is not null) return;
        SoLoud = new SoLoud.SoLoud();
        SoLoud.Init(SoloudFlags.ClipRoundoff, Backend);
    }

    public static void Close() {
        SoLoud.DeInit();
    }
}