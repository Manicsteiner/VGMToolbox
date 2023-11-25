using System;
using System.Collections.Generic;
using System.Text;

namespace VGMToolbox.format
{
    public interface IGd3TagFormat : IFormat
    {
        string GetTitleTagEn();
        string GetTitleTagJp();
        string GetGameTagEn();
        string GetGameTagJp();
        string GetSystemTagEn();
        string GetSystemTagJp();        
        string GetArtistTagEn();
        string GetArtistTagJp();
        string GetDateTag();
        string GetRipperTag();
        string GetCommentTag();
        
        void SetTitleTagEn(string pNewValue);
        void SetTitleTagJp(string pNewValue);
        void SetGameTagEn(string pNewValue);
        void SetGameTagJp(string pNewValue);
        void SetSystemTagEn(string pNewValue);
        void SetSystemTagJp(string pNewValue);
        void SetArtistTagEn(string pNewValue);
        void SetArtistTagJp(string pNewValue);
        void SetDateTag(string pNewValue);
        void SetRipperTag(string pNewValue);
        void SetCommentTag(string pNewValue);
       
        void UpdateTags();
    }
}
