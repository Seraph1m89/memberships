using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Memberships.Areas.Admin.Models
{
    public class ButtonModel
    {
        public ButtonModel()
        {
            AdditionalParameters = new Dictionary<string, string>();
        }

        public ButtonModel(string defaultId) : this()
        {
            AddOrUpdateParameter("Id", defaultId);
        }

        public string Action { get; set; }

        public string Text { get; set; }

        public string Glyph { get; set; }

        public ButtonVisualType ButtonType { get; set; }

        public Dictionary<string, string> AdditionalParameters { get; set; }

        public void AddOrUpdateParameter(string parameterName, string parameterValue)
        {
            AdditionalParameters[parameterName] = parameterValue;
        }

        public string GetActionParameters()
        {
            var sb = new StringBuilder("?");

            foreach (var keyValue in AdditionalParameters)
            {
                if (int.TryParse(keyValue.Value, out var intValue))
                {
                    if (intValue > 0)
                    {
                        sb.Append($"{char.ToLowerInvariant(keyValue.Key[0]) + keyValue.Key.Substring(1)}={keyValue.Value}&");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(keyValue.Value))
                {
                    sb.Append(
                        $"{char.ToLowerInvariant(keyValue.Key[0]) + keyValue.Key.Substring(1)}={keyValue.Value}&");
                }
            }

            return sb.ToString().Substring(0, sb.Length - 1);
        }

        public string GetButtonClass()
        {
            var classStart = "btn-";

            switch (ButtonType)
            {
                case ButtonVisualType.Success:
                    return $"{classStart}success";
                case ButtonVisualType.Danger:
                    return $"{classStart}danger";
                case ButtonVisualType.Warning:
                    return $"{classStart}warning";
                default:
                    return $"{classStart}{ButtonType.ToString().ToLowerInvariant()}";
            }
        }
    }

    public enum ButtonVisualType
    {
        Success,
        Danger,
        Warning,
        Primary,
        Info
    }
}