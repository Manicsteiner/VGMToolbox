using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.util;

namespace VGMToolbox.tools
{
    class MdxUtil
    {
        #region Constructor
        public MdxUtil() { }
        #endregion

        #region Variables
        Hashtable pdxHash = new Hashtable();
        
        #endregion

        #region methods

        public string getPdxForFile(string pFileName, TreeNode pParentNode)
        {
            string ret = null;
            FileInfo fi = new FileInfo(pFileName);
            if (fi.Exists && fi.Extension.Equals(Mdx.MDX_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
            {
                FileStream fs = File.OpenRead(pFileName);
                byte[] data = new byte[fs.Length];
                FileUtil.ReadWholeArray(fs, data);
                fs.Close();
                fs.Dispose();
                Mdx mdx = new Mdx(data);
                ret = mdx.PdxFileName;

                // Populate Tree View
                if (pParentNode != null)
                { 
                    // create file node
                    TreeNode fileNode = new TreeNode(Path.GetFileName(pFileName));
                    TreeNode titleNode = new TreeNode("Title: " + mdx.Title);
                    TreeNode pdxNode = new TreeNode("PDX: " + mdx.PdxFileName);

                    if (!fileNode.Text.Equals(pParentNode.Text))
                    {
                        fileNode.Nodes.Add(titleNode);
                        fileNode.Nodes.Add(pdxNode);
                        pParentNode.Nodes.Add(fileNode);
                    }
                    else
                    {
                        pParentNode.Nodes.Add(titleNode);
                        pParentNode.Nodes.Add(pdxNode);
                    }
                }
            }
            return ret;
        }

        public void getPdxForFile(string pFileName, TextBox pOutputWindow, TreeNode pParentNode, bool pCheckExists)
        {
            string pdxPath = null;

            // get PDX file
            Mdx mdx = getMdxForFile(pFileName);
            string pdxFileName = mdx.PdxFileName;

            if (pdxFileName != null && pdxFileName != String.Empty)
            {
                // add file extension if needed
                if (!pdxFileName.EndsWith(Mdx.PDX_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
                {
                    pdxFileName += Mdx.PDX_FILE_EXTENSION;
                }
                
                // build PDX path
                pdxPath = Path.Combine(Path.GetDirectoryName(pFileName), pdxFileName);

                if (!pdxHash.ContainsKey(pdxPath))
                {                                        
                    // Add to hash
                    pdxHash.Add(pdxPath, pdxPath);


                    if (pCheckExists)
                    {
                        pOutputWindow.Text += pdxPath;

                        if (File.Exists(pdxPath))
                        {
                            pOutputWindow.Text += " (Found)";
                        }
                        else
                        {
                            pOutputWindow.Text += " (Not Found)";
                        }
                    }
                    else
                    {
                         pOutputWindow.Text += pdxFileName;
                    }
                    pOutputWindow.Text += Environment.NewLine;
                }
            }
            buildMdxTreeNode(mdx, pFileName, pParentNode);
        }

        public void getPdxForDir(string pDirectory, TextBox pOutputWindow, TreeNode pParentNode, bool pCheckExists)
        {
            foreach (string f in Directory.GetFiles(pDirectory, "*.mdx"))
            {
                getPdxForFile(f, pOutputWindow, pParentNode, pCheckExists);
            }
        }

        public Mdx getMdxForFile(string pFileName)
        {
            Mdx ret = null;
            FileInfo fi = new FileInfo(pFileName);
            if (fi.Exists && fi.Extension.Equals(Mdx.MDX_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
            {
                FileStream fs = File.OpenRead(pFileName);
                byte[] data = new byte[fs.Length];
                FileUtil.ReadWholeArray(fs, data);
                fs.Close();
                fs.Dispose();
                Mdx mdx = new Mdx(data);
                ret = mdx;                
            }
            return ret;
        }
        
        public void buildMdxTreeNode(Mdx pMdx, string pFileName, TreeNode pParentNode)
        { 
            // Populate Tree View
            if (pParentNode != null)
            {
                // create file node
                TreeNode fileNode = new TreeNode(Path.GetFileName(pFileName));
                TreeNode titleNode = new TreeNode("Title: " + pMdx.Title);
                TreeNode pdxNode = new TreeNode("PDX: " + pMdx.PdxFileName);

                if (!fileNode.Text.Equals(pParentNode.Text))
                {
                    fileNode.Nodes.Add(titleNode);
                    fileNode.Nodes.Add(pdxNode);
                    pParentNode.Nodes.Add(fileNode);
                }
                else
                {
                    pParentNode.Nodes.Add(titleNode);
                    pParentNode.Nodes.Add(pdxNode);
                }
            }
        }
        
        #endregion

    }
}
