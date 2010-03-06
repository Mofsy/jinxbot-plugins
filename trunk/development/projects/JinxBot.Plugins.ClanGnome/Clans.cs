namespace JinxBot.Plugins.ClanGnome
{
    partial class ClansDataContext
    {
        public static ClansDataContext Create(string dbPath)
        {
            return new ClansDataContext("Data Source=" + dbPath + "; Persist Security Info=False;");
        }

        public static ClansDataContext CreateReadOnly(string dbPath)
        {
            return new ClansDataContext("Data Source=" + dbPath + "; Persist Security Info=False;") { ObjectTrackingEnabled = false };
        }
    }
}
