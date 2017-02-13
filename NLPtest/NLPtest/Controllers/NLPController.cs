using NLPtest;
using System.Collections.Generic;
using System;
using NLPtest.view;
using NLPtest.WorldObj;

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
                   var nlp =  new NLPControler();
           //     var nlp = new NLPControlerTestStub();
             //   nlp.Initialize();
                return nlp;
            }
        }
     
    }

    public ContentTurn testAnalizer(string inputText,out string log)
    {
        SemanticAnalizer sa = new SemanticAnalizer();
        //    var a = MorfAnalizer.createSentence(inputText);
       // var context = new TextContext();
        var sen = ma.meniAnalize(inputText);

      //  sa.findGufContext(sen);


        ContentTurn input = new ContentTurn();
        List<WorldObject> sentence = new List<WorldObject>();
        List<WorldObject> last = new List<WorldObject>();
        string logTemp;
        log = "";
        List<ITemplate> context = new List<ITemplate>();
        var sentences = sa.findGufContext(sen, context);

        foreach (var s in sentences)
        {
            sentence = sa.findTemplate(s.ToArray(),out logTemp);
            log += logTemp;
            last = sentence;
            input.Add(sentence);
        }
        
        return input;

    }

 //   public void Initialize(){
	//	ma = new MorfAnalizer();
	//}

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
        
	
