﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class MainClass
    {
        static void Main(string[] args)
        {
            DataModelContainer ctx = new DataModelContainer();

            User user = new User();

            ctx.SaveChanges();
        }
    }
}