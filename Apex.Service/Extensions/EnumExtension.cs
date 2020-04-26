using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Apex.Core.Enums;

namespace Apex.Service.Extensions
{
    public static class EnumExtension
    {

        public static string Code(this ErrorKeys enumValue) => ((long)enumValue).ToString();
    }
}