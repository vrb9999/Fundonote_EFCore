namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.Entities;
    using DatabaseLayer.LabelModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Services.Entities;

    public class LabelRL : ILabelRL
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration iconfiguration;

        public LabelRL(FundoContext fundoContext, IConfiguration iconfiguration)
        {
            this.fundoContext = fundoContext;
            this.iconfiguration = iconfiguration;
        }

        public async Task AddLabel(int UserId, int NoteId, string LabelName)
        {
            try
            {
                Label label = new Label();
                label.UserId = UserId;
                label.NoteId = NoteId;
                label.LabelName = LabelName;

                this.fundoContext.Labels.Add(label);
                await this.fundoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LabelResponseModel>> GetAllLabel(int UserId)
        {
            try
            {
                var label = fundoContext.Labels.FirstOrDefault(u => u.UserId == UserId);
                if (label == null)
                {
                    return null;
                }

                var res = await (from user in fundoContext.Users
                                 join notes in fundoContext.Notes on user.UserId equals UserId
                                 join labels in fundoContext.Labels on notes.NoteId equals labels.NoteId
                                 where labels.UserId == UserId
                                 select new LabelResponseModel
                                 {
                                     LabelId = labels.LabelId,
                                     UserId = UserId,
                                     NoteId = notes.NoteId,
                                     Title = notes.Title,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     Email = user.Email,
                                     Description = notes.Description,
                                     LabelName = labels.LabelName,
                                 }).ToListAsync();
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LabelResponseModel>> GetLabelByNoteId(int UserId, int NoteId)
        {
            try
            {
                var label = this.fundoContext.Labels.FirstOrDefault(x => x.UserId == UserId);
                if (label == null)
                {
                    return null;
                }

                var result = await (from user in fundoContext.Users
                                    join notes in fundoContext.Notes on user.UserId equals UserId //where notes.NoteId == NoteId
                                    join labels in fundoContext.Labels on notes.NoteId equals labels.NoteId
                                    where labels.NoteId == NoteId
                                    select new LabelResponseModel
                                    {
                                        LabelId = labels.LabelId,
                                        UserId = UserId,
                                        NoteId = notes.NoteId,
                                        Title = notes.Title,
                                        FirstName = user.FirstName,
                                        LastName = user.LastName,
                                        Email = user.Email,
                                        Description = notes.Description,
                                        LabelName = labels.LabelName,
                                    }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateLabel(int UserId, int NoteId, string LabelName)
        {
            try
            {
                var label = fundoContext.Labels.FirstOrDefault(u => u.UserId == UserId && u.NoteId == NoteId);
                if (label == null)
                {
                    return false;
                }

                label.LabelName = LabelName;
                await fundoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteLabel(int UserId, int NoteId, int LabelId)
        {
            try
            {
                var result = this.fundoContext.Labels.Where(x => x.NoteId == NoteId && x.UserId == UserId && x.LabelId == LabelId).FirstOrDefault();
                if (result == null)
                {
                    return false;
                }

                this.fundoContext.Labels.Remove(result);
                await this.fundoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
