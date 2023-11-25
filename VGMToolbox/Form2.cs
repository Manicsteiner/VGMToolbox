using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

using VGMToolbox.forms;
using VGMToolbox.forms.audit;
using VGMToolbox.forms.compression;
using VGMToolbox.forms.examine;
using VGMToolbox.forms.extraction;
using VGMToolbox.forms.hoot;
using VGMToolbox.forms.nsf;
using VGMToolbox.forms.other;
using VGMToolbox.forms.stream;
using VGMToolbox.forms.xsf;
using VGMToolbox.util;

namespace VGMToolbox
{
    public partial class Form2 : Form
    {
        Font treeviewBoldFont;
        
        public Form2()
        {
            InitializeComponent();

            this.Text = ConfigurationManager.AppSettings["VersionString"];

            // add empty form
            ZZZ_NotYetImplemented zzz_NotYetImplemented = new ZZZ_NotYetImplemented();
            this.splitContainer1.Panel2.Controls.Add(zzz_NotYetImplemented);

            buildMenuNodes();
        }

        private void buildMenuNodes()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            this.treeviewBoldFont = new Font(this.tvMenuTree.Font, FontStyle.Bold);

            // add empty form
            EmptyForm emptyForm = new EmptyForm();
            this.splitContainer1.Panel2.Controls.Add(emptyForm);

            // set tag for displaying the empty form
            nodeTag.FormClass = emptyForm.GetType().Name;

            TreeNode rootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_RootNode"]);
            rootNode.NodeFont = this.treeviewBoldFont;
            rootNode.Tag = nodeTag;
            
            /////////
            // Tools
            /////////
            TreeNode tools_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_MiscellaneousToolsNode"]);
            tools_RootNode.Tag = nodeTag;
            tools_RootNode.NodeFont = this.treeviewBoldFont;

            // EXTRACTION                        
            TreeNode ext_RootNode = buildExtractionTreeNode();
            ext_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(ext_RootNode);

            // Stream
            TreeNode stream_RootNode = buildStreamTreeNode();
            stream_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(stream_RootNode);

            // GENH
            TreeNode genh_RootNode = buildGenhTreeNode();
            genh_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(genh_RootNode);

            // GBS
            TreeNode gbs_RootNode = buildGbsTreeNode();
            gbs_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(gbs_RootNode);

            // NSF
            TreeNode nsf_RootNode = buildNsfTreeNode();
            nsf_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(nsf_RootNode);

            // VGM
            TreeNode vgm_RootNode = buildVgmTreeNode();
            vgm_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(vgm_RootNode);

            // xSF
            TreeNode xsf_RootNode = buildXsfTreeNode();
            xsf_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(xsf_RootNode);

            // Hoot
            TreeNode hoot_RootNode = buildHootTreeNode();
            hoot_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(hoot_RootNode);

            // Compression                        
            TreeNode comp_RootNode = buildCompressionTreeNode();
            comp_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(comp_RootNode);

            // Other
            TreeNode other_RootNode = buildOtherTreeNode();
            other_RootNode.Tag = nodeTag;
            tools_RootNode.Nodes.Add(other_RootNode);

            // add Tools node to Root
            rootNode.Nodes.Add(tools_RootNode);

            ///////////
            // Examine
            ///////////
            TreeNode examine_RootNode = buildExamineTreeNode();
            examine_RootNode.Tag = nodeTag;
            rootNode.Nodes.Add(examine_RootNode);

            ////////////
            // Auditing
            ////////////
            TreeNode auditing_RootNode = buildAuditingTreeNode();
            auditing_RootNode.Tag = nodeTag;
            rootNode.Nodes.Add(auditing_RootNode);            


            // add Root node to tree
            tvMenuTree.Nodes.Add(rootNode);
            // tvMenuTree.ExpandAll();
            rootNode.Expand();
            

            tvMenuTree.NodeMouseClick += tvMenuTree_doClick;
        }

        private TreeNode buildAuditingTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            TreeNode auditing_RootNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_AuditingRootNode"]);
            
            ////////////////////
            // Datafile Creator
            ////////////////////
            TreeNode auditing_DatafileCreatorNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_DatafileCreatorNode"]);

            // add form
            DatafileCreatorForm datafileCreatorForm = 
                new DatafileCreatorForm(auditing_DatafileCreatorNode);
            this.splitContainer1.Panel2.Controls.Add(datafileCreatorForm);

            // set tag for displaying the form
            nodeTag.FormClass = datafileCreatorForm.GetType().Name;
            auditing_DatafileCreatorNode.Tag = nodeTag;

            /////////////
            // Rebuilder
            /////////////
            TreeNode auditing_RebuilderNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_RebuilderNode"]);

            // add form
            RebuilderForm auditing_RebuilderForm =
                new RebuilderForm(auditing_RebuilderNode);
            this.splitContainer1.Panel2.Controls.Add(auditing_RebuilderForm);

            // set tag for displaying the form
            nodeTag.FormClass = auditing_RebuilderForm.GetType().Name;
            auditing_RebuilderNode.Tag = nodeTag;

            ////////////////////
            // Datafile Checker
            ////////////////////
            TreeNode auditing_DatafileCheckerNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_DatafileCheckerNode"]);

            // add form
            DatafileCheckerForm auditing_DatafileCheckerForm =
                new DatafileCheckerForm(auditing_DatafileCheckerNode);
            this.splitContainer1.Panel2.Controls.Add(auditing_DatafileCheckerForm);

            // set tag for displaying the form
            nodeTag.FormClass = auditing_DatafileCheckerForm.GetType().Name;
            auditing_DatafileCheckerNode.Tag = nodeTag;            

            ///////////////////
            // Datafile Editor
            ///////////////////
            TreeNode datafileEditorNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_DatafileEditorNode"]);

            // add form
            DatafileEditorForm datafileEditorForm = new DatafileEditorForm(datafileEditorNode);
            this.splitContainer1.Panel2.Controls.Add(datafileEditorForm);

            // set tag for displaying the form
            nodeTag.FormClass = datafileEditorForm.GetType().Name;
            datafileEditorNode.Tag = nodeTag; 

            //////////////////////
            // Collection Builder
            //////////////////////
            
