using java.lang;
using NLPtest;
using System.Collections.Generic;
using System;
using NLPtest.view;

public class NLPControlerTestStub : INLPControler
{
	
    public string getClass(string text){
		return "�";
	}



    public string getName(string inputText){
		return "����";
	}

	 public string GetGender(string text){
		return "masculaine";
	}
	
        public string GetGeneralFeeling(string text){
		return "good";
	}

    public List<Sentence> Analize(string text)
    {
        return new List<Sentence>();
    }

    public void Initialize()
    {

    }

    public bool checkContext(string text, ConversationContext context)
    {
        return true;
    }

    ContentTurn INLPControler.checkContext(string text, ConversationContext context)
    {
        return new ContentTurn();
    }
}
        
	
