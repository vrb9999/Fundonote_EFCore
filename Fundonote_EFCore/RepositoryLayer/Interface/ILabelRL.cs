namespace RepositoryLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.LabelModels;

    public interface ILabelRL
    {
        Task AddLabel(int UserId, int NoteId, string LabelName);
    }
}
