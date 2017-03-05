using Bot_Application1.dataBase;
using Model.dataBase;
using Model.Models;
using NLPtest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Controllers
{
    public class UserControler
    {

        DataBaseController dc = DataBaseController.getInstance();


        public void addUser(IUser user)
        {
            dc.addNewUser(user);
        }

    }
}