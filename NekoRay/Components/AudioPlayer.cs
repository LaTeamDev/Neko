namespace NekoRay; 

public class AudioPlayer : Behaviour {
    public IPlayable? AudioClip;

    public void Play() => AudioClip?.Play();
    public void Stop() => AudioClip?.Stop();
    public void Pause() => AudioClip?.Pause();
    public void Resume() => AudioClip?.Resume();

    public void Seek(float seconds) => AudioClip?.Seek(seconds);

    public float Volume {
        get => AudioClip?.Volume??1f;
        set => AudioClip!.Volume = value;
    }
    
    public float Pitch {
        get => AudioClip?.Pitch??1f;
        set => AudioClip!.Pitch = value;
    }
    
    public float Pan {
        get => AudioClip?.Pan??0.5f;
        set => AudioClip!.Pan = value;
    }
    
    public float? Length => AudioClip?.Length;
    public float? TimePlayed => AudioClip?.TimePlayed;
    public bool IsReady => AudioClip?.IsReady??false;
    public bool IsPlaying => AudioClip?.IsPlaying??false;
    
    public bool Loop = false;
    
    public override void Dispose() {
        base.Dispose();
        AudioClip?.Dispose();
    }

    public void Update() {
        AudioClip?.Update();
    }
}