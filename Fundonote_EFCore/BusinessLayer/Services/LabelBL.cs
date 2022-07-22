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
    }
}
