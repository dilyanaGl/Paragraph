using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace Paragraph.Services.DataServices
{
    using Data.Common;
    using Data.Models;

    public class UserService : IUserService
    {
        private readonly IRepository<ParagraphUser> userRepository;

        public UserService(IRepository<ParagraphUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public ParagraphUser SetRandomAdmin()
        {
            return this.userRepository.All().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
        }

         
    }
}
