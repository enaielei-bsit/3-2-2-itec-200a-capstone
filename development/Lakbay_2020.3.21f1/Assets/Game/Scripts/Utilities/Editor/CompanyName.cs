using UnityEditor;

using UnityEngine;

using Com.Sabattie.Theo.ScriptTemplateSettings;

namespace Utilities {
    [CreateAssetMenu(
        fileName="COMPANY_NAME",
        menuName="ScriptTemplate/Keywords/Company Name"
    )]
    public class CompanyName : Keyword {
        public override string GetValue(string scriptPath) {
            return PlayerSettings.companyName;
        }
    }
}
