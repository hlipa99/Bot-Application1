using java.lang;
using NLPtest;
using System.Collections.Generic;
using System;

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
}
        
	
