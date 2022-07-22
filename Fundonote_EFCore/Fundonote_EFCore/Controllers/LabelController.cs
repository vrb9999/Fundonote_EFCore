namespace Fundonote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.Entities;
    using DatabaseLayer.LabelModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using NLogger.Interface;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LabelController : ControllerBase
    {
        private readonly ILoggerManager logger;
        private readonly FundoContext fundoContext;
        private readonly ILabelBL labelBL;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        public LabelController(FundoContext fundoContext, ILabelBL labelBL, ILoggerManager logger, IDistributedCache distributedCache, IMemoryCache memoryCache)
        {
            this.fundoContext = fundoContext;
            this.labelBL = labelBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
        }

        [HttpPost("AddLabel/{NoteId}/{LabelName}")]
        public async Task<IActionResult> AddLabel(int NoteId, string LabelName)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var note = this.fundoContext.Notes.FirstOrDefault(x => x.NoteId == NoteId);

                if (note == null || note.IsTrash == true)
                {
                    this.logger.LogInfo($"Entered invalid NoteId {NoteId}");
                    return this.BadRequest(new { success = false, Message = "Note not found, Enter valid NoteId" });
                }

                if (LabelName == string.Empty || LabelName == "string")
                {
                    this.logger.LogInfo($"Entered the default values {NoteId}");
                    return this.BadRequest(new { success = false, Message = "Enter Valid Data" });

                }

                var label = this.fundoContext.Labels.FirstOrDefault(x => x.LabelName == LabelName);
                if (label == null)
                {
                    await this.labelBL.AddLabel(UserId, NoteId, LabelName);
                    this.logger.LogInfo($"Label Cread Successfully with noted id = {NoteId}");
                    return this.Ok(new { success = true, Message = "Label Created Successfully..." });
                }

                return this.BadRequest(new { success = false, Message = "Label with the name already Exists !!" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpGet("GetAllLabel")]
        public async Task<IActionResult> GetAllLabel()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var LabelData = await this.labelBL.GetAllLabel(UserId);
                if (LabelData.Count == 0)
                {
                    return this.BadRequest(new { success = false, Message = "You don't have any Notes!!" });
                }

                this.logger.LogInfo($"All Labels Retrieved Successfully UserId = {userId}");
                return this.Ok(new { success = true, Message = "Labels Data Retrieved successfully...", data = LabelData });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpGet("GetAllLabelByNoteId/{NoteId}")]
        public async Task<IActionResult> GetAllLabelByNoteId(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var NoteData = await this.labelBL.GetLabelByNoteId(UserId, NoteId);
                if (NoteData == null)
                {
                    this.logger.LogError($"No Labels exists for NoteId = {NoteId} | UserId = {userId}");
                    return this.BadRequest(new { success = false, Message = "You don't have any Notes!!" });
                }

                this.logger.LogInfo($"All Labels retrieved successfully for NoteId = {NoteId} | UserId = {userId}");
                return this.Ok(new { success = true, Message = "Labels Data Retrieved successfully...", data = NoteData });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPut("UpdateLabel")]
        public async Task<IActionResult> UpdateLabel(int NoteId, string LabelName)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var label = this.fundoContext.Labels.FirstOrDefault(x => x.NoteId == NoteId);
                if (label == null)
                {
                    return this.BadRequest(new { success = false, Message = $"Label not found for NoteId: {NoteId}" });
                }

                var labelName = this.fundoContext.Labels.FirstOrDefault(x => x.LabelName == LabelName);
                if (labelName == null)
                {
                    await this.labelBL.UpdateLabel(UserId, NoteId, LabelName);
                    this.logger.LogInfo($"Label Updated Successfully for NoteId={NoteId}|UserId = {userId}");
                    return Ok(new { success = true, Message = "Label Updated Successfully..." });
                }

                return this.BadRequest(new { success = false, Message = "Label with name already exsists" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpDelete("DeleteLabel")]
        public async Task<IActionResult> DeleteLabel(int NoteId, int LabelId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                bool result = await this.labelBL.DeleteLabel(UserId, NoteId, LabelId);
                if (result)
                {
                    return this.Ok(new { success = true, Message = "Label Deleted successfully..." });
                }

                return this.BadRequest(new { success = false, Message = $"Label not found for NoteId : {NoteId}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
