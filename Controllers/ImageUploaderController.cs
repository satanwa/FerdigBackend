using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace Defence22.Controller
{
    [ApiController]
    [Route("imageuploader")]

    public class ImageUploaderController : ControllerBase
    {
        private readonly IWebHostEnvironment _hosting;


        public ImageUploaderController(IWebHostEnvironment hosting)
        {
            _hosting = hosting;
        }

        //Her har vi å laget en HttpPost som skulle sende bildeopplastning fra vue til vår wwwroot/imageFiles
        //Denne fant vi dessverre aldri helt ut om feilet her i backend eller i front end da vi prøvde oss på begge.

        [HttpPost]
        [Route("action")]
        public IActionResult PostImage(IFormFile file)
        {
            if (file != null && file.Length> 0)
            {
                var webRoothPath = _hosting.WebRootPath;
                var fileName = Path.GetFileName(file.FileName);
                var absolutePath = Path.Combine($"{webRoothPath}/imageFiles/{file.FileName}");
                //string absolutePath = Path.Combine($"{webRoothPath}/imageFiles/{file.FileName}");

                using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                {
                    try
                    {
                        file.CopyTo(fileStream);
                    }
                    catch
                    {
                       
                    }
                }

                return Ok();
            }
            else
            {
              return BadRequest();
            }

        }

    }
}
    
