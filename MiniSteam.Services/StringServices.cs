namespace MiniSteam.Services
{
    public interface IStringServices
    {
        string GetString(string stn);
    }
    public class StringServices : IStringServices
    {
        public string GetString(string stn)
        {
            return string.Join(" ", stn, "It works");
        }
    }
}
