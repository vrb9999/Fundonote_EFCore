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

        Task<List<LabelResponseModel>> GetAllLabel(int UserId);

        Task<List<LabelResponseModel>> GetLabelByNoteId(int UserId, int NoteId);

        Task<bool> UpdateLabel(int UserId, int NoteId, string LabelName);

        Task<bool> DeleteLabel(int UserId, int NoteId, int LabelId);
    }
}
