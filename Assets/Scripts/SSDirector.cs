public class SSDirector : System.Object
{
    // singlton instance
    private static SSDirector _instance;

    public MainController currentController { get; set; }


    // get instance anytime anywhare!
    public static SSDirector getInstance()
    {
        if (_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }

}
