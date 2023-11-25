using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class MidiExtractorForm : AVgmtForm
    {
        public MidiExtractorForm(TreeNode pTreeNode) : 
            base(pTreeNode)

        {
            InitializeComponent();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_MidiExtractor_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_MidiExtractor_IntroText"];
            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
            
            this.grpSourceFiles.AllowDrop = true;
            this.btnDoTask.Hide();
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ExtractMidiWorker.ExtractMidiStruct bwStruct = new ExtractMidiWorker.ExtractMidiStruct();            
            bwStruct.SourcePaths = s;
            base.backgroundWorker_Execute(bwStruct);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExtractMidiWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_MidiExtractor_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_MidiExtractor_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_MidiExtractor_MessageBegin"];
        }
    }
}
