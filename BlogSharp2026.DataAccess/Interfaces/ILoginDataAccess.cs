using BlogSharp2026.DataAccess.Model;
namespace BlogSharp2026.DataAccess.Interfaces;
public  interface ILoginDataAccess
{
    Author? GetAuthorByEmailAndPassword(LoginInfo loginInfo);
}