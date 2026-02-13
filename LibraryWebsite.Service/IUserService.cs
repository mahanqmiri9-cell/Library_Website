using LibraryWebsite.Model;
using LibraryWebsite.Service.DTOs;
using System.Collections.Generic;

namespace LibraryWebsite.Service
{
    public interface IUserService
    {
        UserAddDTO Add(UserAddDTO user);
        UserGetByIdDTO? GetById(int id);
        List<BookGetDTO> GetAll(int PageNumber , int PageSize);
        List<UserGetDTO> GetAllUsers(int pagenumber, int pagesize);
        User GetByUserName(string username);
        void Delete(int id);
        UserUpdateDTO Update(int UserId , UserUpdateDTO dto);
        LoginResponseDTO Login(string username, string password);


    }
}
