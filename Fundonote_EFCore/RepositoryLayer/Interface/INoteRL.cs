namespace RepositoryLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.NoteModels;

    public interface INoteRL
    {
        Task AddNote(int UserId, NotePostModel notePostModel);

        Task<List<GetNoteResponse>> GetAllNote(int UserId);

        Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel);

        Task<bool> DeleteNote(int userId, int noteId);

        Task<bool> ArchiveNote(int userId, int noteId);

        Task<bool> PinNote(int userId, int noteId);

        Task<bool> TrashNote(int userId, int noteId);

        Task<string> Remainder(int userId, int noteId, DateTime Remainder);
    }
}
