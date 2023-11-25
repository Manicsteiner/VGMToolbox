using System;
using System.ComponentModel;
using System.IO;

using VGMToolbox.format;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.stream
{
    class MpegDemuxWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {                
        public struct MpegDemuxStruct : IVgmtWorkerStruct
        {
            public string SourceFormat { set; get; }
            
            public bool ExtractAudio { set; get; }
            public bool ExtractVideo { set; get; }

            public bool AddHeader { set; get; }
            public bool SplitAudioTracks { set; get; }
            public bool AddPlaybackHacks { set; get; }

            public string[] SourcePaths { set; get; }
        }

        public MpegDemuxWorker() :
            base() { }

        protected override void DoTaskForFile(string path, IVgmtWorkerStruct pMpegDemuxStruct, DoWorkEventArgs e)
        {
            MpegDemuxStruct demuxStruct = (MpegDemuxStruct)pMpegDemuxStruct;
            MpegStream.DemuxOptionsStruct demuxOptions = new MpegStream.DemuxOptionsStruct();

            demuxOptions.ExtractAudio = demuxStruct.ExtractAudio;
            demuxOptions.ExtractVideo = demuxStruct.ExtractVideo;

            demuxOptions.AddHeader = demuxStruct.AddHeader;
            demuxOptions.SplitAudioStreams = demuxStruct.SplitAudioTracks;
            demuxOptions.AddPlaybackHacks = demuxStruct.AddPlaybackHacks;

            switch (demuxStruct.SourceFormat)
            {
                case "ASF (MS Advanced Systems Format)":
                case "WMV (MS Advanced Systems Format)":
                    MicrosoftAsfContainer asfStream = new MicrosoftAsfContainer(path);
                    asfStream.DemultiplexStreams(demuxOptions);
                    break;
                case "BIK (Bink Video Container)":
                    BinkStream binkStream = new BinkStream(path);
                    binkStream.DemultiplexStreams(demuxOptions);
                    break;
                case "DSI (Racjin/Racdym PS2 Video)":
                    RacjinDsiStream dsiStream = new RacjinDsiStream(path);
                    dsiStream.DemultiplexStreams(demuxOptions);
                    break;
                case "DVD Video (VOB)":
                    DvdVideoStream dvdStream = new DvdVideoStream(path);
                    dvdStream.DemultiplexStreams(demuxOptions);
                    break;
                case "Electronic Arts VP6 (VP6)":
                    ElectronicArtsVp6Stream vp6Stream = new ElectronicArtsVp6Stream(path);
                    vp6Stream.DemultiplexStreams(demuxOptions);
                    break;
                case "Electronic Arts MPC (MPC)":
                    ElectronicArtsMpcStream mpcStream = new ElectronicArtsMpcStream(path);
                    mpcStream.DemultiplexStreams(demuxOptions);
                    break;
                case "H4M (Hudson GameCube Video)":
                    HudsonHvqm4VideoStream hvqmStream = new HudsonHvqm4VideoStream(path);
                    hvqmStream.DemultiplexStreams(demuxOptions);
                    break;
                case "MO (Mobiclip)":
                    MobiclipStream.MovieType movieType = MobiclipStream.GetMobiclipStreamType(path);                    

                    switch (movieType)
                    {
                        //case MobiclipStream.MovieType.NintendoDs:
                        //    MobiclipNdsStream mobiclipNdsStream = new MobiclipNdsStream(path);
                        //    mobiclipNdsStream.DemultiplexStreams(demuxOptions);
                        //    break;
                        case MobiclipStream.MovieType.Wii:
                            MobiclipWiiStream mobiclipWiiStream = new MobiclipWiiStream(path);
                            mobiclipWiiStream.DemultiplexStreams(demuxOptions);
                            break;
                        default:
                            throw new FormatException(String.Format("Unsupported Mobiclip type, for file: {0}", Path.GetFileName(path)));
                    }

                    break;
                case "MPEG":
                    int mpegType = MpegStream.GetMpegStreamType(path);
                    
                    switch (mpegType)
                    {
                        case 1:
                            Mpeg1Stream mpeg1Stream = new Mpeg1Stream(path);
                            mpeg1Stream.DemultiplexStreams(demuxOptions);
                            break;
                        case 2:
                            Mpeg2Stream mpeg2Stream = new Mpeg2Stream(path);
                            mpeg2Stream.DemultiplexStreams(demuxOptions);
                            break;
                        default:
                            throw new FormatException(String.Format("Unsupported MPEG type, for file: {0}", Path.GetFileName(path)));
                    }
                    break;
                case "MPS (PSP UMD Movie)":
                    SonyPspMpsStream mpsStream = new SonyPspMpsStream(path);
                    mpsStream.DemultiplexStreams(demuxOptions);
                    break;

                case "PAM (PlayStation Advanced Movie)":
                    SonyPamStream pamStream = new SonyPamStream(path);
                    pamStream.DemultiplexStreams(demuxOptions);
                    break;

                case "PMF (PSP Movie Format)":
                    SonyPmfStream pmfStream = new SonyPmfStream(path);
                    pmfStream.DemultiplexStreams(demuxOptions);
                    break;

                case "PSS (PlayStation Stream)":
                    SonyPssStream sps = new SonyPssStream(path);
                    sps.DemultiplexStreams(demuxOptions);
                    break;

                case "SFD (CRI Sofdec Video)":
                    SofdecStream ss = new SofdecStream(path);
                    ss.DemultiplexStreams(demuxOptions);
                    break;

                case "THP":
                    NintendoThpMovieStream thp = new NintendoThpMovieStream(path);
                    thp.DemultiplexStreams(demuxOptions);
                    break;
                case "USM (CRI Movie 2)":
                    CriUsmStream cus = new CriUsmStream(path);
                    cus.DemultiplexStreams(demuxOptions);
                    break;

                case "XMV (Xbox Media Video)":
                    XmvStream xmv = new XmvStream(path);
                    xmv.DemultiplexStreams(demuxOptions);
                    break;

                default:
                    throw new FormatException("Source format not defined.");
            }

            this.outputBuffer.Append(Path.GetFileName(path) + Environment.NewLine);
        }               
    }
}
