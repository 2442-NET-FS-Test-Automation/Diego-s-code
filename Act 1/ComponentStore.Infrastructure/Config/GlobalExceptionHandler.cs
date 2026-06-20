using Serilog;

namespace ComponentStore.Infrastructure;

public static class GlobalExceptionHandler
{
    public static void Register()
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }

    private static void CurrentDomain_UnhandledException(
        object sender,
        UnhandledExceptionEventArgs e)
    {
        Log.Fatal(e.ExceptionObject as Exception, "Unhandled application exception");
    }

    private static void TaskScheduler_UnobservedTaskException(
        object? sender, UnobservedTaskExceptionEventArgs e)
    {
        Log.Fatal(e.Exception, "Unobserved task exception"); 
    }
}