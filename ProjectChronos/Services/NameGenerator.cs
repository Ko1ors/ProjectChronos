namespace ProjectChronos.Services
{
    public class NameGenerator
    {
        private static Random random = new Random();

        private static string[] adjectives;
        private static string[] nouns;

        public static void LoadWordLists()
        {
            adjectives = File.ReadAllLines("Assets\\adjectives.txt");
            nouns = File.ReadAllLines("Assets\\nouns.txt");
        }

        public static string GenerateName()
        {
            if (adjectives == null || nouns == null)
            {
                LoadWordLists();
            }

            string adjective = adjectives[random.Next(adjectives.Length)];
            string noun = nouns[random.Next(nouns.Length)];

            return $"{adjective} {noun}";
        }
    }
}
