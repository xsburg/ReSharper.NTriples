using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace ReSharperNTriplesLanguage
{
    [ActionHandler("ReSharperNTriplesLanguage.About")]
    public class AboutAction : IActionHandler
    {
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.Show(
                "The NTriples Language\nStephan Burguchev\n\nThe NTriples Language ReSharper support",
                "About The NTriples Language",
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
