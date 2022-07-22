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
    }
}
