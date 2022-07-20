namespace DatabaseLayer.NoteModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class NoteReminderModel
    {
        [Required]
        [DefaultValue("YYYY-MM-DD HH:MM:SS")]
        [RegularExpression(@"[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (2[0-3]|[01][0-9]):[0-5][0-9]:[0-5][0-9]", ErrorMessage = "Please Enter the Valid Data time eg.2021-01-30 01:59:59")]
        public string Remainder { get; set; }
    }
}
