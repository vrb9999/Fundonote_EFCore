﻿namespace RepositoryLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.NoteModels;

    public interface INoteRL
    {
        Task AddNote(int UserId, NotePostModel notePostModel);
    }
}
