namespace Xasa.Onboarding
{
    public class NewHire
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string NeoServiceBusConnection { get; set; }

        public string NeoApiKey { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(Email);
        }
    }
}
