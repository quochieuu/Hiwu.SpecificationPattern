using System.ComponentModel;

namespace Hiwu.SpecificationPattern.Shared.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDescription(this Enum enm)
        {
            var type = enm.GetType();
            var memberInfo = type.GetMember(enm.ToString()).FirstOrDefault();

            var descriptionAttribute = memberInfo?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return descriptionAttribute?.Description ?? enm.ToString();
        }

    }
}
