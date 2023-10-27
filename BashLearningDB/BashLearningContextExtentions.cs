using BashDataBaseModels;
using BashLearningModelsValidate;

namespace BashLearningDB;

public static class BashLearningContextExtension
{
    public static bool IsAdmin(this BashLearningContext context, User? user)
    {
        if (user == null)
            return false; 
        return context.Admins
            .Any(admin =>
                admin.UserId == user.UserId
                && admin.IsActual == true) != null;
    }

    public static bool Register(this BashLearningContext context, Validator<User> validator, User user)
    {
        if (!validator.Validate(user).Result)
            return false;

        context.Users.Add(user);
        context.SaveChanges();
        return true;
    }

    
    
}