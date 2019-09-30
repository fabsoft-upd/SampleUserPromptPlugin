using FabSoftUpd.Wizard;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace SampleUpdUserPromptPlugin
{
    public class FileSelectionEditor : ITypeEditor
    {
        private PropertyItem _propertyItem;
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

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
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
