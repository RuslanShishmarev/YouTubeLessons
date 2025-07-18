using SimpleWebFramework.Main.Services.Interfaces;

namespace SimpleWebFramework.Main.Services;

public class LoggingTest : ILoggingTest
{
    private IUserService _userService;

    public LoggingTest(IUserService userService)
    {
        _userService = userService;
    }

    public void Log()
    {
        Console.WriteLine(_userService.Id);
    }
}
