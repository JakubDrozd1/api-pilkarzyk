using BLLLibrary.IService;
using DataLibrary.Entities;
using DataLibrary.Helper.Email;
using DataLibrary.Model.DTO.Request.EmailRequest;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using DataLibrary.UoW;
using Microsoft.Extensions.Configuration;

namespace BLLLibrary.Service
{
    public class ResetPasswordService(IUnitOfWork unitOfWork, IConfiguration configuration) : IResetPasswordService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;


        public async Task AddResetPasswordAsync(string email)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _unitOfWork.ReadUsersRepository.GetUserByEmailAsync(email) ?? throw new Exception("User is null");
                await _unitOfWork.CreateResetPasswordRepository.AddResetPasswordAsync(new GetResetPasswordRequest()
                {
                    IDUSER = user.ID_USER
                });
                var resetPassword = await _unitOfWork.ReadResetPasswordRepository.GetLastAdded(user.ID_USER) ?? throw new Exception("Reset is null");
                await _unitOfWork.SaveChangesAsync();
                string? emailSend = _configuration["MailSettings:From"] ?? throw new Exception("Sender not found");
                EMAIL_SENDER? emailSender = await _unitOfWork.ReadEmailSender.GetEmailDetailsAsync(emailSend) ?? throw new Exception("Sender not found");
                EmailSender sendmail = new(emailSender, new CancellationToken());
                await sendmail.SendRecoveryPasswordMessageAsync(new GetEmailResetPasswordRequest()
                {
                    To = email,
                    IdResetPassword = resetPassword.ID_RESET_PASSWORD ?? throw new Exception("Sender not found"),
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<RESET_PASSWORD?> GetLastAdded(int userId)
        {
            return await _unitOfWork.ReadResetPasswordRepository.GetLastAdded(userId);
        }

        public async Task<GetResetPasswordResponse?> GetResetPasswordByIdAsync(int passwordResetId)
        {
            return await _unitOfWork.ReadResetPasswordRepository.GetResetPasswordByIdAsync(passwordResetId);
        }
    }
}
