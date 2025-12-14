using LibraryWebsite.Model;
using LibraryWebsite.Service.DTOs;
using System.Collections.Generic;

namespace LibraryWebsite.Service
{
    public interface IUserService
    {
        bool Add(User user);
        UserGetByIdDTO? GetById(int id);
        List<UserGetDTO> GetAll(int PageNumber , int PageSize);

        bool Delete(int id);
        void Update(User user);
        LoginResponseDTO Login(string username, string password);

    }
}
