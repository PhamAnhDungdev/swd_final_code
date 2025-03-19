using OnlineLearningIT.Models;
using OnlineLearningIT.Repositories.Interfaces;

namespace OnlineLearningIT.Repositories.Implementations
{
    public class CertificateRepository : ICertificateRepository
    {

        private OnlineLearningItContext _context;

        public CertificateRepository(OnlineLearningItContext context)
        {
            _context = context;
        }
        public async Task<bool> AddNewCertificate(Certificate certificate)
        {
            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateCertificate(Certificate certificate)
        {
            // Kiểm tra null trước tiên
            if (certificate == null)
            {
                await Console.Out.WriteLineAsync("Certificate is null");
                return false;
            }

            // Log thông tin an toàn
            await Console.Out.WriteLineAsync($"Validating Certificate: Title={certificate.Title ?? "null"}, " +
                $"Description={(certificate.Description?.Length > 20 ? certificate.Description.Substring(0, 20) + "..." : certificate.Description ?? "null")}, " +
                $"TemplateUrl={certificate.TemplateUrl ?? "null"}");

            // Tiếp tục với các kiểm tra khác
            if (string.IsNullOrWhiteSpace(certificate.Title) || certificate.Title.Length > 200)
            {
                await Console.Out.WriteLineAsync("Title validation failed");
                return false;
            }

            if (certificate.Description != null && certificate.Description.Length > 1000)
            {
                await Console.Out.WriteLineAsync("Description validation failed");
                return false;
            }

            if (certificate.TemplateUrl != null)
            {
                bool isValidUrl = Uri.TryCreate(certificate.TemplateUrl, UriKind.Absolute, out _);
                if (!isValidUrl)
                {
                    await Console.Out.WriteLineAsync($"URL validation failed: {certificate.TemplateUrl}");
                    return false;
                }
            }

            await Console.Out.WriteLineAsync("Certificate validation passed");
            return true;
        }
    }
}
