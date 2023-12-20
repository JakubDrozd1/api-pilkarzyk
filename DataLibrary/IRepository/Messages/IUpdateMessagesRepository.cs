using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository.Messages
{
    public interface IUpdateMessagesRepository
    {
        Task UpdateMessageAsync(MESSAGES message, FbTransaction? transaction = null);
        Task UpdateAnswerMessageAsync(GetMessageRequest getMessageRequest, FbTransaction? transaction = null);
    }
}
