namespace Xasa.Onboarding
{
    public class NewHire
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(Email);
        }
    }
}
