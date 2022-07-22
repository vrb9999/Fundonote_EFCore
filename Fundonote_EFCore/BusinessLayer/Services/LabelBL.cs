namespace BusinessLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.LabelModels;
    using RepositoryLayer.Interface;

    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL labelRL;

        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public async Task AddLabel(int UserId, int NoteId, string LabelName)
        {
            try
            {
                await this.labelRL.AddLabel(UserId, NoteId, LabelName);
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
                return await this.labelRL.GetAllLabel(UserId);
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
                return await this.labelRL.GetLabelByNoteId(UserId, NoteId);
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
                return await this.labelRL.UpdateLabel(UserId, NoteId, LabelName);
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
                return await this.labelRL.DeleteLabel(UserId, NoteId, LabelId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
