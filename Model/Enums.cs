using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Model
{
    public enum ProgrammingLanguage
    {
        [Description("C#语言")]
        CSharp = 1,
        [Description("Java语言")]
        Java = 2,
        [Description("C语言")]
        C = 3,
        [Description("CPP语言")]
        CPP = 4
    }
}
