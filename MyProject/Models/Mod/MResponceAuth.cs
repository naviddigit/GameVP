using DataService.Entity;

namespace ProjectLayer.Models.Mod
{
    public class MResponceAuth
    {
        public object? accessToken { get; set; }
        public Account? user { get; set; }
        public object? message { get; set; }
    }
}