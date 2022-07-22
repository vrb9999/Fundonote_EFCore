namespace Fundonote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.Entities;
    using DatabaseLayer.NoteModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using NLogger.Interface;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : Controller
    {
        private readonly ILoggerManager logger;
        private readonly FundoContext fundoContext;
        private readonly INoteBL noteBL;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        public NoteController(FundoContext fundoContext, INoteBL noteBL, ILoggerManager logger, IDistributedCache distributedCache, IMemoryCache memoryCache)
        {
            this.fundoContext = fundoContext;
            this.noteBL = noteBL;
            this.logger = logger;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
        }

        [HttpPost("AddNote")]
        public async Task<IActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                await this.noteBL.AddNote(UserId, notePostModel);
                this.logger.LogInfo($"Note Created Successfully UserId = {userId}");
                return this.Ok(new { sucess = true, Message = "Note Created Successfully..." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetAllNote")]
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
                int UserId = int.Parse(userId.Value);
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
                    return this.Ok(new { sucess = true, Message = "Note Deleted successfully..." });
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

        [HttpPut("Trash/{NoteId}")]
        public async Task<IActionResult> TrashNote(int NoteId)
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

                bool result = await this.noteBL.TrashNote(UserId, NoteId);
                if (result == true)
                {
                    return this.Ok(new { sucess = true, Message = "Note moved to trash SuccessFully !!" });
                }

                return this.Ok(new { sucess = true, Message = "Note removed from trash SuccessFully !!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("Remainder/{NoteId}")]
        public async Task<IActionResult> Remainder(int NoteId, NoteReminderModel noteReminderModel)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = int.Parse(userId.Value);
                var remainder = Convert.ToDateTime(noteReminderModel.Remainder);
                var res = this.fundoContext.Notes.Where(x => x.NoteId == NoteId).FirstOrDefault();
                if (res == null)
                {
                    return this.BadRequest(new { sucess = false, Message = "Note not Found" });

                }

                string result = await this.noteBL.Remainder(UserId, NoteId, remainder);
                if (result != null)
                {
                    return this.Ok(new { sucess = true, Message = "Remainder set SuccessFully !! ", data = result });
                }

                return this.Ok(new { sucess = true, Message = "Remainder Deleted SuccessFully !!" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpGet("GetAllNoteUsingRedis")]
        public async Task<IActionResult> GetAllNoteUsingRedis()
        {
            try
            {
                var CacheKey = "NoteList";
                string SerializeNoteList;
                var notelist = new List<GetNoteResponse>();
                var redisnotelist = await distributedCache.GetAsync(CacheKey);
                if (redisnotelist != null)
                {
                    SerializeNoteList = Encoding.UTF8.GetString(redisnotelist);
                    notelist = JsonConvert.DeserializeObject<List<GetNoteResponse>>(SerializeNoteList);
                }
                else
                {
                    var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                    int userId = int.Parse(userid.Value);
                    notelist = await this.noteBL.GetAllNote(userId);
                    SerializeNoteList = JsonConvert.SerializeObject(notelist);
                    redisnotelist = Encoding.UTF8.GetBytes(SerializeNoteList);
                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    await distributedCache.SetAsync(CacheKey, redisnotelist, option);
                }

                return this.Ok(new { success = true, message = $"All notes fetched successfully using Redis cache", data = notelist });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
