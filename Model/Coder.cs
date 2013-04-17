using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class Coder:People
    {
       public IList<ProgrammingLanguage> LanguageList = new List<ProgrammingLanguage>() 
       {
           ProgrammingLanguage.Java,
           ProgrammingLanguage.CSharp,
           ProgrammingLanguage.C
       };
      
    }
}
