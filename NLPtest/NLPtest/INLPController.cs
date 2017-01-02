public interface INLPControler
{
	public static void Initialize();
	public static List<Sentence> Analize();
	 public  static string getClass(string text);
	  public static string removeParentheses(string input, char start, char end);
	

        public static String getName(string inputText);
	 public static string GetGender(string text);
	
        public static string GetGeneralFeeling(string text);
        
	
}