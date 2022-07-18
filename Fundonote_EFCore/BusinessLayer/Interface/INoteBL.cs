﻿namespace BusinessLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.NoteModels;

    public interface INoteBL
    {
        Task AddNote(int UserId, NotePostModel notePostModel);

        Task<List<GetNoteResponse>> GetAllNote(int UserId);
    }
}
