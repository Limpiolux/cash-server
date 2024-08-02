using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cash_server.Models
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}