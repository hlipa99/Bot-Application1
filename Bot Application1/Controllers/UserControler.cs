using Bot_Application1.dataBase;
using Model.dataBase;
using NLPtest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Controllers
{
    public class UserControler
    {

        DataBaseControler dc = new DataBaseControler();
        
        public Users getUser(string id)
        {
            var user = dc.getUser(id);
            return user;
        }


        public void addUser(Users user)
        {
            dc.addNewUser(user);
        }

    }
}