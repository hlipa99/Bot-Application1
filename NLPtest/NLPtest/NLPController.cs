public interface NLPControler
{
	
	MorfAnalizer ma;
	public static void Initialize(s){
		ma = new MorfAnalizer();
	}



	public static List<Sentence> Analize(string text){
		return ma.maniAnalize(text);

	}



	 public  static string getClass(string text){
		return ma.getClass(text);
	}

	

        public static String getName(string inputText){
		return ma.getName(text);

	}

	 public static string GetGender(string text){
		return ma.GetGender(text);
	}
	
        public static string GetGeneralFeeling(string text){
		return ma.GetGeneralFeeling(text);
	}

}
        
	
}