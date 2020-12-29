namespace Pairs.DesktopClient.Presenter
{
    class NewPlayerCredentials : PlayerCredentials
    {
        public string RepeatedPassword { get; set; }

        public bool Valid => GetAlertMessage() == null;

        public string GetAlertMessage()
        {
            if (Nick == null || Nick.Length < 3)
            {
                return "Nick has to contain at least 3 characters.";
            }
            if (Password == null || Password.Length < 3)
            {
                return "Password has to contain at least 3 characters.";
            }
            if (RepeatedPassword == null || !Password.Equals(RepeatedPassword))
            {
                return "Password and repeated password do not match up.";
            }
            return null;
        }
    }
}
