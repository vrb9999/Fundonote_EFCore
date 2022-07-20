namespace Fundonote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.Entities;
    using DatabaseLayer.NoteModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLogger.Interface;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : Controller
    {
        private readonly ILoggerManager logger;
        private readonly FundoContext fundoContext;
        private readonly INoteBL noteBL;

        public NoteController(FundoContext fundoContext, INoteBL noteBL, ILoggerManager logger)
        {
            this.fundoContext = fundoContext;
            this.noteBL = noteBL;
            this.logger = logger;
        }

        [HttpPost("AddNote")]
        public async Task<IActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                await this.noteBL.AddNote(UserId, notePostModel);
                this.logger.LogInfo($"Note Created Successfully UserId = {userId}");
                return this.Ok(new { sucess = true, Message = "Note Created Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetALlNote")]
        public async Task<IActionResult> GetAllNote()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var NoteData = await this.noteBL.GetAllNote(UserId);
                if (NoteData.Count == 0)
                {
                    this.logger.LogInfo($"No Notes Exists At Moment!! UserId = {userId}");
                    return this.BadRequest(new { sucess = false, Message = "You Dont Have Any Notes!!" });
                }

                this.logger.LogInfo($"All Notes Retrieved Successfully UserId = {userId}");
                return this.Ok(new { sucess = true, Message = "Notes Data Retrieved successfully...", data = NoteData });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateNote/{NoteId}")]
        public async Task<IActionResult> UpdateNote(int NoteId, UpdateNoteModel updateNoteModel)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                if (updateNoteModel.Title == "string" && updateNoteModel.Description == "string" && updateNoteModel.Bgcolor == "string")
                {
                    return this.BadRequest(new { sucess = false, Message = "Please Provide Valid Fields for Note!!" });
                }

                await this.noteBL.UpdateNote(UserId, NoteId, updateNoteModel);
                this.logger.LogInfo($"Note Updates Successfully NoteId={NoteId}|UserId = {userId}");
                return Ok(new { sucess = true, Message = $"NoteId {NoteId} Updated Successfully..." });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Note Does Not Exists!!")
                {
                    this.logger.LogInfo($"No Notes Exists at Moment!!");
                    return this.BadRequest(new { sucess = false, Message = $"NoteId {NoteId} Does not Exists!!" });
                }

                throw ex;
            }
        }

        [HttpDelete("DeleteNote/{NoteId}")]
        public async Task<IActionResult> DeleteNote(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                bool result = await this.noteBL.DeleteNote(UserId, NoteId);
                if (result)
                {
                    return this.Ok(new { sucess = true, Message = "Notes Deleted successfully..." });
                }

                return this.BadRequest(new { sucess = false, Message = $"Note not found for NoteId : {NoteId}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("Archive/{NoteId}")]
        public async Task<IActionResult> IsArchive(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var res = this.fundoContext.Notes.Where(x => x.NoteId == NoteId).FirstOrDefault();
                if (res == null)
                {
                    return this.BadRequest(new { sucess = false, Message = "Note not Found" });

                }

                bool result = await this.noteBL.ArchiveNote(UserId, NoteId);
                if (result == true)
                {
                    return this.Ok(new { sucess = true, Message = "Note Archive SuccessFully !!" });
                }

                return this.Ok(new { sucess = true, Message = "Note UnArchive SuccessFully !!" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPut("PinNote/{NoteId}")]
        public async Task<IActionResult> PinNote(int NoteId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var res = this.fundoContext.Notes.Where(x => x.NoteId == NoteId).FirstOrDefault();
                if (res == null)
                {
                    return this.BadRequest(new { sucess = false, Message = "Note not Found" });
                }

                bool result = await this.noteBL.PinNote(UserId, NoteId);
                if (result == true)
                {
                    return this.Ok(new { sucess = true, Message = "Note Pin SuccessFully !!" });
                }

                return this.Ok(new { sucess = true, Message = "Note UnPin SuccessFully !!" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
