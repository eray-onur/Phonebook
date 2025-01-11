namespace Phonebook.Directory.Application.Models
{
    public class CommandValidationException: Exception
    {
        public CommandValidationException(string message): base(message)
        {
            
        }
    }
}
