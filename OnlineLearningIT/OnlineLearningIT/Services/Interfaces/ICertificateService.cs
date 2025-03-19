using OnlineLearningIT.Models;
namespace OnlineLearningIT.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<bool> AddNewCertificate(Certificate certificate);
        Task<bool> ValidateCertificate(Certificate certificate);
    }
}
