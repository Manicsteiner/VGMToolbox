using System;
using System.Collections.Generic;
using System.Text;

namespace VGMToolbox.format
{
    public interface INezPlugPlaylistFormat : IFormat
    {
        byte[] SongName { get; }
        byte[] SongArtist { get; }
        byte[] SongCopyright { get; }
        
        byte[] StartingSong { get; }
        byte[] TotalSongs { get; }
        
        string GetNezPlugPlaylistFormat();    
    }
}
