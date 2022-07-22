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
    }
}
