﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public int Id { get; set; }

        public string FirstName  { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public decimal NetWorth  { get; set; }

        public virtual ICollection<SongPerformer> PerformerSongs { get; set; }
            = new HashSet<SongPerformer>();
    }
}
