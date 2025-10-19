using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendDailyMemoryReportAsync(string toEmail, IEnumerable<DTOs.MemoryDto> memories, DateTime reportDate);
        Task SendEmailAsync(string to, string subject, string body);
        Task SendWelcomeEmailAsync(string toEmail, string userName);
        Task SendMemoryConfirmationEmailAsync(string toEmail, DTOs.MemoryDto memory);
    }
}