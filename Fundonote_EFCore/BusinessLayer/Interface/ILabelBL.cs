namespace BusinessLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DatabaseLayer.LabelModels;

    public interface ILabelBL
    {
        Task AddLabel(int UserId, int NoteId, string LabelName);

        Task<bool> UpdateLabel(int UserId, int NoteId, string LabelName);
    }
}
