using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    internal interface IMessagesRepository
    {
        List<Message> GetAllMessages();
        Message? GetMessageById(int messageId);
        void AddMessage(Message message);
        void UpdateMessage(Message message);
        void DeleteMessage(int messageId);
    }
}
