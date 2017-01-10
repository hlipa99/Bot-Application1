using NLPtest;
using NLPtest.view;
using System.Collections.Generic;

public interface INLPControler
{
	void Initialize();
	 List<Sentence> Analize(string text);
	  string getClass(string text);

      string getName(string inputText);
	  string GetGender(string text);
	
      string GetGeneralFeeling(string text);

      ContentTurn checkContext(string text,ConversationContext context);


}