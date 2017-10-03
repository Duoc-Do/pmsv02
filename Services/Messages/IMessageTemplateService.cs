//using System.Collections.Generic;
//using WebApp.Models;

//namespace WebApp.Services.Messages
//{
//    /// <summary>
//    /// Message template service
//    /// </summary>
//    public partial interface ISenMessageTemplateService
//    {
//        /// <summary>
//        /// Delete a message template
//        /// </summary>
//        /// <param name="messageTemplate">Message template</param>
//        void DeleteSenMessageTemplate(SenMessageTemplate messageTemplate);

//        /// <summary>
//        /// Inserts a message template
//        /// </summary>
//        /// <param name="messageTemplate">Message template</param>
//        void InsertSenMessageTemplate(SenMessageTemplate messageTemplate);

//        /// <summary>
//        /// Updates a message template
//        /// </summary>
//        /// <param name="messageTemplate">Message template</param>
//        void UpdateSenMessageTemplate(SenMessageTemplate messageTemplate);

//        /// <summary>
//        /// Gets a message template by identifier
//        /// </summary>
//        /// <param name="messageTemplateId">Message template identifier</param>
//        /// <returns>Message template</returns>
//        SenMessageTemplate GetSenMessageTemplateById(int messageTemplateId);

//        /// <summary>
//        /// Gets a message template by name
//        /// </summary>
//        /// <param name="messageTemplateName">Message template name</param>
//        /// <param name="storeId">Store identifier</param>
//        /// <returns>Message template</returns>
//        SenMessageTemplate GetSenMessageTemplateByName(string messageTemplateName, int storeId);

//        /// <summary>
//        /// Gets all message templates
//        /// </summary>
//        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
//        /// <returns>Message template list</returns>
//        IList<SenMessageTemplate> GetAllSenMessageTemplates(int storeId);

//        /// <summary>
//        /// Create a copy of message template with all depended data
//        /// </summary>
//        /// <param name="messageTemplate">Message template</param>
//        /// <returns>Message template copy</returns>
//        SenMessageTemplate CopySenMessageTemplate(SenMessageTemplate messageTemplate);
//    }
//}
