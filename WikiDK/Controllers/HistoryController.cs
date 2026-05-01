using Microsoft.AspNetCore.Mvc;
using WikiDK.Services;

namespace WikiDK.Controllers
{
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        public HistoryService _historyService;
        public HistoryController(HistoryService historyService) { _historyService = historyService; }

        /// <summary>
        /// Gets a history record by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetHistoryById(int id)
        {
            var history = await _historyService.GetHistoryById(id);
            if (history == null)
            {
                return NotFound();
            }
            return Ok(history);
        }
        /// <summary>
        /// Gets history records associated with a specific article by its ID.
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet("get/all/article/{articleId}")]
        public async Task<IActionResult> GetHistoryByArticleId(int articleId)
        {
            var histories = await _historyService.GetAllHistoriesAssociatedWithArticle(articleId);
            return Ok(histories);
        }
        /// <summary>
        /// Gets history records associated with a specific editor by their ID.
        /// </summary>
        /// <param name="editorId"></param>
        /// <returns></returns>
        [HttpGet("get/all/editor/{editorId}")]
        public async Task<IActionResult> GetHistoryByEditorId(int editorId)
        {
            var histories = await _historyService.GetAllHistoriesAssociatedWithEditor(editorId);
            return Ok(histories);
        }
    }
}
