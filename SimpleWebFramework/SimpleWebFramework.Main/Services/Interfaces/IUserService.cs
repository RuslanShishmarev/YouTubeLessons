using SimpleWebFramework.Main.Models;

namespace SimpleWebFramework.Main.Services.Interfaces;

public interface IUserService
{
    Guid Id { get; }

    void AddUser(User user);

    void RemoveUser(int userId);

    IEnumerable<User> GetUsers();
}
