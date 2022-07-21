﻿namespace RepositoryLayer.Services.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Bgcolor { get; set; }

        public bool IsPin { get; set; }

        public bool IsArchive { get; set; }

        public bool IsRemainder { get; set; }

        public bool IsTrash { get; set; }

        [Required]
        [ForeignKey("UserId")]

        public virtual int? UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime RegisteredDate { get; set; }

        public DateTime Remainder { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
