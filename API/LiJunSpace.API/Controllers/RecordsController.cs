using LiJunSpace.API.Dtos;
using LiJunSpace.API.Services;
using LiJunSpace.Common.Dtos.Record;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiJunSpace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
        private readonly RecordService _recordService;
        private readonly ILogger<AccountController> _logger;
        public RecordsController(RecordService recordService, ILogger<AccountController> logger)
        {
            _recordService = recordService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("upload-image")]
        public async Task<ActionResult> UploadImages([FromForm] IFormFile file)
        {
            try
            {
                var result = await _recordService.UploadRecordImageAsync(HttpContext.User.Identity!.Name!, file);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(RecordCreationDto creationDto)
        {
            try
            {
                var result = await _recordService.CreateNewRecordAsync(creationDto, HttpContext.User.Identity!.Name!);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Authorize]
        [HttpGet("{page}")]
        public async Task<ActionResult> Get(int page, string key = null, bool desc = true)
        {
            try
            {
                var result = await _recordService.GetByPageAsync(new RecordQueryDto()
                {
                    Page = page,
                    Key = key,
                    TimeDesc = desc
                });

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Authorize]
        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetDetail(string id)
        {
            try
            {
                var result = await _recordService.GetRecordAsync(id);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Authorize]
        [HttpPost("edit")]
        public async Task<ActionResult> Edit(RecordUpdateDto recordUpdateDto)
        {
            try
            {
                var result = await _recordService.EditRecordAsync(recordUpdateDto, HttpContext.User.Identity!.Name!);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Authorize]
        [HttpGet("delete/{recordId}")]
        public async Task<ActionResult> Delete(string recordId)
        {
            try
            {
                var result = await _recordService.DeleteRecordAsync(recordId, HttpContext.User.Identity!.Name!);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }
    }
}
