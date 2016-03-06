using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Control
    {
        public Control ()
        {
            Stop = false;
            Start = false;
            Reset = false;
        }

        public bool Stop { get; set; }

        public bool Start { get; set; }

        public bool Reset { get; set; }
    }
}