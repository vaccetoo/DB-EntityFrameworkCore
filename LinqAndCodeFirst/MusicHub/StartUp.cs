namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportSongsAboveDuration(context, 4));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Include(a => a.Producer)
                .Include(a => a.Songs)
                .ThenInclude(s => s.Writer)
                .ToList()
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        s.Name,
                        s.Price,
                        WriterName = s.Writer.Name
                    }).OrderByDescending(s => s.Name)
                      .ThenBy(s => s.WriterName)
                      .ToList(),
                    AlbumPrice = a.Price
                })
                .OrderByDescending(a => a.AlbumPrice)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var album in albums)
            {
                result.AppendLine($"-AlbumName: {album.AlbumName}");
                result.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                result.AppendLine($"-ProducerName: {album.ProducerName}");
                result.AppendLine($"-Songs:");

                int songsCounter = 0;

                foreach (var song in album.Songs)
                {
                    songsCounter++;

                    result.AppendLine($"---#{songsCounter}");
                    result.AppendLine($"---SongName: {song.Name}");
                    result.AppendLine($"---Price: {song.Price:f2}");
                    result.AppendLine($"---Writer: {song.WriterName}");
                }

                result.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var output = new StringBuilder();

            var songs = context.Songs
                .Include(s => s.SongPerformers)
                .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .ThenInclude(a => a.Producer)
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    PerformersNames = s.SongPerformers
                                             .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}"),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToList();

            int count = 1;

            foreach (var song in songs)
            {
                // -Song #1
                //  ---SongName: Away
                // ---Writer: Norina Renihan
                // ---AlbumProducer: Georgi Milkov
                // ---Duration: 00:05:35

                output.AppendLine($"-Song #{count++}");
                output.AppendLine($"---SongName: {song.Name}");
                output.AppendLine($"---Writer: {song.WriterName}");

                if (song.PerformersNames != null)
                {
                    foreach (var name in song.PerformersNames.OrderBy(n => n))
                    {
                        output.AppendLine($"---Performer: {name}");
                    }
                }

                output.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                output.AppendLine($"---Duration: {song.Duration}");

            }

            return output.ToString().TrimEnd();
        }
    }
}
