using System.ComponentModel.DataAnnotations;
using BashDataBaseModels;
using BashLearningDB;
using BashLearningModelsValidate;
using EncryptModule;
using Microsoft.EntityFrameworkCore;

namespace Site;

public class AuthorizationService
{

    private readonly BashLearningContext _context;
    private readonly Cryptograph _cryptoGraph;
    
    public AuthorizationService(BashLearningContext context, Cryptograph cryptoGraph) 
    { 
        _context = context;
        _cryptoGraph = cryptoGraph;
    }

    public Task<User?> Login(string login, string password)
    {
        var _login = _cryptoGraph.Coding(login);
        var _password = _cryptoGraph.Coding(password);
        return _context.Users.FirstOrDefaultAsync(user => user.Login == _login && user.Password == _password && user.IsActual);
    }

    public Task<bool> Exists(string login)
    {
        var _login = _cryptoGraph.Coding(login);
        return _context.Users.AnyAsync(user => user.Login == _login && user.IsActual);
    }
    
    public  Task<bool> Exists(User user)
    {
        return _context.Users.AnyAsync(_user => _user.Login == user.Login && user.IsActual);
    }
    
    public Task<bool> IsAdmin(User user)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var res = _context.Admins
                .FirstOrDefault(admin =>
                    admin.UserId == user.UserId
                    && admin.IsActual) != null;
            return res;
        });
    }

    public Task<User?> Register(Validator<User> validator, User user)
    {
        return Task<User?>.Factory.StartNew(() =>
        {
            if (!validator.Validate(user).Result || Exists(user).Result)
            {
                return null;
            }
            user.IsActual = true;
            _context.Users.Add( Coding(user));
            _context.SaveChanges();
            return user;
        });
    }

    private User Coding(User user)
    {
        user.Login = _cryptoGraph.Coding(user.Login);
        user.Password = _cryptoGraph.Coding(user.Password);
        return user;
    }
}