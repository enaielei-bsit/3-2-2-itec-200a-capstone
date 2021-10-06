/*
 * Date Created: Wednesday, October 6, 2021 4:08 PM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 */

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
