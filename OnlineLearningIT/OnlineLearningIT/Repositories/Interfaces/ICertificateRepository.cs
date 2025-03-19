using OnlineLearningIT.Models;

namespace OnlineLearningIT.Repositories.Interfaces
{
    public interface ICertificateRepository
    {
        Task<bool> AddNewCertificate(Certificate certificate);
        Task<bool> ValidateCertificate(Certificate certificate);
    }
}
