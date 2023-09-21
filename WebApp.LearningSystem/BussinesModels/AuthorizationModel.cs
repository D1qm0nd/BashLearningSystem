using DataModels;

namespace WebApp.LearningSystem.BussinesModels;

public class AuthorizationModel : IHaveAccount<Account>
{
    public Account? Account { get; set; }

    string IHaveAccount<Account>.AccountToString => $"{Account.Surname} {Account.Name} {Account.MiddleName}";
}