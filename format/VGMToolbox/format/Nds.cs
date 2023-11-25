using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Nds : IFormat
    {
        private const string FormatAbbreviation = "NDS";
        public const string FileExtension = ".NDS";

        public string FilePath { get; set; }
        public byte[] GameTitle { get; set; }
        public byte[] GameCode { get; set; }
        public byte[] MakerCode { get; set; }
        public string GameSerial { get; set; }

        private Dictionary<char, string> countryCodeHash = new Dictionary<char, string>();
        private Dictionary<string, string> makerCodeHash = new Dictionary<string, string>();
        private Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public byte[] GetAsciiSignature()
        {
            return null;
        }
        
        public string GetFileExtensions()
        {
            return Nds.FileExtension;
        }

        public void Initialize(Stream pStream, string pFilePath)
        {
            char countryCode;
            string makerCode;
            
            this.initializeCountryCodes();
            this.initializeMakerCodes();

            this.FilePath = pFilePath;
            this.GameTitle = ParseFile.ParseSimpleOffset(pStream, 0, 0x0C);
            this.GameCode = ParseFile.ParseSimpleOffset(pStream, 0x0C, 4);
            this.MakerCode = ParseFile.ParseSimpleOffset(pStream, 0x10, 2);

            countryCode = VGMToolbox.util.ByteConversion.GetAsciiText(this.GameCode)[3];
            makerCode = VGMToolbox.util.ByteConversion.GetAsciiText(this.MakerCode);

            if (this.countryCodeHash.ContainsKey(countryCode))
            {
                this.GameSerial = String.Format("NTR-{0}-{1}", VGMToolbox.util.ByteConversion.GetAsciiText(this.GameCode), this.countryCodeHash[countryCode]);
            }
            else
            {
                this.GameSerial = null;
            }


            this.tagHash.Add("Game Title", VGMToolbox.util.ByteConversion.GetAsciiText(this.GameTitle));
            this.tagHash.Add("Game Serial", this.GameSerial);

            if (this.makerCodeHash.ContainsKey(makerCode))
            {
                this.tagHash.Add("Maker Code", String.Format("{0} ({1})", makerCode, this.makerCodeHash[makerCode]));
            }
            else
            {
                this.tagHash.Add("Maker Code", String.Format("{0}", makerCode));
            }
        }

        public string GetFormatAbbreviation()
        {
            return Nds.FormatAbbreviation;
        }
        
        public bool IsFileLibrary()
        {
            return false;
        }
        
        public bool HasMultipleFileExtensions()
        {
            return false;
        }

        public bool UsesLibraries()
        {
            return false;    
        }

        public bool IsLibraryPresent()
        {
            return true;
        }

        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum, ref CryptoStream pMd5CryptoStream,
            ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();
        }
    
        /// <summary>
        /// Initializes hash table of country codes.
        /// </summary>
        private void initializeCountryCodes()
        {
            this.countryCodeHash.Add('J', "JPN");
            this.countryCodeHash.Add('E', "USA");
            this.countryCodeHash.Add('P', "EUR");
            this.countryCodeHash.Add('D', "NOE");
            this.countryCodeHash.Add('F', "NOE");
            this.countryCodeHash.Add('I', "ITA");
            this.countryCodeHash.Add('S', "SPA");
            this.countryCodeHash.Add('H', "HOL");
            this.countryCodeHash.Add('K', "KOR");
            this.countryCodeHash.Add('X', "EUU");
        }


        /// <summary>
        /// Initializes hash table of maker codes.  List source in "\external\docs", courtesy of FluBBa.
        /// </summary>
        private void initializeMakerCodes()
        {
            this.makerCodeHash.Add("01", "Nintendo");
            this.makerCodeHash.Add("02", "Rocket Games/Ajinomoto");
            this.makerCodeHash.Add("03", "Imagineer-Zoom");
            this.makerCodeHash.Add("04", "Gray Matter?");
            this.makerCodeHash.Add("05", "Zamuse");
            this.makerCodeHash.Add("06", "Falcom");
            this.makerCodeHash.Add("07", "Enix?");
            this.makerCodeHash.Add("08", "Capcom");
            this.makerCodeHash.Add("09", "Hot B Co.");
            this.makerCodeHash.Add("0A", "Jaleco");
            this.makerCodeHash.Add("0B", "Coconuts Japan");
            this.makerCodeHash.Add("0C", "Coconuts Japan/G.X.Media");
            this.makerCodeHash.Add("0D", "Micronet?");
            this.makerCodeHash.Add("0E", "Technos");
            this.makerCodeHash.Add("0F", "Mebio Software");
            this.makerCodeHash.Add("0G", "Shouei System");
            this.makerCodeHash.Add("0H", "Starfish");
            this.makerCodeHash.Add("0J", "Mitsui Fudosan/Dentsu");
            this.makerCodeHash.Add("0L", "Warashi Inc.");
            this.makerCodeHash.Add("0N", "Nowpro");
            this.makerCodeHash.Add("0P", "Game Village");
            this.makerCodeHash.Add("0Q", "IE Institute");
            this.makerCodeHash.Add("10", "?????????????");
            this.makerCodeHash.Add("12", "Infocom");
            this.makerCodeHash.Add("13", "Electronic Arts Japan");
            this.makerCodeHash.Add("15", "Cobra Team");
            this.makerCodeHash.Add("16", "Human/Field");
            this.makerCodeHash.Add("17", "KOEI");
            this.makerCodeHash.Add("18", "Hudson Soft");
            this.makerCodeHash.Add("19", "S.C.P.");
            this.makerCodeHash.Add("1A", "Yanoman");
            this.makerCodeHash.Add("1C", "Tecmo Products");
            this.makerCodeHash.Add("1D", "Japan Glary Business");
            this.makerCodeHash.Add("1E", "Forum/OpenSystem");
            this.makerCodeHash.Add("1F", "Virgin Games (Japan)");
            this.makerCodeHash.Add("1G", "SMDE");
            this.makerCodeHash.Add("1J", "Daikokudenki");
            this.makerCodeHash.Add("1P", "Creatures Inc.");
            this.makerCodeHash.Add("1Q", "TDK Deep Impresion");
            this.makerCodeHash.Add("20", "Destination Software/KSS");
            this.makerCodeHash.Add("21", "Sunsoft/Tokai Engineering??");
            this.makerCodeHash.Add("22", "POW (Planning Office Wada)/VR 1 Japan??");
            this.makerCodeHash.Add("23", "Micro World");
            this.makerCodeHash.Add("25", "San-X");
            this.makerCodeHash.Add("26", "Enix");
            this.makerCodeHash.Add("27", "Loriciel/Electro Brain");
            this.makerCodeHash.Add("28", "Kemco Japan");
            this.makerCodeHash.Add("29", "Seta");
            this.makerCodeHash.Add("2A", "Culture Brain");
            this.makerCodeHash.Add("2C", "Palsoft");
            this.makerCodeHash.Add("2D", "Visit Co.,Ltd.");
            this.makerCodeHash.Add("2E", "Intec");
            this.makerCodeHash.Add("2F", "System Sacom");
            this.makerCodeHash.Add("2G", "Poppo");
            this.makerCodeHash.Add("2H", "Ubisoft Japan");
            this.makerCodeHash.Add("2J", "Media Works");
            this.makerCodeHash.Add("2K", "NEC InterChannel");
            this.makerCodeHash.Add("2L", "Tam");
            this.makerCodeHash.Add("2M", "Jordan");
            this.makerCodeHash.Add("2N", "Smilesoft?, Rocket?");
            this.makerCodeHash.Add("2Q", "Mediakite");
            this.makerCodeHash.Add("30", "Viacom");
            this.makerCodeHash.Add("31", "Carrozzeria");
            this.makerCodeHash.Add("32", "Dynamic");
            this.makerCodeHash.Add("34", "Magifact");
            this.makerCodeHash.Add("35", "Hect");
            this.makerCodeHash.Add("36", "Codemasters");
            this.makerCodeHash.Add("37", "Taito/GAGA Communications");
            this.makerCodeHash.Add("38", "Laguna");
            this.makerCodeHash.Add("39", "Telstar Fun & Games, Event/Taito");
            this.makerCodeHash.Add("3B", "Arcade Zone Ltd");
            this.makerCodeHash.Add("3C", "Entertainment International/Empire Software?");
            this.makerCodeHash.Add("3D", "Loriciel");
            this.makerCodeHash.Add("3E", "Gremlin Graphics");
            this.makerCodeHash.Add("3F", "K.Amusement Leasing Co.");
            this.makerCodeHash.Add("40", "Seika Corp.");
            this.makerCodeHash.Add("41", "Ubi Soft Entertainment");
            this.makerCodeHash.Add("42", "Sunsoft US?");
            this.makerCodeHash.Add("44", "Life Fitness");
            this.makerCodeHash.Add("46", "System 3");
            this.makerCodeHash.Add("47", "Spectrum Holobyte");
            this.makerCodeHash.Add("49", "IREM");
            this.makerCodeHash.Add("4B", "Raya Systems");
            this.makerCodeHash.Add("4C", "Renovation Products");
            this.makerCodeHash.Add("4D", "Malibu Games");
            this.makerCodeHash.Add("4F", "Eidos, U.S. Gold (<=1995)");
            this.makerCodeHash.Add("4G", "Playmates Interactive?");
            this.makerCodeHash.Add("4J", "Fox Interactive");
            this.makerCodeHash.Add("4K", "Time Warner Interactive");
            this.makerCodeHash.Add("4Q", "Disney Interactive");
            this.makerCodeHash.Add("4S", "Black Pearl");
            this.makerCodeHash.Add("4U", "Advanced Productions");
            this.makerCodeHash.Add("4X", "GT Interactive");
            this.makerCodeHash.Add("4Y", "RARE");
            this.makerCodeHash.Add("4Z", "Crave Entertainment");
            this.makerCodeHash.Add("50", "Absolute Entertainment");
            this.makerCodeHash.Add("51", "Acclaim");
            this.makerCodeHash.Add("52", "Activision");
            this.makerCodeHash.Add("53", "American Sammy");
            this.makerCodeHash.Add("54", "Take 2 Interactive, GameTek");
            this.makerCodeHash.Add("55", "Hi Tech");
            this.makerCodeHash.Add("56", "LJN LTD.");
            this.makerCodeHash.Add("58", "Mattel");
            this.makerCodeHash.Add("5A", "Mindscape, Red Orb Entertainment?");
            this.makerCodeHash.Add("5B", "Romstar");
            this.makerCodeHash.Add("5C", "Taxan");
            this.makerCodeHash.Add("5D", "Midway, Tradewest");
            this.makerCodeHash.Add("5F", "American Softworks");
            this.makerCodeHash.Add("5G", "Majesco Sales Inc");
            this.makerCodeHash.Add("5H", "3DO");
            this.makerCodeHash.Add("5K", "Hasbro");
            this.makerCodeHash.Add("5L", "NewKidCo");
            this.makerCodeHash.Add("5M", "Telegames");
            this.makerCodeHash.Add("5N", "Metro3D");
            this.makerCodeHash.Add("5P", "Vatical Entertainment");
            this.makerCodeHash.Add("5Q", "LEGO Media");
            this.makerCodeHash.Add("5S", "Xicat Interactive");
            this.makerCodeHash.Add("5T", "Cryo Interactive");
            this.makerCodeHash.Add("5W", "Red Storm Entertainment");
            this.makerCodeHash.Add("5X", "Microids");
            this.makerCodeHash.Add("5Z", "Conspiracy/Swing");
            this.makerCodeHash.Add("60", "Titus");
            this.makerCodeHash.Add("61", "Virgin Interactive");
            this.makerCodeHash.Add("62", "Maxis");
            this.makerCodeHash.Add("64", "LucasArts Entertainment");
            this.makerCodeHash.Add("67", "Ocean");
            this.makerCodeHash.Add("69", "Electronic Arts");
            this.makerCodeHash.Add("6B", "Laser Beam");
            this.makerCodeHash.Add("6E", "Elite Systems");
            this.makerCodeHash.Add("6F", "Electro Brain");
            this.makerCodeHash.Add("6G", "The Learning Company");
            this.makerCodeHash.Add("6H", "BBC");
            this.makerCodeHash.Add("6J", "Software 2000");
            this.makerCodeHash.Add("6L", "BAM! Entertainment");
            this.makerCodeHash.Add("6M", "Studio 3");
            this.makerCodeHash.Add("6Q", "Classified Games");
            this.makerCodeHash.Add("6S", "TDK Mediactive");
            this.makerCodeHash.Add("6U", "DreamCatcher");
            this.makerCodeHash.Add("6V", "JoWood Produtions");
            this.makerCodeHash.Add("6W", "SEGA");
            this.makerCodeHash.Add("6X", "Wannado Edition");
            this.makerCodeHash.Add("6Y", "LSP (Light & Shadow Prod.)");
            this.makerCodeHash.Add("6Z", "ITE Media");
            this.makerCodeHash.Add("70", "Infogrames");
            this.makerCodeHash.Add("71", "Interplay");
            this.makerCodeHash.Add("72", "JVC (US)");
            this.makerCodeHash.Add("73", "Parker Brothers");
            this.makerCodeHash.Add("75", "Sales Curve(Storm/SCI)");
            this.makerCodeHash.Add("78", "THQ");
            this.makerCodeHash.Add("79", "Accolade");
            this.makerCodeHash.Add("7A", "Triffix Entertainment");
            this.makerCodeHash.Add("7C", "Microprose Software");
            this.makerCodeHash.Add("7D", "Universal Interactive, Sierra, Simon & Schuster?");
            this.makerCodeHash.Add("7F", "Kemco");
            this.makerCodeHash.Add("7G", "Rage Software");
            this.makerCodeHash.Add("7H", "Encore");
            this.makerCodeHash.Add("7J", "Zoo");
            this.makerCodeHash.Add("7K", "Kiddinx");
            this.makerCodeHash.Add("7L", "Simon & Schuster Interactive");
            this.makerCodeHash.Add("7M", "Asmik Ace Entertainment Inc./AIA");
            this.makerCodeHash.Add("7N", "Empire Interactive?");
            this.makerCodeHash.Add("7Q", "Jester Interactive");
            this.makerCodeHash.Add("7S", "Rockstar Games?");
            this.makerCodeHash.Add("7T", "Scholastic");
            this.makerCodeHash.Add("7U", "Ignition Entertainment");
            this.makerCodeHash.Add("7V", "Summitsoft");
            this.makerCodeHash.Add("7W", "Stadlbauer");
            this.makerCodeHash.Add("80", "Misawa");
            this.makerCodeHash.Add("81", "Teichiku");
            this.makerCodeHash.Add("82", "Namco Ltd.");
            this.makerCodeHash.Add("83", "LOZC");
            this.makerCodeHash.Add("84", "KOEI");
            this.makerCodeHash.Add("86", "Tokuma Shoten Intermedia");
            this.makerCodeHash.Add("87", "Tsukuda Original");
            this.makerCodeHash.Add("88", "DATAM-Polystar");
            this.makerCodeHash.Add("8B", "BulletProof Software (BPS)	");
            this.makerCodeHash.Add("8C", "Vic Tokai Inc.");
            this.makerCodeHash.Add("8E", "Character Soft");
            this.makerCodeHash.Add("8F", "I'Max");
            this.makerCodeHash.Add("8G", "Saurus");
            this.makerCodeHash.Add("8J", "General Entertainment");
            this.makerCodeHash.Add("8N", "Success");
            this.makerCodeHash.Add("8P", "SEGA Japan");
            this.makerCodeHash.Add("90", "Takara Amusement");
            this.makerCodeHash.Add("91", "Chun Soft");
            this.makerCodeHash.Add("92", "Video System, McO'River???");
            this.makerCodeHash.Add("93", "BEC");
            this.makerCodeHash.Add("95", "Varie");
            this.makerCodeHash.Add("96", "Yonezawa/S'pal");
            this.makerCodeHash.Add("97", "Kaneko");
            this.makerCodeHash.Add("99", "Victor Interactive Software, Pack in Video");
            this.makerCodeHash.Add("9A", "Nichibutsu/Nihon Bussan");
            this.makerCodeHash.Add("9B", "Tecmo");
            this.makerCodeHash.Add("9C", "Imagineer");
            this.makerCodeHash.Add("9F", "Nova (pirate?)");
            this.makerCodeHash.Add("9G", "Den'Z, Global Star, Take2/Magic Pocket");
            this.makerCodeHash.Add("9H", "Bottom Up");
            this.makerCodeHash.Add("9J", "TGL (Technical Group Laboratory)");
            this.makerCodeHash.Add("9L", "Hasbro Japan?");
            this.makerCodeHash.Add("9N", "Marvelous Entertainment");
            this.makerCodeHash.Add("9P", "Keynet Inc.");
            this.makerCodeHash.Add("9Q", "Hands-On Entertainment");
            this.makerCodeHash.Add("A0", "Telenet");
            this.makerCodeHash.Add("A1", "Hori");
            this.makerCodeHash.Add("A4", "Konami");
            this.makerCodeHash.Add("A5", "K.Amusement Leasing Co.");
            this.makerCodeHash.Add("A6", "Kawada");
            this.makerCodeHash.Add("A7", "Takara");
            this.makerCodeHash.Add("A9", "Technos Japan Corp.");
            this.makerCodeHash.Add("AA", "JVC (Europe/Japan?), Victor Musical Indutries");
            this.makerCodeHash.Add("AC", "Toei Animation");
            this.makerCodeHash.Add("AD", "Toho");
            this.makerCodeHash.Add("AF", "Namco");
            this.makerCodeHash.Add("AG", "Media Rings Corporation, Playmates?");
            this.makerCodeHash.Add("AH", "J-Wing");
            this.makerCodeHash.Add("AJ", "Pioneer LDC");
            this.makerCodeHash.Add("AK", "KID");
            this.makerCodeHash.Add("AL", "Mediafactory");
            this.makerCodeHash.Add("AP", "Infogrames Hudson");
            this.makerCodeHash.Add("AQ", "Kiratto. Ludic Inc");
            this.makerCodeHash.Add("B0", "Acclaim Japan");
            this.makerCodeHash.Add("B1", "ASCII, Nexoft?");
            this.makerCodeHash.Add("B2", "Bandai");
            this.makerCodeHash.Add("B4", "Enix");
            this.makerCodeHash.Add("B6", "HAL Laboratory");
            this.makerCodeHash.Add("B7", "SNK");
            this.makerCodeHash.Add("B9", "Pony Canyon (Hanbai/Inc?)");
            this.makerCodeHash.Add("BA", "Culture Brain");
            this.makerCodeHash.Add("BB", "Sunsoft");
            this.makerCodeHash.Add("BC", "Toshiba EMI");
            this.makerCodeHash.Add("BD", "Sony Imagesoft");
            this.makerCodeHash.Add("BF", "Sammy");
            this.makerCodeHash.Add("BG", "Magical");
            this.makerCodeHash.Add("BH", "Visco");
            this.makerCodeHash.Add("BJ", "Compile");
            this.makerCodeHash.Add("BL", "MTO Inc.");
            this.makerCodeHash.Add("BN", "Sunrise Interactive");
            this.makerCodeHash.Add("BP", "Global A Entertainment");
            this.makerCodeHash.Add("BQ", "Fuuki");
            this.makerCodeHash.Add("C0", "Taito");
            this.makerCodeHash.Add("C2", "Kemco (1990-92)");
            this.makerCodeHash.Add("C3", "Square");
            this.makerCodeHash.Add("C4", "Tokuma Shoten");
            this.makerCodeHash.Add("C5", "Data East");
            this.makerCodeHash.Add("C6", "Tonkin House, Tokyo Shoseki");
            this.makerCodeHash.Add("C8", "KOEI");
            this.makerCodeHash.Add("CA", "Konami/Ultra/Palcom");
            this.makerCodeHash.Add("CB", "NTVIC/VAP");
            this.makerCodeHash.Add("CC", "Use Co.,Ltd.");
            this.makerCodeHash.Add("CD", "Meldac");
            this.makerCodeHash.Add("CE", "Pony Canyon(J)/FCI(U)");
            this.makerCodeHash.Add("CF", "Angel, Sotsu Agency/Sunrise");
            this.makerCodeHash.Add("CJ", "Boss");
            this.makerCodeHash.Add("CG", "Yumedia/Aroma Co., Ltd");
            this.makerCodeHash.Add("CK", "Axela/Crea-Tech?");
            this.makerCodeHash.Add("CL", "Sekaibunka-Sha, Sumire kobo?, Marigul Management Inc.?");
            this.makerCodeHash.Add("CM", "Konami Computer Entertainment Osaka");
            this.makerCodeHash.Add("CN", "NEC Interchannel");
            this.makerCodeHash.Add("CP", "Enterbrain");
            this.makerCodeHash.Add("D0", "Taito/Disco");
            this.makerCodeHash.Add("D1", "Sofel");
            this.makerCodeHash.Add("D2", "Quest, Bothtec");
            this.makerCodeHash.Add("D3", "Sigma, ?????");
            this.makerCodeHash.Add("D4", "Ask Kodansha");
            this.makerCodeHash.Add("D6", "Naxat");
            this.makerCodeHash.Add("D7", "Copya System");
            this.makerCodeHash.Add("D8", "Capcom Co., Ltd.");
            this.makerCodeHash.Add("D9", "Banpresto");
            this.makerCodeHash.Add("DA", "TOMY");
            this.makerCodeHash.Add("DB", "LJN Japan");
            this.makerCodeHash.Add("DD", "NCS");
            this.makerCodeHash.Add("DE", "Human Entertainment");
            this.makerCodeHash.Add("DF", "Altron");
            this.makerCodeHash.Add("DG", "Jaleco???");
            this.makerCodeHash.Add("DH", "Gaps Inc.");
            this.makerCodeHash.Add("DL", "????");
            this.makerCodeHash.Add("DN", "Elf");
            this.makerCodeHash.Add("E0", "Jaleco");
            this.makerCodeHash.Add("E1", "????");
            this.makerCodeHash.Add("E2", "Yutaka");
            this.makerCodeHash.Add("E3", "Varie");
            this.makerCodeHash.Add("E4", "T&ESoft");
            this.makerCodeHash.Add("E5", "Epoch");
            this.makerCodeHash.Add("E7", "Athena");
            this.makerCodeHash.Add("E8", "Asmik");
            this.makerCodeHash.Add("E9", "Natsume");
            this.makerCodeHash.Add("EA", "King Records");
            this.makerCodeHash.Add("EB", "Atlus");
            this.makerCodeHash.Add("EC", "Epic/Sony Records(J)");
            this.makerCodeHash.Add("EE", "IGS (Information Global Service)");
            this.makerCodeHash.Add("EG", "Chatnoir");
            this.makerCodeHash.Add("EH", "Right Stuff");
            this.makerCodeHash.Add("EJ", "????");
            this.makerCodeHash.Add("EL", "Spike");
            this.makerCodeHash.Add("EM", "Konami Computer Entertainment Tokyo");
            this.makerCodeHash.Add("EN", "Alphadream Corporation");
            this.makerCodeHash.Add("EP", "Sting(?)");
            this.makerCodeHash.Add("F0", "A Wave");
            this.makerCodeHash.Add("F1", "Motown Software");
            this.makerCodeHash.Add("F2", "Left Field Entertainment");
            this.makerCodeHash.Add("F3", "Extreme Ent. Grp.");
            this.makerCodeHash.Add("F4", "TecMagik");
            this.makerCodeHash.Add("F9", "Cybersoft");
            this.makerCodeHash.Add("FB", "Psygnosis");
            this.makerCodeHash.Add("FE", "Davidson/Western Tech.");
            this.makerCodeHash.Add("FK", "The Game Factory (E)");
            this.makerCodeHash.Add("FL", "Hip Games");
            this.makerCodeHash.Add("FM", "Aspyr");
            this.makerCodeHash.Add("FP", "Mastiff?");
            this.makerCodeHash.Add("FQ", "iQue");
            this.makerCodeHash.Add("FR", "Digital Tainment Pool");
            this.makerCodeHash.Add("FS", "XS Games");
            this.makerCodeHash.Add("FT", "Daiwon");
            this.makerCodeHash.Add("G1", "PCCW Japan");
            this.makerCodeHash.Add("G4", "KiKi Co Ltd");
            this.makerCodeHash.Add("G5", "Open Sesame Inc???");
            this.makerCodeHash.Add("G6", "Sims");
            this.makerCodeHash.Add("G7", "Broccoli");
            this.makerCodeHash.Add("G8", "Avex");
            this.makerCodeHash.Add("G9", "D3 Publisher");
            this.makerCodeHash.Add("GB", "Konami Computer Entertainment Japan");
            this.makerCodeHash.Add("GD", "Square-Enix");
            this.makerCodeHash.Add("GE", "KSG?");
            this.makerCodeHash.Add("GF", "Micott & Basara Inc.");
            this.makerCodeHash.Add("GH", "Orbital Media");
            this.makerCodeHash.Add("GY", "The Game Factory (U)");
            this.makerCodeHash.Add("H1", "Treasure");
            this.makerCodeHash.Add("H2", "Aruze");
            this.makerCodeHash.Add("H3", "Ertain");
            this.makerCodeHash.Add("H4", "SNK Playmore");
            this.makerCodeHash.Add("IH", "Yojigen");
            this.makerCodeHash.Add("LH", "Trend Verlag, East Entertainment");
            this.makerCodeHash.Add("NK", "Diffusion, Neko Entertainment/Naps team.");
            this.makerCodeHash.Add("TK", "Tasuke/Works");
            this.makerCodeHash.Add("VN", "Valcon Games");        
        }
    }
}
