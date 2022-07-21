namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.Entities;
    using DatabaseLayer.NoteModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Services.Entities;

    public class NoteRL : INoteRL
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration iconfiguration;

        public NoteRL(FundoContext fundoContext, IConfiguration iconfiguration)
        {
            this.fundoContext = fundoContext;
            this.iconfiguration = iconfiguration;
        }

        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                Note note = new Note();
                note.UserId = UserId;
                note.Title = notePostModel.Title;
                note.Description = notePostModel.Description;
                note.Bgcolor = notePostModel.Bgcolor;
                note.RegisteredDate = DateTime.Now;
                note.ModifiedDate = DateTime.Now;
                this.fundoContext.Notes.Add(note);
                await this.fundoContext.SaveChangesAsync();
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
                return await this.fundoContext.Notes
                .Where(n => n.UserId == UserId && n.IsTrash == false)
                .Join(fundoContext.Users,
                n => n.UserId,
                u => u.UserId,
                (n, u) => new GetNoteResponse
                {
                    NoteId = n.NoteId,
                    UserId = (int)n.UserId,
                    Title = n.Title,
                    Description = n.Description,
                    Bgcolor = n.Bgcolor,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    CreatedDate = u.CreatedDate,
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateNote(int userId, int noteId, UpdateNoteModel updateNoteModel)
        {
            try
            {
                var updateNote = fundoContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
                if (updateNote == null)
                {
                    throw new Exception("Note Does Not Exists!!");
                }

                updateNote.Title = updateNoteModel.Title;
                updateNote.Description = updateNoteModel.Description;
                updateNote.Bgcolor = updateNoteModel.Bgcolor;
                updateNote.IsPin = updateNoteModel.IsPin;
                updateNote.IsArchive = updateNoteModel.IsArchive;
                updateNote.IsRemainder = updateNoteModel.IsRemainder;
                updateNote.IsTrash = updateNoteModel.IsTrash;
                updateNote.ModifiedDate = DateTime.Now;
                this.fundoContext.Notes.UpdateRange(updateNote);
                await this.fundoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteNote(int userId, int noteId)
        {
            var flag = false;
            try
            {
                var result = this.fundoContext.Notes.Where(x => x.NoteId == noteId && x.UserId == userId).FirstOrDefault();
                if (result != null)
                {
                    flag = true;
                    result.IsTrash = true;
                    this.fundoContext.Notes.Update(result);
                    await this.fundoContext.SaveChangesAsync();
                    return await Task.FromResult(flag);
                }

                return await Task.FromResult(flag);
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
                var flag = true;
                var note = this.fundoContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
                if (note != null && note.IsTrash == false)
                {
                    if (note.IsArchive == false)
                    {
                        note.IsArchive = true;
                    }
                    else
                    {
                        note.IsArchive = false;
                        flag = false;
                    }

                    this.fundoContext.Notes.Update(note);
                    await this.fundoContext.SaveChangesAsync();
                    return await Task.FromResult(flag);
                }

                return await Task.FromResult(!flag);
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
                var flag = true;
                var note = this.fundoContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
                if (note != null && note.IsTrash == false)
                {
                    if (note.IsPin == false)
                    {
                        note.IsPin = true;
                    }
                    else
                    {
                        note.IsPin = false;
                        flag = false;
                    }

                    this.fundoContext.Notes.Update(note);
                    await this.fundoContext.SaveChangesAsync();
                    return await Task.FromResult(flag);
                }

                return await Task.FromResult(!flag);
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
                var flag = true;
                var note = this.fundoContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
                if (note != null)
                {
                    if (note.IsTrash == false)
                    {
                        note.IsTrash = true;
                    }
                    else
                    {
                        note.IsTrash = false;
                        flag = false;
                    }

                    this.fundoContext.Notes.Update(note);
                    await this.fundoContext.SaveChangesAsync();
                    return await Task.FromResult(flag);
                }

                return await Task.FromResult(!flag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> Remainder(int userId, int noteId, DateTime Remainder)
        {
            try
            {
                var note = this.fundoContext.Notes.Where(x => x.NoteId == noteId && x.UserId == userId).FirstOrDefault();
                if (note != null && note.IsRemainder == false)
                {
                    note.Remainder = Remainder;
                    note.IsRemainder = true;
                    await this.fundoContext.SaveChangesAsync();
                    return "Reminder Set Successfull for date:" + Remainder.Date + " And Time : " + Remainder.TimeOfDay;
                }
                else
                {
                    note.IsRemainder = false;
                    await this.fundoContext.SaveChangesAsync();
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
