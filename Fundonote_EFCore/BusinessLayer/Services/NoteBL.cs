using BusinessLayer.Interface;
using DatabaseLayer.NoteModels;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;

        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }

        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                await this.noteRL.AddNote(UserId, notePostModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetNoteResponse>> GetAllNote(int UserId)
        {
            try
            {
                return await this.noteRL.GetAllNote(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateNote(int UserId, int NoteId, UpdateNoteModel updateNoteModel)
        {
            try
            {
                await this.noteRL.UpdateNote(UserId, NoteId, updateNoteModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteNote(int UserId, int NoteId)
        {
            try
            {
                return await this.noteRL.DeleteNote(UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ArchiveNote(int userId, int noteId)
        {
            try
            {
                return await this.noteRL.ArchiveNote(userId, noteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PinNote(int userId, int noteId)
        {
            try
            {
                return await this.noteRL.PinNote(userId, noteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> TrashNote(int userId, int noteId)
        {
            try
            {
                return await this.noteRL.TrashNote(userId, noteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
