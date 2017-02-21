using NLPtest;
using NLPtest.view;
using System.Collections.Generic;

public interface INLPControler
{
//	void Initialize();
	 List<Sentence> Analize(string text);
	  string getClass(string text);

      string getName(string inputText);
	  string GetGender(string text);
	
      string GetGeneralFeeling(string text);

      ContentList checkContext(string text,ConversationContext context);
    ContentList testAnalizer(string inputText, out string log);
    
}