using NLPtest;
using System.Collections.Generic;
using System;
using NLPtest.view;

public class NLPControler : INLPControler
{
	
	static INLPControler instance;
    static object syncLock = new object();
    MorfAnalizer ma = null;

    private NLPControler()
    {
        this.ma = new MorfAnalizer();
    }

    public static INLPControler getInstence()
    {
        lock (syncLock)
        {
            if (instance != null)
            {
                return instance;
            }else
            {
                //   var nlp =  new NLPControler();
                var nlp = new NLPControlerTestStub();
                nlp.Initialize();
                return nlp;
            }
        }
     
    }



	public void Initialize(){
		ma = new MorfAnalizer();
	}

    public List<Sentence> Analize(string text)
    {
        return ma.meniAnalize(text);
    }

    public  string getClass(string text){
		return ma.getClass(text);
	}

	

        public string getName(string inputText){
		return ma.getName(inputText);

	}

	 public string GetGender(string text){
		return ma.GetGender(text);
	}
	
        public string GetGeneralFeeling(string text){
		return ma.GetGeneralFeeling(text);
	}

    public ContentTurn checkContext(string text, ConversationContext context)
    {
        throw new NotImplementedException();
    }
}
        
	
