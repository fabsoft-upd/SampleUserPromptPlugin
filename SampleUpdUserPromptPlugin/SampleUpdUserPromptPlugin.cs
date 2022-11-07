using System;
using System.ComponentModel;
using System.Windows;
using System.IO;
using FabSoftUpd.Wizard.Workflows_v1.UserPrompts;
using System.Collections.Generic;
using System.Windows.Input;

namespace SampleUpdUserPromptPlugin
{
    [Serializable]
    public class SampleFileUserPrompt : UserPromptBase
    {
        public override string PromptTypeDescription { get { return "Sample Lookup File Prompt Plugin"; } }

        [DisplayName("Max Line Length")]
        public int MaxLength { get; set; } = int.MaxValue;

        [DisplayName("Min Line Length")]
        public int MinLength { get; set; } = 0;

        [Browsable(false)] // To hide a property from being visible in the UI, use [Browsable(False)]
        public int HiddenProperty { get; set; } = 0;

        [DisplayName("Lookup File")]
        [FabSoftUpd.Wizard.PropertyGrid.Attributes.PropertyOrderAttribute(1)] // To specify the order of properties, use [FabSoftUpd.Wizard.PropertyGrid.Attributes.PropertyOrderAttribute(#)]
        [FabSoftUpd.Wizard.PropertyGrid.Attributes.EditorTypeAttribute(typeof(FileSelectionEditor))] // To customize the editor for this property in the Property Grid, use [FabSoftUpd.Wizard.PropertyGrid.Attributes.EditorTypeAttribute(typeof(<EDITOR>))]
        public string FilePath { get; set; } = null;

        public SampleFileUserPrompt() { }

        public SampleFileUserPrompt(string name, string label)
        {
            Name = name;
            Label = label;
        }


        public override UserPromptBase Clone()
        {
            SampleFileUserPrompt newProperty = new SampleFileUserPrompt();
            CopyTo(newProperty);
            return newProperty;
        }

        public override bool Configure()
        {
            return true;
        }

        public override void CopyTo(UserPromptBase targetProperty)
        {
            if (typeof(SampleFileUserPrompt).IsAssignableFrom(targetProperty.GetType()))
            {
                CopyTo((SampleFileUserPrompt)targetProperty);
            }
        }

        public void CopyTo(SampleFileUserPrompt targetProperty)
        {
            base.CopyTo(targetProperty);

            targetProperty.MaxLength = this.MaxLength;
            targetProperty.MinLength = this.MinLength;
            targetProperty.HiddenProperty = this.HiddenProperty;
            targetProperty.FilePath = this.FilePath;
        }


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

        private PromptControls _PromptControls = null;
        public override UIElement GetControl(out ValidateResult validator, string defaultValue, string troubleshootingInfo = null, Dictionary<Key, Action<Window, string>> altKeyCallbacks = null)
        {
            AttachDebugger();
            
            validator = new ValidateResult((out string errorMessage) => {
                errorMessage = null;
                return true;
            });

            _PromptControls = new PromptControls();

            _PromptControls.btnRefresh.Click += ((object sender, RoutedEventArgs e) =>
            {
                try
                {
                    _PromptControls.cmbLookup.ItemsSource = LoadFile();
                }
                catch (Exception)
                {
                    // Ignore
                }
            });

            _PromptControls.cmbLookup.ItemsSource = LoadFile();
            _PromptControls.cmbLookup.Text = defaultValue;

            return _PromptControls;
        }

        private List<string> LoadFile()
        {
            List<string> fileData = new List<string>();
            using (FileStream fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        fileData.Add(sr.ReadLine());
                    }
                }
            }
            return fileData;
        }

        public override bool Validate(UIElement control)
        {
            try
            {
                string resultValue = _PromptControls?.cmbLookup?.SelectedItem?.ToString();
                if (resultValue != null && resultValue.Length >= MinLength && resultValue.Length <= MaxLength)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // Ignore
            }
            return false;
        }

        public override void CalculateResult(UIElement control, int pageIndex)
        {
            string result = _PromptControls?.cmbLookup?.SelectedItem?.ToString();
            SetResult(pageIndex, result);
        }

        public override void Reset()
        {
            // Not used
        }
    }
}
