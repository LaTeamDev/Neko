using System.Runtime.CompilerServices;

namespace NekoRay;

public static class ThreadSafety
{
	public static int CurrentThreadId => Environment.CurrentManagedThreadId;
	
	public static string? CurrentThreadName => Thread.CurrentThread.Name;
	
	[field: ThreadStatic]
	public static bool IsCurrentThreadMain { get; private set; }

	internal static void MarkThisThreadMain()
	{
		IsCurrentThreadMain = true;
	}
	
	public static void ThrowIfNotMainThread([CallerMemberName] string memberName = "")
	{
		if (IsCurrentThreadMain) return;
		throw new Exception(memberName + " was not called in the main thread");
	}
	
	public static void ThrowIfMainThread([CallerMemberName] string memberName = "")
	{
		if (!IsCurrentThreadMain) return;
		throw new Exception(memberName + " was called in the main thread");
	}
}

