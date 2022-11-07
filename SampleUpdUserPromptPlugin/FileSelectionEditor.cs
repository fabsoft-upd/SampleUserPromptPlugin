using FabSoftUpd.Wizard;
using System.Windows;

namespace SampleUpdUserPromptPlugin
{
    public class FileSelectionEditor : FabSoftUpd.Wizard.PropertyGrid.ICustomTypeEditor
    {
        private FabSoftUpd.Wizard.PropertyGrid.PropertyItem _propertyItem;
        private FilePropertyGridControl _gridControl;


        [System.Diagnostics.Conditional("DEBUG")]
        private static void AttachDebugger() // Attach debugger if none is already attached and it is compiled for DEBUG.
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                System.Diagnostics.Debugger.Launch();
            }
        }

        public FrameworkElement ResolveEditor(FabSoftUpd.Wizard.PropertyGrid.PropertyItem propertyItem)
        {
            
            AttachDebugger();
            
            _propertyItem = propertyItem;

            _gridControl = new FilePropertyGridControl();

            _gridControl.btnBrowse.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                string filePath = ((SampleFileUserPrompt)_propertyItem.Instance).FilePath;

                System.Windows.Forms.OpenFileDialog opeFileDlg = new System.Windows.Forms.OpenFileDialog()
                {
                    FileName = filePath,
                    Multiselect = false
                };

                if (opeFileDlg.ShowDialog(new Win32WindowWrapper(WpfWindowHelper.GetParentWindowHandle((UIElement)sender))) == System.Windows.Forms.DialogResult.OK)
                {
                    ((SampleFileUserPrompt)_propertyItem.Instance).FilePath = opeFileDlg.FileName;
                }

                RefreshTextField();
            });

            RefreshTextField();

            return _gridControl;
        }

        public void RefreshTextField()
        {
            string newText = "<NULL>";
            if (_propertyItem != null)
            {
                newText = ((SampleFileUserPrompt)_propertyItem.Instance).FilePath;
            }
            _gridControl.txtFilePath.Text = newText;
        }
    }
}
