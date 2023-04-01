using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ContactsManagement.Core.Enums.ContactsManager
{
    public enum GenderOptions
    {
        Male, Female, Other
    }
    public static class Gender
    {
        public static GenderOptions Get(string? gender)
        {
            try
            {
                if (gender == null) throw new ArgumentNullException();
                return (GenderOptions)Enum.Parse(typeof(GenderOptions), gender, true);
            }
            catch
            {
                return GenderOptions.Other;
            }
        }
        public static string Get(GenderOptions? _options)
        {
            switch (_options)
            {
                case GenderOptions.Male:
                    return "Male";
                case GenderOptions.Female:
                    return "Female";
                default:
                    return "Other";
            }
        }
    }
    /* public class Gender
     {
         public GenderOptions _options;

         public Gender(GenderOptions options)
         {
             _options = options;
         }
         /// <summary>
         /// Parses a string to Enum of GenderOptions
         /// </summary>
         /// <param name="gender"></param>
         public Gender(string gender)
         {            
             _options = (GenderOptions)Enum.Parse(typeof(GenderOptions), gender,true);
         }
         public override string ToString()
         {
             switch (_options)
             {
                 case GenderOptions.Male:
                     return "Male";
                 case GenderOptions.Female:
                     return "Female";
                 default:
                     return "Other";
             }
         }
         public override bool Equals(object? obj)
         {
             if(obj == null) return false;

             Gender other = (Gender)obj;
             return (_options.CompareTo(other._options) == 0) ? true : false;
         }
         public override int GetHashCode()
         {
             return base.GetHashCode();
         }
     }*/
}
