using GoDecola.API.Model;

namespace GoDecola.API.DTO.Response
{
    public record LoginResponseDTO
    (
        UserType userType,
        string token
    );
}
