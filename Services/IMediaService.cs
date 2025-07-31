using GoDecola.API.Entities;

namespace GoDecola.API.Services
{
    public interface IMediaService
    {
        Task<TravelPackageMedia> UploadMediaForTravelPackageAsync(IFormFile file, int travelPackageId);
    }
}
