using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.AttachmentUseCase;

namespace SagaAserhi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentController : ControllerBase
    {
        private readonly IUploadAttachmentUseCase _uploadUseCase;
        // private readonly IGetAttachmentByIdUseCase _getByIdUseCase;
        // private readonly IGetAttachmentsByClientIdUseCase _getByClientIdUseCase;
        // private readonly IDeleteAttachmentUseCase _deleteUseCase;

        public AttachmentController(
            IUploadAttachmentUseCase uploadUseCase
            // IGetAttachmentByIdUseCase getByIdUseCase,
            // IGetAttachmentsByClientIdUseCase getByClientIdUseCase,
            // IDeleteAttachmentUseCase deleteUseCase
            )
        {
            _uploadUseCase = uploadUseCase;
            // _getByIdUseCase = getByIdUseCase;
            // _getByClientIdUseCase = getByClientIdUseCase;
            // _deleteUseCase = deleteUseCase;
        }

        [HttpPost("upload/{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Upload(IFormFile file, string clientId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _uploadUseCase.ExecuteAsync(file, clientId, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //     [HttpGet("{id}")]
        //     [ProducesResponseType(StatusCodes.Status200OK)]
        //     [ProducesResponseType(StatusCodes.Status404NotFound)]
        //     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //     public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        //     {
        //         try
        //         {
        //             var result = await _getByIdUseCase.ExecuteAsync(id, cancellationToken);
        //             if (result == null)
        //                 return NotFound();
        //             return Ok(result);
        //         }
        //         catch (Exception ex)
        //         {
        //             return StatusCode(500, new { message = ex.Message });
        //         }
        //     }

        //     [HttpGet("client/{clientId}")]
        //     [ProducesResponseType(StatusCodes.Status200OK)]
        //     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //     public async Task<IActionResult> GetByClientId(string clientId, CancellationToken cancellationToken)
        //     {
        //         try
        //         {
        //             var result = await _getByClientIdUseCase.ExecuteAsync(clientId, cancellationToken);
        //             return Ok(result);
        //         }
        //         catch (Exception ex)
        //         {
        //             return StatusCode(500, new { message = ex.Message });
        //         }
        //     }

        //     [HttpDelete("{id}")]
        //     [ProducesResponseType(StatusCodes.Status200OK)]
        //     [ProducesResponseType(StatusCodes.Status404NotFound)]
        //     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //     public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        //     {
        //         try
        //         {
        //             var result = await _deleteUseCase.ExecuteAsync(id, cancellationToken);
        //             if (!result)
        //                 return NotFound();
        //             return Ok(new { message = "Archivo eliminado correctamente" });
        //         }
        //         catch (Exception ex)
        //         {
        //             return StatusCode(500, new { message = ex.Message });
        //         }
        //     }
        // }


    }
}