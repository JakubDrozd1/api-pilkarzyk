using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Entities;

namespace DataLibrary.IRepository.Messages
{
    public interface IDeleteMessagesRepository
    {
        Task DeleteMessageAsync(int messageId);
    }
}