            /*
            TreeNode auditing_CollectionBuilderNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_CollectionBuilderNode"]);

            // add form
            Auditing_CollectionBuilderForm auditing_CollectionBuilderForm =
                new Auditing_CollectionBuilderForm(auditing_CollectionBuilderNode);
            this.splitContainer1.Panel2.Controls.Add(auditing_CollectionBuilderForm);

            // set tag for displaying the form
            nodeTag.formClass = auditing_CollectionBuilderForm.GetType().Name;
            auditing_CollectionBuilderNode.Tag = nodeTag;            
            */

            auditing_RootNode.NodeFont = this.treeviewBoldFont;

            auditing_RootNode.Nodes.Add(auditing_DatafileCreatorNode);
            auditing_RootNode.Nodes.Add(auditing_RebuilderNode);
            auditing_RootNode.Nodes.Add(auditing_DatafileCheckerNode);
            auditing_RootNode.Nodes.Add(datafileEditorNode);
            // auditing_RootNode.Nodes.Add(auditing_CollectionBuilderNode);

            return auditing_RootNode;
        }

        private TreeNode buildExamineTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            TreeNode examine_RootNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExamineRootNode"]);

            // Tag Viewer
            TreeNode examine_TagViewerNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_TagViewerNode"]);
            TagViewerForm examine_TagViewerForm = 
                new TagViewerForm(examine_TagViewerNode);
            this.splitContainer1.Panel2.Controls.Add(examine_TagViewerForm);

            nodeTag.FormClass = examine_TagViewerForm.GetType().Name;
            examine_TagViewerNode.Tag = nodeTag;

            // Crc Calculator
            TreeNode examine_CrcCalculatorNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ChecksumNode"]);
            CrcCalculatorForm examine_CrcCalculatorForm =
                new CrcCalculatorForm(examine_CrcCalculatorNode);
            this.splitContainer1.Panel2.Controls.Add(examine_CrcCalculatorForm);

            nodeTag.FormClass = examine_CrcCalculatorForm.GetType().Name;
            examine_CrcCalculatorNode.Tag = nodeTag;

            // Search for Files
            TreeNode examine_SearchForFilesNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_SearchForFilesNode"]);
            SearchForFileForm examine_SearchForFileForm =
                new SearchForFileForm(examine_SearchForFilesNode);
            this.splitContainer1.Panel2.Controls.Add(examine_SearchForFileForm);

            nodeTag.FormClass = examine_SearchForFileForm.GetType().Name;
            examine_SearchForFilesNode.Tag = nodeTag;

            examine_RootNode.NodeFont = this.treeviewBoldFont;
            examine_RootNode.Nodes.Add(examine_TagViewerNode);
            examine_RootNode.Nodes.Add(examine_CrcCalculatorNode);
            examine_RootNode.Nodes.Add(examine_SearchForFilesNode);

            return examine_RootNode;
        }

        private TreeNode buildHootTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            TreeNode hoot_RootNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_HootRootNode"]);

            //TreeNode hoot_CsvDatafileNode = 
            //    new TreeNode(ConfigurationManager.AppSettings["MenuTree_CsvDatafileNode"]);
            
            ///////////////
            // XML Builder
            ///////////////
            TreeNode hoot_XmlBuilderNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_XmlBuilderNode"]);

            // add form
            XmlBuilderForm hoot_XmlBuilderForm = new XmlBuilderForm(hoot_XmlBuilderNode);
            this.splitContainer1.Panel2.Controls.Add(hoot_XmlBuilderForm);

            // set tag for displaying the form
            nodeTag.FormClass = hoot_XmlBuilderForm.GetType().Name;
            hoot_XmlBuilderNode.Tag = nodeTag;

            ///////////////
            // Auditor
            ///////////////
            TreeNode hoot_AuditorNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_HootAuditorNode"]);

            // add form
            HootAuditorForm hoot_AuditorForm = new HootAuditorForm(hoot_AuditorNode);
            this.splitContainer1.Panel2.Controls.Add(hoot_AuditorForm);

            // set tag for displaying the form
            nodeTag.FormClass = hoot_AuditorForm.GetType().Name;
            hoot_AuditorNode.Tag = nodeTag;


            hoot_RootNode.NodeFont = this.treeviewBoldFont;

            // hoot_RootNode.Nodes.Add(hoot_CsvDatafileNode);
            hoot_RootNode.Nodes.Add(hoot_XmlBuilderNode);
            hoot_RootNode.Nodes.Add(hoot_AuditorNode);

            return hoot_RootNode;
        }

        private TreeNode buildNsfTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();

            TreeNode nsf_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_NsfRootNode"]);
            nsf_RootNode.NodeFont = this.treeviewBoldFont;

            // NSFE to M3U
            TreeNode nsf_NsfeM3uNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_NsfeM3uNode"]);            

            // add form
            Nsfe2NsfM3uForm nsf_Nsfe2NsfM3uForm = new Nsfe2NsfM3uForm(nsf_NsfeM3uNode);
            this.splitContainer1.Panel2.Controls.Add(nsf_Nsfe2NsfM3uForm);

            // set tag for displaying the form
            nodeTag.FormClass = nsf_Nsfe2NsfM3uForm.GetType().Name;
            nsf_NsfeM3uNode.Tag = nodeTag;

            // NSF to M3U
            TreeNode nsfM3uNode =new TreeNode("NSF M3U Creator");

            // add form
            NsfToM3uForm nsfToM3uForm = new NsfToM3uForm(nsfM3uNode);
            this.splitContainer1.Panel2.Controls.Add(nsfToM3uForm);

            // set tag for displaying the form
            nodeTag.FormClass = nsfToM3uForm.GetType().Name;
            nsfM3uNode.Tag = nodeTag;

            nsf_RootNode.Nodes.Add(nsf_NsfeM3uNode);
            nsf_RootNode.Nodes.Add(nsfM3uNode);

            return nsf_RootNode;        
        }

        private TreeNode buildGbsTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();

            TreeNode gbs_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_GbsRootNode"]);
            TreeNode gbs_M3uNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_GbsM3uNode"]);
            gbs_RootNode.NodeFont = this.treeviewBoldFont;

            // add form
            Gbs_GbsToM3uForm gbs_GbsToM3uForm = new Gbs_GbsToM3uForm(gbs_M3uNode);
            this.splitContainer1.Panel2.Controls.Add(gbs_GbsToM3uForm);

            // set tag for displaying the form
            nodeTag.FormClass = gbs_GbsToM3uForm.GetType().Name;
            gbs_M3uNode.Tag = nodeTag;

            gbs_RootNode.Nodes.Add(gbs_M3uNode);

            return gbs_RootNode;
        }

        private TreeNode buildXsfTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            EmptyForm emptyForm = new EmptyForm();
            TreeNode xsf_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_XsfRootNode"]);
            TreeNode _2sf_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_2sfRootNode"]);
            TreeNode psf_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_PsfRootNode"]);
            TreeNode psf2_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_Psf2RootNode"]);
            TreeNode ssf_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SsfRootNode"]);
            TreeNode psp_RootNode = new TreeNode("PSP");

            // NDS to 2SF
            TreeNode ndsTo2sfNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_NdsTo2sfNode"]);

            // Add Form
            NdsTo2sfForm ndsTo2sfForm = new NdsTo2sfForm(ndsTo2sfNode);
            this.splitContainer1.Panel2.Controls.Add(ndsTo2sfForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = ndsTo2sfForm.GetType().Name;
            ndsTo2sfNode.Tag = nodeTag;

            //////////////
            // 2SF Ripper
            //////////////
            //TreeNode xsf_2sfRipperNode = 
            //    new TreeNode(ConfigurationManager.AppSettings["MenuTree_2sfRipperOldNode"]);
            
            //// Add 2SF Ripper Form
            //Xsf_2sfRipperForm xsf_2sfRipperForm = new Xsf_2sfRipperForm(xsf_2sfRipperNode);
            //this.splitContainer1.Panel2.Controls.Add(xsf_2sfRipperForm);

            //// Set Tag for displaying the Form
            //nodeTag.formClass = xsf_2sfRipperForm.GetType().Name;
            //xsf_2sfRipperNode.Tag = nodeTag;

            TreeNode xsf_Make2sfNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_Make2sfNode"]);

            // Add Make 2SF Form
            Make2sfForm xsf_Mk2sfForm = new Make2sfForm(xsf_Make2sfNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Mk2sfForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Mk2sfForm.GetType().Name;
            xsf_Make2sfNode.Tag = nodeTag;

            //////////////
            // 2SF Timer
            //////////////
            TreeNode xsf_2sfTimerNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_2sfTimerNode"]);

            TwoSfTimerForm xsf_2sfTimerForm = new TwoSfTimerForm(xsf_2sfTimerNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_2sfTimerForm);

            nodeTag.FormClass = xsf_2sfTimerForm.GetType().Name;
            xsf_2sfTimerNode.Tag = nodeTag;

            //////////////////
            // SDAT Optimizer
            //////////////////
            TreeNode xsf_SdatOptimizerNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SdatOptimizerNode"]);

            SdatOptimizerForm xsf_2sfSdatOptimizerForm = new SdatOptimizerForm(xsf_SdatOptimizerNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_2sfSdatOptimizerForm);

            nodeTag.FormClass = xsf_2sfSdatOptimizerForm.GetType().Name;
            xsf_SdatOptimizerNode.Tag = nodeTag;

            ////////////////////
            // 2SF TAG COPIER
            ////////////////////
            TreeNode xsf_2sfTagMigratorNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_V1toV2CopyNode"]);

            TwoSfTagsMigratorForm xsf_2sfTagsMigratorForm = new TwoSfTagsMigratorForm(xsf_2sfTagMigratorNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_2sfTagsMigratorForm);

            nodeTag.FormClass = xsf_2sfTagsMigratorForm.GetType().Name;
            xsf_2sfTagMigratorNode.Tag = nodeTag;

            //////////////////////
            // PSF2 DATA EXTRACTOR
            //////////////////////
            TreeNode psf2DataExtractorNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_Psf2DataFinderNode"]);

            // Add Form
            Psf2DataFinderForm psf2DataFinderForm = new Psf2DataFinderForm(psf2DataExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(psf2DataFinderForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = psf2DataFinderForm.GetType().Name;
            psf2DataExtractorNode.Tag = nodeTag;

            ////////////
            // MKPSF2FE
            ////////////
            TreeNode xsf_MkPsf2FENode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_MkPsf2FENode"]);

            // Add UnpkPsf2 Ripper Form
            Mkpsf2FrontEndForm xsf_Mkpsf2FrontEndForm =
                new Mkpsf2FrontEndForm(xsf_MkPsf2FENode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Mkpsf2FrontEndForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Mkpsf2FrontEndForm.GetType().Name;
            xsf_MkPsf2FENode.Tag = nodeTag;

            /////////////////////////
            // PSF2 SETTINGS CHANGER
            /////////////////////////
            TreeNode xsf_Psf2SettingsUpdaterNode = new TreeNode("PSF2 Settings Updater");

            // Add Form
            Psf2SettingsUpdaterForm xsf_Psf2SettingsUpdaterForm = new Psf2SettingsUpdaterForm(xsf_Psf2SettingsUpdaterNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Psf2SettingsUpdaterForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Psf2SettingsUpdaterForm.GetType().Name;
            xsf_Psf2SettingsUpdaterNode.Tag = nodeTag;

            //////////////
            // UNPKPSF2FE
            //////////////
            TreeNode xsf_UnPsf2FENode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_UnPkPsf2FENode"]);

            // Add UnpkPsf2 Ripper Form
            UnpackPsf2Form xsf_Unpkpsf2FrontEndForm =
                new UnpackPsf2Form(xsf_UnPsf2FENode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Unpkpsf2FrontEndForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Unpkpsf2FrontEndForm.GetType().Name;
            xsf_UnPsf2FENode.Tag = nodeTag;
            
            /////////////////
            // PSF2TOPSF2LIB
            /////////////////
            TreeNode xsf_Psf2ToPsf2LibNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_Psf2ToPsf2LibNode"]);

            // Add UnpkPsf2 Ripper Form
            Psf2ToPsf2LibForm xsf_Psf2ToPsf2LibForm =
                new Psf2ToPsf2LibForm(xsf_Psf2ToPsf2LibNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Psf2ToPsf2LibForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Psf2ToPsf2LibForm.GetType().Name;
            xsf_Psf2ToPsf2LibNode.Tag = nodeTag;

            /////////////////
            // PSF2SQEXTRACTOR
            /////////////////
            TreeNode xsf_Psf2SqExtractorNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_Psf2SqExtractorNode"]);

            // Add Form
            Psf2SqExtractorForm xsf_Psf2SqExtractorForm =
                new Psf2SqExtractorForm(xsf_Psf2SqExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Psf2SqExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Psf2SqExtractorForm.GetType().Name;
            xsf_Psf2SqExtractorNode.Tag = nodeTag;

            /////////////////
            // PSF2TIMER
            /////////////////
            TreeNode xsf_Psf2TimerNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_Psf2TimerNode"]);

            // Add UnpkPsf2 Ripper Form
            Psf2TimerForm xsf_Psf2TimerForm = new Psf2TimerForm(xsf_Psf2TimerNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Psf2TimerForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Psf2TimerForm.GetType().Name;
            xsf_Psf2TimerNode.Tag = nodeTag;

            //////////////
            // BIN2PSF FE
            //////////////
            TreeNode xsf_Bin2PsfFENode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_Bin2PsfFENode"]);

            // Add Form
            Bin2PsfFrontEndForm xsf_Bin2PsfFrontEndForm =
                new Bin2PsfFrontEndForm(xsf_Bin2PsfFENode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Bin2PsfFrontEndForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Bin2PsfFrontEndForm.GetType().Name;
            xsf_Bin2PsfFENode.Tag = nodeTag;

            ////////////////
            // VAB SPLITTER
            ////////////////
            TreeNode vabSplitterNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_VabSplitterNode"]);

            // Add Form
            VabSplitterForm vabSplitterForm = new VabSplitterForm(vabSplitterNode);
            this.splitContainer1.Panel2.Controls.Add(vabSplitterForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = vabSplitterForm.GetType().Name;
            vabSplitterNode.Tag = nodeTag;

            //////////////////
            // PSF STUB MAKER
            //////////////////
            TreeNode psfStubCreatorNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_PsfStubCreatorNode"]);

            // Add UnpkPsf2 Ripper Form
            PsfStubMakerForm psfStubMakerForm = new PsfStubMakerForm(psfStubCreatorNode);
            this.splitContainer1.Panel2.Controls.Add(psfStubMakerForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = psfStubMakerForm.GetType().Name;
            psfStubCreatorNode.Tag = nodeTag;

            //////////////////////
            // PSF Timer
            //////////////////////
            TreeNode xsf_SeqExtractNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SeqExtractorNode"]);

            // Add Form
            PsfTimerForm xsf_PsxSeqExtractForm =
                new PsfTimerForm(xsf_SeqExtractNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_PsxSeqExtractForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_PsxSeqExtractForm.GetType().Name;
            xsf_SeqExtractNode.Tag = nodeTag;

            /////////////////////////
            // EASY DRIVER EXTRACTOR
            /////////////////////////
            TreeNode easyDriverExtractorNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_EasyPsfExtractorNode"]);

            // Add Form
            EasyPsfDriverExtractorForm easyPsfDriverExtractorForm = 
                new EasyPsfDriverExtractorForm(easyDriverExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(easyPsfDriverExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = easyPsfDriverExtractorForm.GetType().Name;
            easyDriverExtractorNode.Tag = nodeTag;

            //////////////////////
            // PSF DATA EXTRACTOR
            //////////////////////
            TreeNode psfDataExtractorNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_PsfDataFinderNode"]);

            // Add Form
            PsfDataFinderForm psfDataFinderForm = new PsfDataFinderForm(psfDataExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(psfDataFinderForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = psfDataFinderForm.GetType().Name;
            psfDataExtractorNode.Tag = nodeTag;

            //////////////////////
            // SEP to SEQ EXTRACTOR
            //////////////////////
            TreeNode psxSepSeqExtractorNode = new TreeNode("SEP Splitter");

            // Add Form
            PsxSepToSeqExtractorForm psxSepToSeqExtractorForm = new PsxSepToSeqExtractorForm(psxSepSeqExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(psxSepToSeqExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = psxSepToSeqExtractorForm.GetType().Name;
            psxSepSeqExtractorNode.Tag = nodeTag;

            //////////////////////
            // PSP DATA EXTRACTOR
            //////////////////////
            TreeNode pspSeqDataExtractorNode =
                new TreeNode("PSP Data Finder");

            // Add Form
            PspSeqDataFinderForm pspSeqDataExtractorForm = new PspSeqDataFinderForm(pspSeqDataExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(pspSeqDataExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = pspSeqDataExtractorForm.GetType().Name;
            pspSeqDataExtractorNode.Tag = nodeTag;

            // SSFMAKE
            TreeNode xsf_SsfMakeFENode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SsfMakeFENode"]);
            
            SsfMakeFrontEndForm xsf_SsfMakeFrontEndForm = 
                new SsfMakeFrontEndForm(xsf_SsfMakeFENode);
            this.splitContainer1.Panel2.Controls.Add(xsf_SsfMakeFrontEndForm);

            nodeTag.FormClass = xsf_SsfMakeFrontEndForm.GetType().Name;
            xsf_SsfMakeFENode.Tag = nodeTag;

            // SSFMAKE ADVANCED
            TreeNode xsf_AdvancedSsfMakeFENode = new TreeNode("ssfmake Advanced Front End");

            SsfMakeAdvancedForm xsf_AdvancedSsfMakeFrontEndForm =
                new SsfMakeAdvancedForm(xsf_AdvancedSsfMakeFENode);
            this.splitContainer1.Panel2.Controls.Add(xsf_AdvancedSsfMakeFrontEndForm);

            nodeTag.FormClass = xsf_AdvancedSsfMakeFrontEndForm.GetType().Name;
            xsf_AdvancedSsfMakeFENode.Tag = nodeTag;

            // SEQ/TON Extractors
            TreeNode xsf_SsfSeqTonExtFENode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SsfSeqTonExtFENode"]);

            SsfSeqTonExtForm xsf_SsfSeqTonExtForm =
                new SsfSeqTonExtForm(xsf_SsfSeqTonExtFENode);
            this.splitContainer1.Panel2.Controls.Add(xsf_SsfSeqTonExtForm);

            nodeTag.FormClass = xsf_SsfSeqTonExtForm.GetType().Name;
            xsf_SsfSeqTonExtFENode.Tag = nodeTag;


            ///////////
            // XSF2EXE
            ///////////
            TreeNode xsf_xsf2ExeNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_Xsf2ExeNode"]);

            // Add Xsf2Exe Form
            Xsf2ExeForm xsf_Xsf2ExeForm = new Xsf2ExeForm(xsf_xsf2ExeNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_Xsf2ExeForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_Xsf2ExeForm.GetType().Name;
            xsf_xsf2ExeNode.Tag = nodeTag;

            //////////////////
            // XSFRECOMPRESS
            /////////////////
            TreeNode xsf_xsfRecompressNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_XsfRecompressNode"]);

            // Add XsfRecompress Form
            RecompressDataForm xsf_RecompressDataForm = new RecompressDataForm(xsf_xsfRecompressNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_RecompressDataForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_RecompressDataForm.GetType().Name;
            xsf_xsfRecompressNode.Tag = nodeTag;

            //////////////////
            // TAG EDITOR
            /////////////////
            TreeNode xsf_xsfTagEditorNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_XsfTagEditorNode"]);

            // Add XsfRecompress Form
            XsfTagEditorForm xsf_XsfTagEditorForm = new XsfTagEditorForm(xsf_xsfTagEditorNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_XsfTagEditorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_XsfTagEditorForm.GetType().Name;
            xsf_xsfTagEditorNode.Tag = nodeTag;

            nodeTag.FormClass = emptyForm.GetType().Name;
            xsf_RootNode.NodeFont = this.treeviewBoldFont;

            xsf_RootNode.Nodes.Add(xsf_xsf2ExeNode);
            xsf_RootNode.Nodes.Add(xsf_xsfRecompressNode);
            xsf_RootNode.Nodes.Add(xsf_xsfTagEditorNode);

            _2sf_RootNode.NodeFont = this.treeviewBoldFont;
            _2sf_RootNode.Tag = nodeTag;
            //_2sf_RootNode.Nodes.Add(xsf_2sfRipperNode);
            _2sf_RootNode.Nodes.Add(xsf_Make2sfNode);
            _2sf_RootNode.Nodes.Add(ndsTo2sfNode);
            _2sf_RootNode.Nodes.Add(xsf_2sfTimerNode);
            _2sf_RootNode.Nodes.Add(xsf_SdatOptimizerNode);
            _2sf_RootNode.Nodes.Add(xsf_2sfTagMigratorNode);
            
            xsf_RootNode.Nodes.Add(_2sf_RootNode);

            psf_RootNode.NodeFont = this.treeviewBoldFont;
            psf_RootNode.Tag = nodeTag;
            psf_RootNode.Nodes.Add(psfDataExtractorNode);            
            psf_RootNode.Nodes.Add(xsf_Bin2PsfFENode);
            psf_RootNode.Nodes.Add(psfStubCreatorNode);
            psf_RootNode.Nodes.Add(xsf_SeqExtractNode);
            psf_RootNode.Nodes.Add(vabSplitterNode);
            psf_RootNode.Nodes.Add(psxSepSeqExtractorNode);
            psf_RootNode.Nodes.Add(easyDriverExtractorNode);
            xsf_RootNode.Nodes.Add(psf_RootNode);

            psf2_RootNode.NodeFont = this.treeviewBoldFont;
            psf2_RootNode.Tag = nodeTag;
            psf2_RootNode.Nodes.Add(psf2DataExtractorNode);
            psf2_RootNode.Nodes.Add(xsf_MkPsf2FENode);
            psf2_RootNode.Nodes.Add(xsf_Psf2SettingsUpdaterNode);
            psf2_RootNode.Nodes.Add(xsf_UnPsf2FENode);
            psf2_RootNode.Nodes.Add(xsf_Psf2ToPsf2LibNode);
            psf2_RootNode.Nodes.Add(xsf_Psf2SqExtractorNode);
            psf2_RootNode.Nodes.Add(xsf_Psf2TimerNode);
            xsf_RootNode.Nodes.Add(psf2_RootNode);

            psp_RootNode.NodeFont = this.treeviewBoldFont;
            psp_RootNode.Tag = nodeTag;
            psp_RootNode.Nodes.Add(pspSeqDataExtractorNode);
            xsf_RootNode.Nodes.Add(psp_RootNode);

            ssf_RootNode.NodeFont = this.treeviewBoldFont;
            ssf_RootNode.Tag = nodeTag;
            ssf_RootNode.Nodes.Add(xsf_SsfMakeFENode);
            //ssf_RootNode.Nodes.Add(xsf_AdvancedSsfMakeFENode);
            ssf_RootNode.Nodes.Add(xsf_SsfSeqTonExtFENode);            
            xsf_RootNode.Nodes.Add(ssf_RootNode);

            return xsf_RootNode;
        }
        
        private TreeNode buildExtractionTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            TreeNode ext_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractionRootNode"]);
            ext_RootNode.NodeFont = this.treeviewBoldFont;

            ///////////
            // GENERIC
            ///////////
            TreeNode ext_GenericNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractionGenericRootNode"]);
            ext_GenericNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            EmptyForm emptyForm = new EmptyForm();
            nodeTag.FormClass = emptyForm.GetType().Name;
            ext_GenericNode.Tag = nodeTag;

            // Byte Remover
            TreeNode ext_ByteRemoverNode = new TreeNode("Byte Remover");

            // Add Byte Remover Form
            ByteRemoverForm extract_ByteRemoverForm = new ByteRemoverForm(ext_ByteRemoverNode);
            this.splitContainer1.Panel2.Controls.Add(extract_ByteRemoverForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_ByteRemoverForm.GetType().Name;
            ext_ByteRemoverNode.Tag = nodeTag;

            // Snakebite
            TreeNode ext_SimpleCutterNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SimpleCutterNode"]);

            // Add Snakebite Form
            SnakebiteGuiForm extract_SnakebiteGuiForm = new SnakebiteGuiForm(ext_SimpleCutterNode);
            this.splitContainer1.Panel2.Controls.Add(extract_SnakebiteGuiForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_SnakebiteGuiForm.GetType().Name;
            ext_SimpleCutterNode.Tag = nodeTag;


            // Offset Finder
            TreeNode ext_OffsetFinderNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_OffsetFinderNode"]);

            // Add Offset Finder Form
            OffsetFinderForm extract_OffsetFinderForm = new OffsetFinderForm(ext_OffsetFinderNode);
            this.splitContainer1.Panel2.Controls.Add(extract_OffsetFinderForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_OffsetFinderForm.GetType().Name;
            ext_OffsetFinderNode.Tag = nodeTag;


            // VFS Extractor
            TreeNode ext_VfsExtractorNode = new TreeNode("Virtual File System Extractor");

            // Add Offset Finder Form
            VfsExtractorForm extract_VfsExtractorForm = new VfsExtractorForm(ext_VfsExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(extract_VfsExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_VfsExtractorForm.GetType().Name;
            ext_VfsExtractorNode.Tag = nodeTag;

            // ISO Extractor
            TreeNode ext_IsoExtractorNode = new TreeNode("ISO/Archive Extractor");

            // Add Offset Finder Form
            IsoExtractorForm extract_IsoExtractorForm = new IsoExtractorForm(ext_IsoExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(extract_IsoExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_IsoExtractorForm.GetType().Name;
            ext_IsoExtractorNode.Tag = nodeTag;

            // CD-XA Extractor
            TreeNode ext_ExtractCdxaNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractCdxaNode"]);

            // Add Cdxa Extractor Form
            ExtractCdxaForm extract_ExtractCdxaForm = new ExtractCdxaForm(ext_ExtractCdxaNode);
            this.splitContainer1.Panel2.Controls.Add(extract_ExtractCdxaForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_ExtractCdxaForm.GetType().Name;
            ext_ExtractCdxaNode.Tag = nodeTag;

            // MIDI Extractor
            TreeNode ext_ExtractMidiNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractMidiNode"]);

            // Add Midi Extractor Form
            MidiExtractorForm extract_MidiExtractorForm = new MidiExtractorForm(ext_ExtractMidiNode);
            this.splitContainer1.Panel2.Controls.Add(extract_MidiExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_MidiExtractorForm.GetType().Name;
            ext_ExtractMidiNode.Tag = nodeTag;


            //----------------
            // COMMON ARCHIVES
            //----------------
            TreeNode ext_CommonArchivesNode = new TreeNode("Common Archives");
            ext_CommonArchivesNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            nodeTag.FormClass = emptyForm.GetType().Name;
            ext_CommonArchivesNode.Tag = nodeTag;

            // CRI CPK
            TreeNode ext_CriCpkExtractorNode =
                new TreeNode("CRI CPK Archive Extractor");

            // Add Snakebite Form
            CriCpkExtractorForm extract_CriCpkExtractorForm = new CriCpkExtractorForm(ext_CriCpkExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(extract_CriCpkExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_CriCpkExtractorForm.GetType().Name;
            ext_CriCpkExtractorNode.Tag = nodeTag;

            // CRI ACB/AWB
            TreeNode ext_CriAcbAwbExtractorNode =
                new TreeNode("CRI ACB/AWB Archive Extractor");

            // Add Form
            CriAcbAwbExtractorForm extract_CriAcbAwbExtractorForm = new CriAcbAwbExtractorForm(ext_CriAcbAwbExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(extract_CriAcbAwbExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_CriAcbAwbExtractorForm.GetType().Name;
            ext_CriAcbAwbExtractorNode.Tag = nodeTag;

            ///////
            // NDS
            ///////
            TreeNode ext_NdsNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractionNdsRootNode"]);
            ext_NdsNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            nodeTag.FormClass = emptyForm.GetType().Name;
            ext_NdsNode.Tag = nodeTag;

            // SDAT Extractor
            TreeNode ext_SdatExtractorNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SdatExtractorNode"]);            

            // Add SDAT Extractor Form
            SdatExtractorForm xsf_SdatExtractorForm = new SdatExtractorForm(ext_SdatExtractorNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_SdatExtractorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_SdatExtractorForm.GetType().Name;
            ext_SdatExtractorNode.Tag = nodeTag;

            // SDAT Finder
            TreeNode ext_SdatFinderNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_SdatFinderNode"]);

            // Add SDAT Extractor Form
            SdatFinderForm xsf_SdatFinderForm = new SdatFinderForm(ext_SdatFinderNode);
            this.splitContainer1.Panel2.Controls.Add(xsf_SdatFinderForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xsf_SdatFinderForm.GetType().Name;
            ext_SdatFinderNode.Tag = nodeTag;

            ////////////
            // STREAMS
            ///////////
            TreeNode ext_StreamsNode =
                new TreeNode("Streams");
            ext_StreamsNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            nodeTag.FormClass = emptyForm.GetType().Name;
            ext_StreamsNode.Tag = nodeTag;

            ///////
            // PC
            ///////
            TreeNode ext_PcNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractionPcRootNode"]);
            ext_PcNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            emptyForm = new EmptyForm();
            nodeTag.FormClass = emptyForm.GetType().Name;
            ext_PcNode.Tag = nodeTag;

            // ExtractSonyAdpcmForm
            TreeNode ext_SonyAdpcmNode =
                new TreeNode("Sony ADPCM Extractor");

            // Add ExtractSonyAdpcmForm
            ExtractSonyAdpcmForm sonyAdpcmForm = new ExtractSonyAdpcmForm(ext_SonyAdpcmNode);
            this.splitContainer1.Panel2.Controls.Add(sonyAdpcmForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = sonyAdpcmForm.GetType().Name;
            ext_SonyAdpcmNode.Tag = nodeTag;

            // ExtractAdxForm
            TreeNode ext_ExtractAdxNode = new TreeNode("CRI ADX Extractor");

            ExtractAdxForm extAdxForm = new ExtractAdxForm(ext_ExtractAdxNode);
            this.splitContainer1.Panel2.Controls.Add(extAdxForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extAdxForm.GetType().Name;
            ext_ExtractAdxNode.Tag = nodeTag;

            // ExtractHCaForm
            TreeNode ext_ExtractHcaNode = new TreeNode("CRI HCA Extractor");

            ExtractHcaForm extHcaForm = new ExtractHcaForm(ext_ExtractHcaNode);
            this.splitContainer1.Panel2.Controls.Add(extHcaForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extHcaForm.GetType().Name;
            ext_ExtractHcaNode.Tag = nodeTag;

            ///////////////////
            // OGG Extractor
            ///////////////////
            TreeNode ext_ExtractOggNode = new TreeNode("Xiph.Org OGG Extractor");

            // Add Extractor Form
            ExtractOggForm extract_ExtractOggForm = new ExtractOggForm(ext_ExtractOggNode);
            this.splitContainer1.Panel2.Controls.Add(extract_ExtractOggForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_ExtractOggForm.GetType().Name;
            ext_ExtractOggNode.Tag = nodeTag;

            ///////
            // WII
            ///////
            TreeNode ext_WiiNode =
                new TreeNode("Nintendo WII");
            ext_WiiNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            emptyForm = new EmptyForm();
            nodeTag.FormClass = emptyForm.GetType().Name;
            ext_WiiNode.Tag = nodeTag;

            ///////////////////
            // WII WAD Unpacker
            ///////////////////
            TreeNode ext_UnpackWadNode = new TreeNode("Nintendo WAD Unpacker");

            // Add Extractor Form
            UnpackNintendoWadForm extract_UnpackWadForm = new UnpackNintendoWadForm(ext_UnpackWadNode);
            this.splitContainer1.Panel2.Controls.Add(extract_UnpackWadForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_UnpackWadForm.GetType().Name;
            ext_UnpackWadNode.Tag = nodeTag;

            ///////////////////
            // Nintendo U8 Extractor
            ///////////////////
            TreeNode ext_ExtractU8Node = new TreeNode("Nintendo U8 Unpacker");

            // Add Extractor Form
            ExtractNintendoU8ArchiveForm extract_ExtractU8Form = new ExtractNintendoU8ArchiveForm(ext_ExtractU8Node);
            this.splitContainer1.Panel2.Controls.Add(extract_ExtractU8Form);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_ExtractU8Form.GetType().Name;
            ext_ExtractU8Node.Tag = nodeTag;


            ext_GenericNode.Nodes.Add(ext_ByteRemoverNode);
            ext_GenericNode.Nodes.Add(ext_SimpleCutterNode);
            ext_GenericNode.Nodes.Add(ext_OffsetFinderNode);
            ext_GenericNode.Nodes.Add(ext_VfsExtractorNode);
            ext_GenericNode.Nodes.Add(ext_IsoExtractorNode);
            ext_GenericNode.Nodes.Add(ext_ExtractMidiNode);
                   
            ext_RootNode.Nodes.Add(ext_GenericNode);

            ext_CommonArchivesNode.Nodes.Add(ext_CriCpkExtractorNode);
            ext_CommonArchivesNode.Nodes.Add(ext_CriAcbAwbExtractorNode);
            ext_RootNode.Nodes.Add(ext_CommonArchivesNode);

            ext_NdsNode.Nodes.Add(ext_SdatExtractorNode);
            ext_NdsNode.Nodes.Add(ext_SdatFinderNode);            
            ext_RootNode.Nodes.Add(ext_NdsNode);

            ext_WiiNode.Nodes.Add(ext_ExtractU8Node);
            ext_WiiNode.Nodes.Add(ext_UnpackWadNode);
            ext_RootNode.Nodes.Add(ext_WiiNode);

            ext_StreamsNode.Nodes.Add(ext_ExtractAdxNode);
            ext_StreamsNode.Nodes.Add(ext_ExtractHcaNode);
            ext_StreamsNode.Nodes.Add(ext_ExtractCdxaNode);
            ext_StreamsNode.Nodes.Add(ext_ExtractOggNode);
            ext_StreamsNode.Nodes.Add(ext_SonyAdpcmNode);
            ext_RootNode.Nodes.Add(ext_StreamsNode);

            // ext_RootNode.Nodes.Add(ext_PcNode);

            return ext_RootNode;
        }

        private TreeNode buildGenhTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();

            TreeNode genh_RootNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_GenhRootNode"]);
            genh_RootNode.NodeFont = this.treeviewBoldFont;

            // GENH Creator
            TreeNode genh_CreatorNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_GenhCreationRootNode"]);

            // Add GENH Creator Form
            Genh_CreatorForm genh_CreatorForm = new Genh_CreatorForm(genh_CreatorNode);
            this.splitContainer1.Panel2.Controls.Add(genh_CreatorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = genh_CreatorForm.GetType().Name;
            genh_CreatorNode.Tag = nodeTag;

            genh_RootNode.Nodes.Add(genh_CreatorNode);

            return genh_RootNode;
        }

        private TreeNode buildVgmTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();

            TreeNode vgm_RootNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_VgmRootNode"]);
            vgm_RootNode.NodeFont = this.treeviewBoldFont;

            // VGM Tagger
            TreeNode vgm_TaggerNode = new TreeNode(ConfigurationManager.AppSettings["MenuTree_VgmTagEditorNode"]);

            // Add  Form
            Vgm_VgmTagEditorForm vgm_VgmTagEditorForm = new Vgm_VgmTagEditorForm(vgm_TaggerNode);
            this.splitContainer1.Panel2.Controls.Add(vgm_VgmTagEditorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = vgm_VgmTagEditorForm.GetType().Name;
            vgm_TaggerNode.Tag = nodeTag;

            vgm_RootNode.Nodes.Add(vgm_TaggerNode);

            return vgm_RootNode;
        }

        private TreeNode buildCompressionTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            TreeNode comp_RootNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_CompressionRootNode"]);
            comp_RootNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            EmptyForm emptyForm = new EmptyForm();
            nodeTag.FormClass = emptyForm.GetType().Name;

            // GZIP
            TreeNode ext_ExtractGzipNode =
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractGzipNode"]);

            // Add Form
            GzipCompressionForm compression_GzipCompressionForm =
                new GzipCompressionForm(ext_ExtractGzipNode);
            this.splitContainer1.Panel2.Controls.Add(compression_GzipCompressionForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = compression_GzipCompressionForm.GetType().Name;
            ext_ExtractGzipNode.Tag = nodeTag;

            // ZLIB
            TreeNode ext_ExtractZlibNode = 
                new TreeNode(ConfigurationManager.AppSettings["MenuTree_ExtractZlibNode"]);

            // Add Form
            ZlibCompressionForm extract_ZlibExtractForm = new ZlibCompressionForm(ext_ExtractZlibNode);
            this.splitContainer1.Panel2.Controls.Add(extract_ZlibExtractForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = extract_ZlibExtractForm.GetType().Name;
            ext_ExtractZlibNode.Tag = nodeTag;

            comp_RootNode.Nodes.Add(ext_ExtractGzipNode);
            comp_RootNode.Nodes.Add(ext_ExtractZlibNode);

            return comp_RootNode;
        }

        private TreeNode buildStreamTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            EmptyForm emptyForm = new EmptyForm();

            TreeNode stream_RootNode = new TreeNode("Stream Tools");
            stream_RootNode.NodeFont = this.treeviewBoldFont;

            // XMA
            TreeNode xma_RootNode = new TreeNode("XMA");
            xma_RootNode.NodeFont = this.treeviewBoldFont;
            nodeTag.FormClass = emptyForm.GetType().Name;
            xma_RootNode.Tag = nodeTag;

            // XMA Converter
            TreeNode xmaConverterNode = new TreeNode("XMA Converter");

            // Add XMA Convertor Form
            XmaConvertForm xmaConverterForm = new XmaConvertForm(xmaConverterNode);
            this.splitContainer1.Panel2.Controls.Add(xmaConverterForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xmaConverterForm.GetType().Name;
            xmaConverterNode.Tag = nodeTag;

            // XMAsh Mash
            TreeNode xmashMashNode = new TreeNode("XMAshMash");

            // Add XMA Convertor Form
            XmashMashForm xmashMashForm = new XmashMashForm(xmashMashNode);
            this.splitContainer1.Panel2.Controls.Add(xmashMashForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = xmashMashForm.GetType().Name;
            xmashMashNode.Tag = nodeTag;

            ///////////
            // VARIOUS
            ///////////
            // MPEG Demuxer
            TreeNode mpegDemuxNode = new TreeNode("Video Demultiplexer");

            // Add MPEG Demuxer Form
            MpegDemuxForm mpegDemuxForm = new MpegDemuxForm(mpegDemuxNode);
            this.splitContainer1.Panel2.Controls.Add(mpegDemuxForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = mpegDemuxForm.GetType().Name;
            mpegDemuxNode.Tag = nodeTag;


            // POS File Maker
            TreeNode posFileMakerNode = new TreeNode(".POS File Maker");

            // Add POS Maker Form
            PosFileCreatorForm posFileCreatorForm = new PosFileCreatorForm(posFileMakerNode);
            this.splitContainer1.Panel2.Controls.Add(posFileCreatorForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = posFileCreatorForm.GetType().Name;
            posFileMakerNode.Tag = nodeTag;

            stream_RootNode.Nodes.Add(posFileMakerNode);
            stream_RootNode.Nodes.Add(mpegDemuxNode);

            xma_RootNode.Nodes.Add(xmaConverterNode);
            xma_RootNode.Nodes.Add(xmashMashNode);
            stream_RootNode.Nodes.Add(xma_RootNode);

            return stream_RootNode;
        }

        private TreeNode buildOtherTreeNode()
        {
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            TreeNode other_RootNode = new TreeNode("Other");
            other_RootNode.NodeFont = this.treeviewBoldFont;

            nodeTag = new VGMToolbox.util.NodeTagStruct();
            EmptyForm emptyForm = new EmptyForm();
            nodeTag.FormClass = emptyForm.GetType().Name;

            // INTERNAL RENAMER
            TreeNode internalRenamerNode = new TreeNode("Internal Renamer");

            // Add Form
            InternalNameFileRenamerForm other_InternalRenamerForm =
                new InternalNameFileRenamerForm(internalRenamerNode);
            this.splitContainer1.Panel2.Controls.Add(other_InternalRenamerForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = other_InternalRenamerForm.GetType().Name;
            internalRenamerNode.Tag = nodeTag;

            // EXTERNAL LIST RENAMER
            TreeNode externalRenamerNode = new TreeNode("External List Renamer");

            // Add Form
            ExternalListFileRenamerForm other_ExternalListRenamerForm =
                new ExternalListFileRenamerForm(externalRenamerNode);
            this.splitContainer1.Panel2.Controls.Add(other_ExternalListRenamerForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = other_ExternalListRenamerForm.GetType().Name;
            externalRenamerNode.Tag = nodeTag;


            // INTERLEAVER
            TreeNode interleaverNode = new TreeNode("Interleaver");

            // Add Form
            InterleaverForm other_InterleaverForm = new InterleaverForm(interleaverNode);
            this.splitContainer1.Panel2.Controls.Add(other_InterleaverForm);

            // Set Tag for displaying the Form
            nodeTag.FormClass = other_InterleaverForm.GetType().Name;
            interleaverNode.Tag = nodeTag;

            other_RootNode.Nodes.Add(internalRenamerNode);
            other_RootNode.Nodes.Add(externalRenamerNode);
            //other_RootNode.Nodes.Add(interleaverNode);

            return other_RootNode;
        }

        private void tvMenuTree_doClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                VGMToolbox.util.NodeTagStruct nts = (VGMToolbox.util.NodeTagStruct)e.Node.Tag;

                // need to fix this so it only changes if the form is not "running"
                //VgmtForm.ResetNodeColor(e.Node); 

                showForm(this.splitContainer1.Panel2.Controls, nts.FormClass);
            }
            else
            {
                showForm(this.splitContainer1.Panel2.Controls, "ZZZ_NotYetImplemented");
            }
        }

        private void showForm(Control.ControlCollection pControls, string pFormClass)
        {            
            foreach (Control ctrl in pControls)
            {
                if ((ctrl is Form)&& (!String.IsNullOrEmpty(ctrl.Name)))
                {
                    if ((!String.IsNullOrEmpty(pFormClass)) && (pFormClass.Equals(ctrl.Name)))
                    {
                        ctrl.Show();
                        ctrl.Visible = true;
                        ctrl.BringToFront();
                    }
                    else
                    {
                        ctrl.Visible = false;
                    }
                }

                // showForm(ctrl.Controls, pFormClass);
            }        
        }

        private void tvMenuTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.IsSelected && e.Node.Tag != null)
            {
                VGMToolbox.util.NodeTagStruct nts = (VGMToolbox.util.NodeTagStruct)e.Node.Tag;

                // need to fix this so it only changes if the form is not "running"
                //VgmtForm.ResetNodeColor(e.Node); 

                showForm(this.splitContainer1.Panel2.Controls, nts.FormClass);
            }
            else
            {
                showForm(this.splitContainer1.Panel2.Controls, "ZZZ_NotYetImplemented");
            }           
        }

        public object GetFormByName(string formName)
        {
            object ret = null;
            
            foreach (Control c in this.splitContainer1.Panel2.Controls)
            {
                if ((c is Form) && (!String.IsNullOrEmpty(c.Name)))
                {
                    if ((!String.IsNullOrEmpty(formName)) && (c.Name.Equals(formName)))
                    {
                        ret = c;
                        break;
                    }
                }            
            }

            return ret;
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }
    }
}
