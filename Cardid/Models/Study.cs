using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cardid.Models
{
    public class Study
    {
        public string DeckID { get; set; }
        public string UserID { get; set; }
        public int TotalScore { get; set; }
        public int PossibleScore { get; set; }

    }
}