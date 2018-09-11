﻿using TicketingSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketingSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUowData Data;

        public BaseController(IUowData data)
        {
            this.Data = data;
        }

        public BaseController()
            : this(new UowData())
        {
        }
    }
}