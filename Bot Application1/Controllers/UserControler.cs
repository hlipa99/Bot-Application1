
using Model.dataBase;
using Model.Models;
using NLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Controllers
{
    public class UserControler
    {

        DataBaseController dc = new DataBaseController();


        public void addUser(IUser user)
        {
            dc.addNewUser(user);
        }

    }
}