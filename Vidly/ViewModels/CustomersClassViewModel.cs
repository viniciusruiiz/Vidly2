﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vidly.Models;

namespace vidly.ViewModels
{
    public class CustomersClassViewModel
    {
        public virtual IList<Customer> Customers { get; set; }
    }
}