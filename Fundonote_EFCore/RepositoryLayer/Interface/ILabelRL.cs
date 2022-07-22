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

        Task<List<LabelResponseModel>> GetAllLabel(int UserId);

        Task<List<LabelResponseModel>> GetLabelByNoteId(int UserId, int NoteId);

        Task<bool> UpdateLabel(int UserId, int NoteId, string LabelName);
    }
}
