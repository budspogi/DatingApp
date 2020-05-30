using System;
// Lesson 77 added by salvador at 28-05-2020
namespace DatingApp.API.Dtos
{
    public class UserForListDto
    {
      //  internal object photoUrl;

        public int Id { get; set;}

        public string Username {get; set;}

        public string Gender {get; set;}

        public int Age  {get; set;}

        public string KnownAs  {get; set;}

        public DateTime Created  {get; set;}

        public DateTime LastActive  {get; set;}

         public string City  {get; set;}

        public string Country  {get; set;}

        public string PhotoUrl {get; set;}
    }
}