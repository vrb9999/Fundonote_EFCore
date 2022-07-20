namespace BusinessLayer.Interface
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

        Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel);

        Task<bool> DeleteNote(int UserId, int NoteId);

        Task<bool> ArchiveNote(int userId, int noteId);

        Task<bool> PinNote(int userId, int noteId);
    }
}
