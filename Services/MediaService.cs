using GoDecola.API.Data;
using GoDecola.API.Entities;

namespace GoDecola.API.Services
{
    public class MediaService : IMediaService
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public MediaService(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }

        public async Task<TravelPackageMedia> UploadMediaForTravelPackageAsync(IFormFile file, int travelPackageId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Arquivo inválido.");
            }

            string subfolder;
            if (file.ContentType.StartsWith("image/"))
            {
                subfolder = Path.Combine("Images", "TravelPackage");
            }
            else if (file.ContentType.StartsWith("video/")) 
            {
                subfolder = Path.Combine("Videos", "TravelPackage");
            }
            else
            {
                throw new ArgumentException("Tipo de arquivo não suportado.");
            }

            var basePath = Path.Combine(_env.WebRootPath, "Medias", subfolder);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}"; // gera um nome único para o arquivo
            var fullFilePath = Path.Combine(basePath, uniqueFileName); // caminho completo do arquivo

            // salva o arquivo no db
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var mediaEntity = new TravelPackageMedia
            {
                TravelPackageId = travelPackageId,
                FilePath = $"/Medias/{subfolder.Replace("\\", "/")}/{uniqueFileName}",
                MimeType = file.ContentType,
                UploadDate = DateTime.UtcNow
            };

            _context.TravelPackageMedias.Add(mediaEntity);
            await _context.SaveChangesAsync();

            return mediaEntity;
        }
    }
}
