using SimpleWebFramework.Main.Models;
using SimpleWebFramework.Main.Services.Interfaces;
using SimpleWebFramework.Main.WebFramework.Models.Attributes;

namespace SimpleWebFramework.Main.Controllers;

[ApiController("User")]
public class CheckLifeController
{
    private IUserService _userService;
    private ILoggingTest _loggingTest;

    public CheckLifeController(
        IUserService userService,
        ILoggingTest loggingTest)
    {
        _userService = userService;
        _loggingTest = loggingTest;
    }

    [HttpPost]
    public IEnumerable<User> CreateUser([FromBody] User user)
    {
        Console.WriteLine(_userService.Id);
        _userService.AddUser(user);
        _loggingTest.Log();
        return _userService.GetUsers();
    }
}
