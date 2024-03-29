using NLog;
using System.Runtime.CompilerServices;

namespace ClothX.Services
{
	// Service for logging errors using NLog
	public class ErrorLogger
	{
		private static ErrorLogger _instance;

		// Singleton instance of the ErrorLogger
		public static ErrorLogger Instance
		{
			get
			{
				if (_instance == null)
					_instance = new ErrorLogger();
				return _instance;
			}
		}

		// Log an error message along with the class name and calling method name
		public void ErrorLoggingFunction(string errorMessage, string className, [CallerMemberName] string callerMethodName = null)
		{
			LogManager.GetCurrentClassLogger().Error($"{errorMessage} occurred in {className}'s {callerMethodName} method.");
		}
	}
}
