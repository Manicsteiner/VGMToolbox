namespace VGMToolbox.format
{
    public interface ISingleTagFormat : IFormat
    {
        string GetTagAsText();
        void UpdateTag(string pNewValue);
    }
}
