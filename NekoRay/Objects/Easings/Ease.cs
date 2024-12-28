using System.Data;
using System.Reflection;
using System.Numerics;

namespace NekoRay.Easings;

public class Ease<T> : Ease where T : ISubtractionOperators<T, T, T>, IMultiplyOperators<T, float, T>, IAdditionOperators<T, T, T> {
    public float Factor => TimeNow / TimeMax;
    public IEasing Easing = new EaseLinear();
    public Getter GetterFunc;
    public Setter SetterFunc;
    public T Start;
    public T End;
    
    internal Ease(Getter from, Setter setter, float time, T to) {
        TimeMax = time;
        GetterFunc = from;
        SetterFunc = setter;
        End = to;
        Start = from();
    }

    public override void Update(float dt) {
        TimeNow += dt;
        SetterFunc(Start + (End-Start)*Easing.Eval(Factor));
    }

    public Ease<T> SetEasing(IEasing easing) {
        Easing = easing;
        return this;
    }

    public delegate T Getter();
    public delegate void Setter(T value);
}

public abstract class Ease {
    public float TimeMax { get; set; }
    public float TimeNow { get; set; }
    
    public Action? AfterFunc;
    
    private static List<Ease> _list = new();
    public static Ease<T> To<T>(Ease<T>.Getter from, Ease<T>.Setter setter, float time, T to)
        where T : ISubtractionOperators<T, T, T>, IMultiplyOperators<T, float, T>, IAdditionOperators<T, T, T>  {
        var a = new Ease<T>(from, setter, time, to);
        _list.Add(a);
        return a;
    }

    public abstract void Update(float dt);

    public void After(Action a) {
        AfterFunc = a;
    }

    public static void UpdateAll(float dt) {
        var thisFrame = new Ease[_list.Count];
        _list.CopyTo(thisFrame);
        foreach (var ease in thisFrame) {
            if (ease.TimeMax < ease.TimeNow) {
                _list.Remove(ease);
                ease.AfterFunc?.Invoke();
                continue;
            }
            ease.Update(dt);
        }
    }
}