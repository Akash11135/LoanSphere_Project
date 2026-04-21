namespace LoanProfile.Helpers
{
    public static class CibilScoreGenerator
    {
        public static int GenerateInitialScore()
        {
            Random random = new Random();
            return random.Next(700, 801);
        }
    }
}