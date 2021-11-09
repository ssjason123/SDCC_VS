using System;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;

namespace SDCCVSPackage
{
    /// <summary>
    ///     Basic Wizard for SDCC projects.
    /// </summary>
    public class SDCCWizard : IWizard
    {
        /// <summary>
        ///     Source form to display in the wizard.
        /// </summary>
        protected SDCCForm ConfigForm;

        /// <inheritdoc />
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind,
            object[] customParams)
        {
            try
            {
                // Create a dialog to configure the settings.
                ConfigForm = new SDCCForm();
                ConfigForm.ShowDialog();

                replacementsDictionary.Add("$porttype$", ConfigForm.PortType.Text);
                replacementsDictionary.Add("$buildformat$", ConfigForm.BuildFormat.Text);
                replacementsDictionary.Add("$emptyproj$", ConfigForm.EmptyCheck.Checked.ToString());
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        /// <inheritdoc />
        public void ProjectFinishedGenerating(Project project)
        {
        }

        /// <inheritdoc />
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        /// <inheritdoc />
        public bool ShouldAddProjectItem(string filePath)
        {
            // Always allow the filters item to be created.
            return (!ConfigForm.EmptyCheck.Checked || filePath.EndsWith(".filters"));
        }

        /// <inheritdoc />
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        /// <inheritdoc />
        public void RunFinished()
        {
        }
    }
}
