using BashDataBaseModels;
using BashLearningDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Site.Controllers;

public class DataController : ControllerBase
{
    private readonly BashLearningContext _context;

    public DataController(BashLearningContext context)
    {
        _context = context;
    }

    [HttpGet("get-all-themes")]
    public List<ThemeInfo> GetThemesAsync() =>
        ThemeInfo.InfoList(GetAllThemes().ToList(), (theme) => new ThemeInfo(theme)).ToList();

    [HttpPost("get-read-themes")]
    public Task<List<ThemeInfo>> GetReadThemesAsync([FromBody] Guid userId) =>
        Task.FromResult(ThemeInfo.InfoList(GetReadThemes(userId), (theme) => new ThemeInfo(theme)).ToList());

    [HttpPost("get-unread-themes")]
    public Task<List<ThemeInfo>> GetUnreadThemesAsync([FromBody] Guid userId) =>
        Task.FromResult(ThemeInfo.InfoList(GetUnreadThemes(userId), theme => new ThemeInfo(theme)).ToList());


    #region DataClasses

    [Serializable]
    public class ResponseData
    {
        public bool Response { get; set; }

        public ResponseData(bool responce)
        {
            Response = responce;
        }
    }

    [Serializable]
    public class RequestData
    {
        public Guid themeId { get; set; }
        public Guid userId { get; set; }
    }

    #endregion

    [HttpPost("theme-is-read")]
    public Task<ResponseData> ThemeIsReadByUser([FromBody] RequestData data = null)
    {
        var res = new ResponseData(false);
        if (data?.userId != null)
        {
            res.Response = ThemeIsRead(data.themeId, data.userId);
        }

        return Task.FromResult(res);
    }

    private IEnumerable<Theme> GetAllThemes() => _context.Themes
        .Include(theme => theme.Commands)
        .ThenInclude(command => command.Attributes).Where(e => e.IsActual);

    private IEnumerable<Theme> GetReadThemes(Guid userId) =>
        _context.Themes
            .Include(theme => theme.Commands)
            .ThenInclude(command => command.Attributes)
            .Include(theme => theme.Reads)
            .Where(theme => theme.Reads.Any(read => read.UserId == userId));


    private IEnumerable<Theme> GetUnreadThemes(Guid userId) => _context.Themes
        .Include(theme => theme.Commands)
        .ThenInclude(command => command.Attributes)
        .Include(theme => theme.Reads)
        .Where(theme => !theme.Reads.Any(read => read.UserId == userId));

    private bool ThemeIsRead(Guid themeId, Guid userId)
    {
        var res = _context.Reads.Any(read => read.UserId == userId && read.ThemeId == themeId);
        return res;
    }
}