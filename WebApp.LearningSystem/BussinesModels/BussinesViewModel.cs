using DataModels;

namespace WebApp.LearningSystem.BussinesModels;

public class BusinessViewModel
{
    public IHaveAccount<Account> AuthorizationModel { get; set; }
    public ContextModel ContextModel { get; init; }

    public Guid? CurrentThemeId { get; set; } = null;
    

    public BusinessViewModel(AuthorizationModel? authorizationModel = null, ContextModel? contextModel = null)
    {
        AuthorizationModel = authorizationModel;
        ContextModel = contextModel;
    }

    public BusinessViewModel()
    {
        
    }
}