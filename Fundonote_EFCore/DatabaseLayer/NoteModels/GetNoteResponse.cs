﻿namespace DatabaseLayer.NoteModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GetNoteResponse
    {
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Bgcolor { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
