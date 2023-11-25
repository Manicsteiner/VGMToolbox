using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VGMToolbox.format
{
    public interface IXsfTagFormat: IFormat
    {       
        string GetTitleTag();
        string GetArtistTag();
        string GetGameTag();
        string GetYearTag();
        string GetGenreTag();
        string GetCommentTag();
        string GetCopyrightTag();
        string GetXsfByTag();
        string GetTaggerTag();
        string GetVolumeTag();
        string GetLengthTag();
        string GetFadeTag();
        string GetSystemTag();

        void SetTitleTag(string pNewValue, bool AddActionToBatchFile);
        void SetArtistTag(string pNewValue, bool AddActionToBatchFile);
        void SetGameTag(string pNewValue, bool AddActionToBatchFile);
        void SetYearTag(string pNewValue, bool AddActionToBatchFile);
        void SetGenreTag(string pNewValue, bool AddActionToBatchFile);
        void SetCommentTag(string pNewValue, bool AddActionToBatchFile);
        void SetCopyrightTag(string pNewValue, bool AddActionToBatchFile);
        void SetXsfByTag(string pNewValue, bool AddActionToBatchFile);
        void SetTaggerTag(string pNewValue, bool AddActionToBatchFile);
        void SetVolumeTag(string pNewValue, bool AddActionToBatchFile);
        void SetLengthTag(string pNewValue, bool AddActionToBatchFile);
        void SetFadeTag(string pNewValue, bool AddActionToBatchFile);
        void SetSystemTag(string pNewValue, bool AddActionToBatchFile);

        void UpdateTags();
    }
}
