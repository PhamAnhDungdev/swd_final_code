using OnlineLearningIT.Services.Interfaces;
using OnlineLearningIT.Models;
using OnlineLearningIT.Repositories.Interfaces;

namespace OnlineLearningIT.Services.Implementations
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;

        public CertificateService(ICertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }

        public Task<bool> AddNewCertificate(Certificate certificate)
        {
            return _certificateRepository.AddNewCertificate(certificate);
        }

        public Task<bool> ValidateCertificate(Certificate certificate)
        {
            return _certificateRepository.ValidateCertificate(certificate);
        }
    }
}
