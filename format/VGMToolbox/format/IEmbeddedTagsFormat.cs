namespace VGMToolbox.format
{
    public interface IEmbeddedTagsFormat : IFormat
    {
        void UpdateSongName(string pNewValue);
        void UpdateArtist(string pNewValue);
        void UpdateCopyright(string pNewValue);

        string GetSongNameAsText();
        string GetArtistAsText();
        string GetCopyrightAsText();
    }
}
