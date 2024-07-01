using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data
{
    public static class ValidationConstants
    {
        // Writer
        public const int WriterNameMaxLength = 20;
        public const int PseudonymMaxLength = 33;

        // Song
        public const int SongNameMaxLength = 20;

        // Album
        public const int AlbumNameMaxLength = 40;

        // Performer
        public const int PerformerFirstLastNameMaxLength = 20;

        // Producer
        public const int ProducerNameMaxLength = 30;
        public const int PhoneNumberMaxLength = 25;
    }
}
