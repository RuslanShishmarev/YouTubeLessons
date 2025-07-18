using SimpleWebFramework.Main.Models;
using SimpleWebFramework.Main.Services.Interfaces;

namespace SimpleWebFramework.Main.Services;

public class UserService : IUserService
{
    public Guid Id { get; }

    private List<User> _users = [];
    public UserService()
    {
        Id = Guid.NewGuid();
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public IEnumerable<User> GetUsers()
    {
        return _users;
    }

    public void RemoveUser(int userId)
    {
        _users.Remove(_users.Find(x => x.Id == userId));
    }
}
