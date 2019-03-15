using System.Diagnostics;

namespace Common
{
    public class StackTraceUtils
    {
        public static string GetCalledMethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            var methodInfo = stackFrames[1].GetMethod();
            return methodInfo?.Name;
        }
    }
}
