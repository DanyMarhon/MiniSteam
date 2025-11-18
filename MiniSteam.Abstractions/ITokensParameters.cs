namespace MiniSteam.Abstractions
{
    public interface ITokensParameters
    {
        string UserName { get; set; }
        string Email { get; set; }
        string PaswordHash { get; set; }
        string Id { get; set; }
        IList<string>? Roles { get; set; }
    }
}
