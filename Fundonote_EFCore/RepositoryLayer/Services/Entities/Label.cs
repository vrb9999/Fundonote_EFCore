namespace RepositoryLayer.Services.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public class Label
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }

        public string LabelName { get; set; }

        [ForeignKey("User")]
        public virtual int? UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Note")]
        public virtual int? NoteId { get; set; }

        public virtual Note Note { get; set; }
    }
}