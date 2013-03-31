using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace ReSharperSecretLanguage
{
    [ActionHandler("ReSharperSecretLanguage.About")]
    public class AboutAction : IActionHandler
    {
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.Show(
                "The Secret Language\nStephan Burguchev\n\nThe Secret Language ReSharper support",
                "About The Secret Language",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }
    }
}
